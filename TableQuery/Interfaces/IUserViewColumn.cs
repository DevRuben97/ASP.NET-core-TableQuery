using System;


namespace TableQuery.Interfaces{

    public interface IUserViewColumn{

        int Id{get;set;}

        int ColumnId{get;set;}

        int UserViewId{get;set;}

        bool Selected{get;set;}

        IViewColumn Column{get;set;}

        IUserView View{get;set;}

    }
}