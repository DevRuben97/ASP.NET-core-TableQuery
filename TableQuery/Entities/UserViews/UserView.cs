using System;
using System.Collections.Generic;


namespace TableQuery.Entities.UserViews{

   public class UserView
    {
       public int Id { get; set; }

       public int UserId { get; set; }

       public int PageId {get;set;}
       public bool IsDefault {get;set;}

       public bool IsShowDefault {get;set;}

       public int Level {get;set;}

       public Page Page{get;set;}

        public IEnumerable<UserViewColumn> ViewColumns{get;set;}
    }
}