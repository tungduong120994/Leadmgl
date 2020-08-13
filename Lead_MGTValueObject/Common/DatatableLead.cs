using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lead_MGTValueObject.Common
{
    public class DatatableLead
    {
        public DataTable CreeateTableLead()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Address1", typeof(string));
            dt.Columns.Add("Band_CC", typeof(string));
            dt.Columns.Add("Band_UPL", typeof(decimal));
            dt.Columns.Add("Bound_Code", typeof(string));
            dt.Columns.Add("CC_Limit", typeof(decimal));
            dt.Columns.Add("DoiTacId", typeof(int));
            dt.Columns.Add("DuAnID", typeof(long));
            dt.Columns.Add("Created_Id", typeof(int));
            dt.Columns.Add("Created_Date", typeof(DateTime));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Income_Level", typeof(decimal));
            dt.Columns.Add("LastModifierUserId", typeof(int));
            dt.Columns.Add("LastModificationTime", typeof(DateTime));
            dt.Columns.Add("Lead_Name", typeof(string));
            dt.Columns.Add("National_Id", typeof(string));
            dt.Columns.Add("Phone_Number", typeof(string));
            dt.Columns.Add("Post_Date", typeof(DateTime));
            dt.Columns.Add("Province", typeof(string));
            dt.Columns.Add("Province_Code", typeof(string));
            dt.Columns.Add("Score_Range", typeof(string));
            dt.Columns.Add("UPL_Interest", typeof(float));
            dt.Columns.Add("UPL_Limit", typeof(decimal));
            dt.Columns.Add("Valid_Date", typeof(DateTime));
            dt.Columns.Add("Transfer_To", typeof(string));
            dt.Columns.Add("Transfer_Date", typeof(DateTime));
            dt.Columns.Add("District_Code", typeof(string));
            dt.Columns.Add("Process_DuplicateID", typeof(int));
            dt.Columns.Add("Status_DuplicateID", typeof(int));
            dt.Columns.Add("Description1", typeof(string));
            dt.Columns.Add("Description2", typeof(string));
            dt.Columns.Add("Description3", typeof(string));
            dt.Columns.Add("Description4", typeof(string));
            dt.Columns.Add("Lead_Code", typeof(string));
            dt.Columns.Add("Lead_GW_Id", typeof(int));
            return dt;
        }
        public DataTable TranferTableLead()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("id", typeof(Int64));
            dt.Columns.Add("cc_code", typeof(string));
            dt.Columns.Add("band_cc", typeof(string));
            dt.Columns.Add("band_upl", typeof(string));
            dt.Columns.Add("bound_code", typeof(string));
            dt.Columns.Add("cc_limit", typeof(decimal));
            dt.Columns.Add("email", typeof(string));
            dt.Columns.Add("income_level", typeof(Int16));
            dt.Columns.Add("phone_number", typeof(string));
            dt.Columns.Add("post_date", typeof(DateTime));
            dt.Columns.Add("score_range", typeof(string));
            dt.Columns.Add("upl_interest", typeof(float));
            dt.Columns.Add("upl_limit", typeof(decimal));
            dt.Columns.Add("valid_date", typeof(DateTime));
            dt.Columns.Add("province_code", typeof(string));
            return dt;
        }
    }
}


