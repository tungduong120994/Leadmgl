using Lead_MGTValueObject.ImportLeadModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using System.Configuration;
using Lead_MGTModel.EntityModel;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Lead_MGTValueObject.Common;
using System.Text;
using System.Text.RegularExpressions;
using Lead_MGTValueObject.PhanPhoiLeadModel;
using Lead_MGTValueObject.DataFillterModel;

namespace Lead_MGTBusiness.GetAllDataFillter
{
    public class GetAllDataFillterBuiness
    {
        List<ImportLeadModel> lis = new List<ImportLeadModel>();
        DatatableLead objData = new DatatableLead();
        DataTableToList objCommom = new DataTableToList();
        public StringBuilder sbThongBaoLoiTemp = new StringBuilder("");
        public StringBuilder sbThongBaoLoi = new StringBuilder("");
        Dictionary<string, string> resultKiemTraLoiDuLieu = new Dictionary<string, string>();
      
        
        public List<DuAnDataFillterModel> GetAllDataDuAn()
        {
            List<DuAnDataFillterModel> lis = new List<DuAnDataFillterModel>();
            var context = new Lead_GWEntities();


            var storedCheckDeDup = context.DA_DuAn.Where(T => T.IsDeleted == false).Select(x => x).ToList();

            foreach (var item in storedCheckDeDup)
            {
                DuAnDataFillterModel row = new DuAnDataFillterModel();
                row.Text = item.TenDuAn;
                row.Value = Convert.ToString( item.Id);
                lis.Add(row);
            }

            return lis;
        }
        public List<BoundCodeDataFillterModel> GetAllDataBoundCode()
        {
            List<BoundCodeDataFillterModel> lis = new List<BoundCodeDataFillterModel>();
            var context = new Lead_GWEntities();


            var storedCheckDeDup = context.DM_BoundCode.Where(T => T.IsDeleted == false).Select(x => x).ToList();

            foreach (var item in storedCheckDeDup)
            {
                BoundCodeDataFillterModel row = new BoundCodeDataFillterModel();
                row.Text = item.Bound_Code;
                row.Value = Convert.ToString(item.Id);
                lis.Add(row);
            }

            return lis;
        }
        public List<TinhThanhModel> GetAllDataProvice()
        {
            List<TinhThanhModel> lis = new List<TinhThanhModel>();

            SqlConnection conn = new SqlConnection();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
            conn.Open();
        
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText =
            string.Format("select * from Province_maping");
            cmd.CommandTimeout = 100000;

            SqlDataReader reader = cmd.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);
            conn.Close();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TinhThanhModel row = new TinhThanhModel();
                row.MaTinhThanh = dt.Rows[i]["area_code"].ToString();
                row.TenTinhThanh = dt.Rows[i]["area_name"].ToString();
                lis.Add(row);
            }
            return lis;
        }
        public List<BoundCodeDataFillterModel> GetDataBoundCodeBySP(string sanPham)
        {
            int a = Convert.ToInt32(sanPham);
            List<BoundCodeDataFillterModel> lis = new List<BoundCodeDataFillterModel>();
            var context = new Lead_GWEntities();


            var storedCheckDeDup = context.DM_BoundCode.Where(T => T.IsDeleted == false && T.SanPhamId == a).Select(x => x).ToList();

            foreach (var item in storedCheckDeDup)
            {
                BoundCodeDataFillterModel row = new BoundCodeDataFillterModel();
                row.Text = item.Bound_Code;
                row.Value = Convert.ToString(item.Id);
                lis.Add(row);
            }

            return lis;
        }

    }
}
