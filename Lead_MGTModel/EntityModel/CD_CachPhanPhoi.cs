//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class CD_CachPhanPhoi
    {
        public long Id { get; set; }
        public string TenCachPhanPhoi { get; set; }
        public Nullable<long> PhanPhoiLeadId { get; set; }
        public Nullable<bool> LichPhanPhoi { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreationTime { get; set; }
        public Nullable<int> CreatorUserId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> DeleterUserId { get; set; }
        public Nullable<System.DateTime> DeletionTime { get; set; }
        public Nullable<System.DateTime> LastModificationTime { get; set; }
        public Nullable<int> LastModifierUserId { get; set; }
    }
}
