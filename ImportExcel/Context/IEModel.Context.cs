﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ImportExcel.Context
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TestCoreFrameEntities : DbContext
    {
        public TestCoreFrameEntities()
            : base("name=TestCoreFrameEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tblEducation> tblEducations { get; set; }
        public virtual DbSet<GBS_CATEGORY_MATRIX> GBS_CATEGORY_MATRIX { get; set; }
        public virtual DbSet<COMPANy> COMPANIES { get; set; }
        public virtual DbSet<COUNTRy> COUNTRIES { get; set; }
        public virtual DbSet<USER> USERS { get; set; }
        public virtual DbSet<TEAM> TEAMS { get; set; }
    }
}
