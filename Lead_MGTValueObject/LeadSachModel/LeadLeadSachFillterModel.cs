using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lead_MGTValueObject.LeadSachModel
{
    public class LeadSachFillterModel
    {
        public string Phone_Number { get; set; }
        public string Bound_Code { get; set; }
        public string Post_Date_From { get; set; }
        public string Post_Date_To { get; set; }
        public string Province { get; set; }
        public string Valid_Date_From { get; set; }
        public string Valid_Date_To { get; set; }
        public string DuAn { get; set; }
        public string PartnerCode { get; set; }
        public string CampaignCode { get; set; }
        public string SourceApiExcel { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
