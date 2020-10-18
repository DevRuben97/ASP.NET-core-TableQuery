using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableQuery.Entities;

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
    }
}
