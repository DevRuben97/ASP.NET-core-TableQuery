using System;
using System.Collections.Generic;

namespace TableQuery.Interfaces{

    public interface IViewColumn{
        int Id {get;set;}

        int PageId{get;set;}
        string Name {get;set;}

        string FieldId{get;set;}

        bool Default{get;set;}

        int Level{get;set;}

        //Navigacion Properties:

        IPage Page{get;set;}
         IEnumerable<IUserViewColumn> ViewColumns{get;set;}
    }
}