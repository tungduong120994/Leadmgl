using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lead_MGTValueObject.Common
{
    public class DatatableLeadImport
    {
        public DataTable CreeateTable()
        {
            DataTable dt = new DataTable();
           
            dt.Columns.Add("Phone_Number", typeof(string));
            dt.Columns.Add("Province_Code", typeof(string));
            dt.Columns.Add("CC_Band", typeof(string));
            dt.Columns.Add("CC_Limit", typeof(decimal));
            dt.Columns.Add("Scoring_CC", typeof(DateTime));
            dt.Columns.Add("UPL_Band", typeof(string));
            dt.Columns.Add("UPL_Limit", typeof(decimal));
            dt.Columns.Add("Lai_Suat", typeof(string));
            dt.Columns.Add("Scoring_UPL", typeof(DateTime));
            dt.Columns.Add("Bound_Code", typeof(string));
            dt.Columns.Add("San_Pham", typeof(string));
            return dt;
        }
    }
}


