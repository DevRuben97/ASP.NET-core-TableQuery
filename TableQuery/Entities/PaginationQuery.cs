using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace TableQuery.Entities
{
  public  class PaginationQuery
    {
        /// <summary>
        /// Numero de pagina
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Tamaño de la pagina. Por ejemplo 10 por pagina 
        /// en un grid con 20 registros devolvera 2 paginas.
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Propiedad para realizar la busqueda entre todos los campos del grid actual.
        /// </summary>
        public string Search { get; set; }
        /// <summary>
        /// Propiedad de tipo Json Object el cual puede recibir cualquier objecto 
        /// con N cantidad de propiedad para realizar el filtrado entre los campos
        /// del grid actual.
        /// </summary>
        public List<FilterQuery> Filters { get; set; }
        /// <summary>
        /// Filtrado por defecto para aplicarse al momento de hacer el primer 
        /// get del grid actual.
        /// </summary>
        public List<FilterQuery> DefaultFilter { get; set; }
        /// <summary>
        /// Objeto para realizar el ordenamiento de los registros
        /// en base a un campo de lista del grid actual
        /// </summary>
        public OrderQuery OrderBy { get; set; }

        public PaginationQuery()
        {
            PageNumber = 1;
            PageSize = 20;
            Filters = new List<FilterQuery>();
            OrderBy = new OrderQuery()
            {
                By = "Id",
                IsDescending = true
            };
        }
        public PaginationQuery(int PageNumber, int PageSize)
        {
            this.PageNumber = PageNumber;
            this.PageSize = PageSize;
        }
        public PaginationQuery(int PageNumber, int PageSize, List<FilterQuery> filters, OrderQuery order)
        {
            this.PageNumber = PageNumber;
            this.PageSize = PageSize;
            this.Filters = filters;
            this.OrderBy = order;
        }
    }
}
