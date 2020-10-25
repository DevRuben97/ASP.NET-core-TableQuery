using System;
using System.Collections.Generic;


namespace TableQuery.Interfaces{

   public interface IUserView
    {
        int Id { get; set; }

        int UserId { get; set; }

        int PageId {get;set;}
        bool IsDefault {get;set;}

        bool IsShowDefault {get;set;}

        int Level {get;set;}

        IPage Page{get;set;}

         IEnumerable<IUserViewColumn> ViewColumns{get;set;}
    }
}