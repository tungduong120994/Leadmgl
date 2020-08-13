using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lead_MGTValueObject.PhanPhoiLeadModel
{
    public class ViewPhanPhoiLeadModel
    {
        public string HeThong { get; set; }
        public string DuAn { get; set; }
        public string MaChienDich { get; set; }
        public List<BoLocModel> BoLoc { get; set; }
        public List<LichPhanPhoiLeadModel> LichPhanPhoi { get; set; }

    }
}
