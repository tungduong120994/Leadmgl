using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lead_MGTValueObject.PhanPhoiLeadModel
{
    public class LichPhanPhoiLeadModel
    {
        public string NgayPhanPhoiLead { get; set; }
        public int? GioPhanPhoi { get; set; }
        public int? SoLuong { get; set; }
        public long? CachPhanPhoi { get; set; }
        public string PhanPhoiLeadId { get; set; }

        public int? NgayPhanPhoi { get; set; }
        public int? ThangPhanPhoi { get; set; }
        public int? NamPhanPhoi { get; set; }
        
    }
}
