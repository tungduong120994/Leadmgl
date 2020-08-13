using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lead_MGTValueObject.ImportLeadModel
{
    public class ImportLeadModel
    {
        public string Phone_Number { get; set; }
        public string Province_Code { get; set; }
        public decimal CC_Band { get; set; }
        public string CC_Limit { get; set; }
        public DateTime Scoring_CC { get; set; }
        public string UPL_Band { get; set; }
        public decimal UPL_Limit { get; set; }
        public string Lai_Suat { get; set; }
        public DateTime Scoring_UPL { get; set; }
        public string Bound_Code { get; set; }
        public string San_Pham { get; set; }        
        //public DateTime Validate_CC { get; set; }
        //public DateTime Validate_UPL { get; set; }


    }
}
