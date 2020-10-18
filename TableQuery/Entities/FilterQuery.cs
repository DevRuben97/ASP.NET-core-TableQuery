using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace TableQuery.Entities
{
    public class FilterQuery
    {
        /// <summary>
        /// Nombre del campo 
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Operador a utilizar para el filtrado. Por ejemplo ==, != , >=, <=.
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// En caso de ser necesario la función a utilizar para realizar el filtro. Por ejemplo .ToString()
        /// </summary>
        public string Function { get; set; }
        /// <summary>
        /// El value para realizar el filtrado
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Validar si el valor suministrado es una fecha.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDate(string value)
        {
            try
            {
                string[] dates = value.Split("/");
                var date = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Metodo para convertir Un Json object a un objeto de tipo filtrado 
        /// para que despues se realice el filtro requerido
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<FilterQuery> ConvertFromJsonObject(JsonElement json)
        {
            //Validar si dentro del objeto hay propiedades con value diferente de nulo.
            var props = json.EnumerateObject().ToList().Where(x => x.Value.GetString() != null).ToList();
            var filters = new List<FilterQuery>();
            var IsDateRange = props.Where(x => IsDate(x.Value.GetString())).Count() == 2; //Determinar si se requiere un filtro por rango de fechas
            var rowIndex = 0;
            var dateRange = 0;

            foreach (var prop in props) //Iterar las propiedad con value valido para realizar el filtrado
            {
                var filter = new FilterQuery();
                filter.FieldName = prop.Name;
                filter.Value = prop.Value.GetString();

                //En caso de que el filtrado sea por el id:
                if (filter.Value.All(char.IsDigit) && filter.FieldName.ToLower().Contains("id"))
                {
                    filter.Operator = "==";
                }
                else if (filter.Value.All(char.IsDigit)) //En caso de que el filtrado se por numeros
                {
                    filter.Operator = "==";
                }
                else if (IsDate(filter.Value)) //En caso de que el filtrado sea por rango de fechas:
                {
                    if (IsDateRange)
                    {
                        if (dateRange == 0)
                        {
                            filter.Operator = ">=";
                            dateRange += 1;
                        }
                        else if (dateRange == 1)
                        {
                            filter.Operator = "<=";
                            dateRange = 0;
                        }
                    }
                    else
                    {
                        filter.Operator = "==";
                    }

                }
                else if (!filter.Value.All(char.IsDigit)) //En caso de que el filtrado no sea por numeros
                {
                    filter.Function = $".ToString().ToLower().Contains(@{rowIndex})";
                    filter.Value = filter.Value.ToLower();
                    filter.Operator = null;
                }
                rowIndex += 1;
                //set the filter:
                filters.Add(filter); //Agregar al listado de filtros
            }

            return filters;
        }
    }
}
