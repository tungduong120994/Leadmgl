﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lead_MGTModel.EntityModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Lead_GWEntities : DbContext
    {
        public Lead_GWEntities()
            : base("name=Lead_GWEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AbpUser> AbpUsers { get; set; }
        public virtual DbSet<CD_DieuKienCheckDuAn> CD_DieuKienCheckDuAn { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<DA_DuAn> DA_DuAn { get; set; }
        public virtual DbSet<DM_BoundCode> DM_BoundCode { get; set; }
        public virtual DbSet<DM_DoiTac> DM_DoiTac { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Lead> Leads { get; set; }
        public virtual DbSet<Lead_Status> Lead_Status { get; set; }
        public virtual DbSet<Lead_Version> Lead_Version { get; set; }
        public virtual DbSet<CD_BoLocPhanPhoi> CD_BoLocPhanPhoi { get; set; }
        public virtual DbSet<CD_CachPhanPhoi> CD_CachPhanPhoi { get; set; }
        public virtual DbSet<CD_DieuKienCheckBoundCode> CD_DieuKienCheckBoundCode { get; set; }
        public virtual DbSet<CD_LichPhanPhoiLead> CD_LichPhanPhoiLead { get; set; }
        public virtual DbSet<CD_PhanPhoiLead> CD_PhanPhoiLead { get; set; }
        public virtual DbSet<GW_Tranfer_CRM> GW_Tranfer_CRM { get; set; }
        public virtual DbSet<Lead_GW> Lead_GW { get; set; }
        public virtual DbSet<Lead_GW_Compare> Lead_GW_Compare { get; set; }
        public virtual DbSet<Lead_GW_Compare_copy1> Lead_GW_Compare_copy1 { get; set; }
        public virtual DbSet<Lead_GW_copy1> Lead_GW_copy1 { get; set; }
        public virtual DbSet<Lead_GW_Process> Lead_GW_Process { get; set; }
        public virtual DbSet<Lead_Import> Lead_Import { get; set; }
        public virtual DbSet<CD_PhanPhoiChiTiet> CD_PhanPhoiChiTiet { get; set; }
    }
}
