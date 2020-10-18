using System;
using System.Collections.Generic;
using System.Text;

namespace TableQuery.Entities
{
  public  class OrderQuery
    {
        /// <summary>
        /// Nombre del campo para realizar el ordenamiento
        /// </summary>
        public string By { get; set; }
        /// <summary>
        /// Especificar si es Ascendente o Descendente
        /// </summary>
        public bool IsDescending { get; set; }
    }
}
