using System;
using System.Collections.Generic;


namespace TableQuery.Entities{

     public class TableView
    {
        public bool IsDefault { get; set; }

        public string ModuleCode { get; set; }

        public string ModuleName { get; set; }

        public int Level { get; set; }

        public List<Column> Columns { get; set; }
    }
}