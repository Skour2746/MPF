﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mpf.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class mpfdbEntities1 : DbContext
    {
        public mpfdbEntities1()
            : base("name=mpfdbEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AdminLogin> AdminLogins { get; set; }
        public virtual DbSet<AnserTbl> AnserTbls { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<DegreeTbl> DegreeTbls { get; set; }
        public virtual DbSet<EventMgmt> EventMgmts { get; set; }
        public virtual DbSet<Loc_Districtn> Loc_Districtn { get; set; }
        public virtual DbSet<Loc_State> Loc_State { get; set; }
        public virtual DbSet<NewsTbl> NewsTbls { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Slider> Sliders { get; set; }
        public virtual DbSet<Upcoming_Events> Upcoming_Events { get; set; }
        public virtual DbSet<UserQuesAn> UserQuesAns { get; set; }
        public virtual DbSet<UserRegister> UserRegisters { get; set; }
    }
}