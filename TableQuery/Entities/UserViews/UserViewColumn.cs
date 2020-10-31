using System;


namespace TableQuery.Entities.UserViews{

    public class UserViewColumn{

       public int Id{get;set;}

       public int ColumnId{get;set;}

       public int UserViewId{get;set;}

       public bool Selected{get;set;}

       public ViewColumn Column{get;set;}

       public UserView View{get;set;}

    }
}