using System;
using System.Collections.Generic;

namespace TableQuery.Entities.UserViews{

    public class ViewColumn{
       public int Id {get;set;}

       public int PageId{get;set;}
       public string Name {get;set;}

       public string FieldId{get;set;}

       public bool Default{get;set;}

       public int Level{get;set;}

        //Navigacion Properties:

       public  Page Page{get;set;}
       public  IEnumerable<UserViewColumn> ViewColumns{get;set;}
    }
}