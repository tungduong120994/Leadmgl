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
    
    public partial class CD_PhanPhoiChiTiet
    {
        public long Id { get; set; }
        public string QuerySoLuong { get; set; }
        public string QueryDieuKien { get; set; }
        public int SoLuongCauHinh { get; set; }
        public int SoLuongDaChuyen { get; set; }
        public int SoLuongCanChuyen { get; set; }
        public System.DateTime NgayChuyen { get; set; }
        public int CachChuyen { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreationTime { get; set; }
        public Nullable<int> CreatorUserId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> DeleterUserId { get; set; }
        public Nullable<System.DateTime> DeletionTime { get; set; }
        public Nullable<System.DateTime> LastModificationTime { get; set; }
        public Nullable<int> LastModifierUserId { get; set; }
        public long LichPhanPhoiId { get; set; }
    }
}
