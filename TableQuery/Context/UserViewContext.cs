using System;
using Microsoft.EntityFrameworkCore;
using TableQuery.Entities.UserViews;

namespace TableQuery.Context{

    public interface IUserViewContext<PageEntity, UserViewEntity, UserViewColumnsEntity, ViewColumnsEntity> 
    where PageEntity: Page
    where UserViewEntity: UserView
    where UserViewColumnsEntity: UserViewColumn
    where ViewColumnsEntity: ViewColumn
    {

        DbSet<PageEntity> Pages {get;set;}

        DbSet<UserViewEntity> UserViews{get;set;}

        DbSet<UserViewColumnsEntity> UserViewColumns{get;set;}

        DbSet<ViewColumnsEntity> ViewColumns{get;set;}
    }
}