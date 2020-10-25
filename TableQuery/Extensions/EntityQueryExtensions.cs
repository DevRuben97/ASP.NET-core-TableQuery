using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableQuery.Entities;
using TableQuery.Interfaces;

namespace TableQuery.Extensions
{
   public static class EntityQueryExtensions
    {
        /// <summary>
        /// Metodo que devuelve una lista paginada de datos.
        /// </summary>
        /// <typeparam name="T">Clase o tipo ha obtener los datos</typeparam>
        /// <param name="Query"></param>
        /// <param name="pagination">Instancia de la clase PaginationQuery que contiene los parametros necesarios para realizar la paginación</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static async Task<PagedResult> ToListPagedAsync<T>(this IQueryable<T> Query, PaginationQuery pagination) where T : class
        {
            var result = new PagedResult();

            result.PageNumber = pagination.PageNumber;
            result.PageSize = pagination.PageSize;

            //En caso de que exista un busqueda de datos:
            if (!string.IsNullOrEmpty(pagination.Search))
            {
                List<string> nameProperties = new List<string>();
                var type = Query.ElementType;
                foreach (var item in type.GetProperties()) //Iterar las propiedades de la clase actual.
                {
                    nameProperties.Add(item.Name);
                }

                Query = ExpressionBuilder.Search(Query, nameProperties.ToArray(), pagination.Search);
            }

            //Aplicar el filtro por defecto:
            if (pagination.DefaultFilter!= null && pagination.DefaultFilter.Count> 0)
            {
                
                Query = ExpressionBuilder.CreateFilterQuery(Query, pagination.DefaultFilter.ToArray());
            }

            //En caso de exista un filtro se aplicara:
            if (pagination.Filters!= null && pagination.Filters.Count> 0) //Verificar si existen filtros
            {
                Query = ExpressionBuilder.CreateFilterQuery(Query, pagination.Filters.ToArray());
                result.Filters = pagination.Filters;
            }
            else
            {
                result.Filters = new List<FilterQuery>();
            }

            //Order by:
            if (Query.ElementType.GetProperties().Where(x => x.Name.ToLower().Equals("id")).Count() > 0)
            {
                Query = ExpressionBuilder.CreateOrderQuery(Query, pagination.OrderBy.By, pagination.OrderBy.IsDescending);
            }
            //Total de los registros
            result.TotalRecords = await Query.CountAsync();
            //Conteo de las paginas:
            var pageCount = (double)result.TotalRecords / result.PageSize;
            result.TotalPages = (int)Math.Ceiling(pageCount); //Calcular el total de paginas en base al conteo actual de los registros totales

            result.NextPage = (result.TotalPages > 1) ? (pagination.PageNumber + 1) : 1; //Determinar la siguiente pagina
            result.PreviousPage = (result.TotalPages > 1) ? (pagination.PageNumber - 1) : 1; //Determinar la pagina anterior

            /* Calcular la cantidad de registros que se saltara en funcion 
             * de la pagina actual y la cantidad de registros por pagina
             */
            var skip = (pagination.PageNumber - 1) * pagination.PageSize;
            result.Data = await Query.Skip(skip).Take(pagination.PageSize).ToListAsync();

            return result;
        }
        /// <summary>
        /// Get the columns of the selected View
        /// </summary>
        /// <param name="paged">Instance of Paged Entity</param>
        /// <param name="dbContext">Instance of DbContext</param>
        /// <param name="userId">The id of the user</param>
        /// <param name="PageCode">The code of the page</param>
        /// <param name="level">Lavel of the page, for create a more of 1 table on a page.</param>
        /// <returns></returns>
        public static async Task<GridResponse> SetColumnsAsync(this PagedResult paged, DbContext dbContext, int userId, string PageCode, int level= 0){
            var grid = GridResponse.Convert(paged);

            //Get the actual View:
            var data = await dbContext.Set<IUserView>()
              .Include(x => x.Page)
              .Include(x => x.ViewColumns).ThenInclude(x => x.Column)
              .Where(x => x.UserId== userId && x.Page.Code== PageCode && x.IsDefault== x.IsShowDefault && x.Level== level)
              .FirstOrDefaultAsync();

            //Get the count of views:
            var viewsCount =await dbContext.Set<IUserView>().Where(x => x.UserId == userId && x.Page.Code == PageCode && x.Level == level).CountAsync();

            var view = new TableView();
            List<Column> columns = new List<Column>();
            // In case there is no user view (It is a new user, it has never entered the current grid or they deleted the user's view)
          if (data== null)
            {
                // Get the columns of the current view:
                var columnsModel = await dbContext.Set<IViewColumn>().Include(x => x.Page).Where(x => x.Page.Code == PageCode && x.Default && x.Level == level).ToListAsync();
                //In case there is no user view, one will be created with the columns of the default view.
                if (viewsCount== 0)
                {
                    var Newview= Activator.CreateInstance<IUserView>();
                    Newview.UserId= userId;
                    Newview.PageId= columnsModel.FirstOrDefault().Page.Id;
                    Newview.IsDefault= true;
                    Newview.IsShowDefault= true;
                    Newview.Level= level;
                    
                    //Set the columns of default view:
                    var NewColumsn= new List<IUserViewColumn>();
                    foreach (var item in columnsModel)
                    {
                        var newColumn= Activator.CreateInstance<IUserViewColumn>();
                        newColumn.ColumnId= item.Id;
                        newColumn.Selected= true;
                        NewColumsn.Add(newColumn);
                    }

                    Newview.ViewColumns= NewColumsn;

                    dbContext.Set<IUserView>().Add(Newview);

                    await dbContext.SaveChangesAsync();
                }

                 columns = columnsModel.Select(x => new Column()
                {
                    FieldId = x.FieldId,
                    Name = x.Name
                }).ToList();

                 view = new TableView()
                {
                    IsDefault = true,
                    Level= level,
                    ModuleCode = PageCode,
                    ModuleName = columnsModel.FirstOrDefault().Page.Name,
                    Columns = columns
                };

            }
            else
            {
                // If there is a view, the current view is returned with its corresponding columns (Default or Custom):
                columns = data.ViewColumns.Where(x => x.Selected).OrderBy(x=> x.Column.Id).Select(x => new Column()
                {
                    Name = x.Column.Name,
                    FieldId = x.Column.FieldId
                }).ToList();

                view = new TableView()
                {
                    IsDefault = data.IsDefault,
                    Level= level,
                    ModuleCode = data.Page.Code,
                    ModuleName = data.Page.Name,
                    Columns = columns
                };

            }


            grid.ViewInfo = view;


            return grid;
        }
    }
}
