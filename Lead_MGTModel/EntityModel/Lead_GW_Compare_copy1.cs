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
    
    public partial class Lead_GW_Compare_copy1
    {
        public long id { get; set; }
        public long leadid { get; set; }
        public string address1 { get; set; }
        public string band_cc { get; set; }
        public string band_upl { get; set; }
        public string bound_code { get; set; }
        public Nullable<decimal> cc_limit { get; set; }
        public string company_id { get; set; }
        public Nullable<int> created_by_user_id { get; set; }
        public Nullable<System.DateTime> created_on_date { get; set; }
        public string cust_limit_id { get; set; }
        public string email { get; set; }
        public Nullable<short> exists { get; set; }
        public Nullable<int> income_level { get; set; }
        public Nullable<int> last_modified_by_user_id { get; set; }
        public Nullable<System.DateTime> last_modified_on_date { get; set; }
        public string lead_name { get; set; }
        public Nullable<decimal> limit { get; set; }
        public string national_id_number { get; set; }
        public string partner_id { get; set; }
        public string phone_number { get; set; }
        public Nullable<System.DateTime> post_date { get; set; }
        public Nullable<long> post_id { get; set; }
        public string province { get; set; }
        public string score_range { get; set; }
        public Nullable<double> upl_interest { get; set; }
        public Nullable<decimal> upl_limit { get; set; }
        public Nullable<System.DateTime> valid_date { get; set; }
        public Nullable<int> dwh_exists { get; set; }
        public string transfer_to { get; set; }
        public string province_code { get; set; }
        public string district_code { get; set; }
        public string tower_code { get; set; }
        public Nullable<System.DateTime> transfer_date { get; set; }
        public Nullable<int> in_active { get; set; }
        public Nullable<int> gr_bound_code { get; set; }
        public string project { get; set; }
        public Nullable<int> gr_project { get; set; }
    }
}
