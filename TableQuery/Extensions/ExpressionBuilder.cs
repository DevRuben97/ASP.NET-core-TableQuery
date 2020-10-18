using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using TableQuery.Entities;

namespace TableQuery.Extensions
{
  internal static  class ExpressionBuilder
    {
        /// <summary>
        /// Metodo para crear los filtros en los registros del tipo del grid actual.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filters">Listado de los filtros a realizar</param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IQueryable<T> CreateFilterQuery<T>(IQueryable<T> query, params FilterQuery[] filters)
        {
            var stringQuerybuilder = new StringBuilder();
            int variableCount = 0;
            foreach (var filter in filters) //Iterar los filtros
            {
                if (variableCount >= 1)
                {
                    stringQuerybuilder.Append(" and "); //Agregar un separador en caso de exista mas de un filftro.
                }
                //Create the query string:
                if (!string.IsNullOrEmpty(filter.Operator))
                {
                    stringQuerybuilder.Append(filter.FieldName);
                    stringQuerybuilder.Append(filter.Operator);
                    stringQuerybuilder.Append($"@{variableCount}");
                    //Ejemplo: Name== @0 and Date= @1 
                }
                else
                {// If not filelName.Contains(@0)
                    if (filter.Function.Equals("Contains()")){
                        stringQuerybuilder.Append($"Convert.ToString({filter.FieldName}).ToLower().Contains(@{variableCount})");
                    }
                    else{
                        stringQuerybuilder.Append(filter.Function);
                    }
                    
                }
                variableCount += 1;
            }
            //Setear los valores de los filtros:
            var filtervalues = new List<string>();
            foreach (var item in filters)
            {
                filtervalues.Add(item.Value);
            }
            query = query.Where(stringQuerybuilder.ToString(), filtervalues.ToArray());

            return query;
        }
        /// <summary>
        /// Realizar la busqueda rapida de todas las propiedades suministradas de tipo del grid actual.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="properties"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IQueryable<T> Search<T>(IQueryable<T> query, string[] properties, string value)
        {

            var stringQuerybuilder = new StringBuilder();
            int variableCount = 0;

            for (int i = 0; i < properties.Count(); i++)
            {
                if (variableCount >= 1)
                {
                    stringQuerybuilder.Append(" || ");
                }
                //stringQuerybuilder.Append(properties[i]);
                stringQuerybuilder.Append($"Convert.ToString({properties[i]}).ToLower().Contains(@0)");

                variableCount += 1;
            }

            query = query.Where(stringQuerybuilder.ToString(), value.ToLower());

            return query;

        }

        /// <summary>
        /// Metodo Crear un  query de ordenamiento en base a los parametros suministrados
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="by">Nombre de la propiedad para realizar el ordenamiento</param>
        /// <param name="desc">Indica si es ascendente o descendente.</param>
        /// <returns></returns>
        public static IQueryable<T> CreateOrderQuery<T>(IQueryable<T> query, string by, bool desc)
        {
            var type = (desc) ? "desc" : "asc";
            query = query.OrderBy($"{by} {type}");

            return query;
        }
    }
}
