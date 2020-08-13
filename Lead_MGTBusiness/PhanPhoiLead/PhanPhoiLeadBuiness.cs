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
using Lead_MGTValueObject.DataFillterModel;
using Lead_MGTValueObject.PhanPhoiLeadModel;
using System.Globalization;

namespace Lead_MGTBusiness.ImportLead
{
    public class PhanPhoiLeadBuiness
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
           // LayLead();
           // GetBoLocLich();
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
        public List<ChienDichModel> GetAllChienDich()
        {
            List<ChienDichModel> lis = new List<ChienDichModel>();

            SqlConnection conn = new SqlConnection();

            conn.ConnectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ConnectionString;
            conn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText =
            string.Format("select Id,MaChienDich,TenChienDich from CD_ChienDich where IsDeleted=0");
            cmd.CommandTimeout = 100000;

            SqlDataReader reader = cmd.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);
            conn.Close();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ChienDichModel row = new ChienDichModel();
                row.ChienDichId = dt.Rows[i]["Id"].ToString();
                row.MaChienDich = dt.Rows[i]["MaChienDich"].ToString();
                row.TenChienDich = dt.Rows[i]["TenChienDich"].ToString();
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
        public List<ViewPhanPhoiLeadModel> GetDataBoundCodeBySP(string sanPham ,string maChienDich)
        {
         
            List<ViewPhanPhoiLeadModel> lis = new List<ViewPhanPhoiLeadModel>();
            List<BoLocModel> lisBoLoc = new List<BoLocModel>();
            List<LichPhanPhoiLeadModel> lisLichPhanPhoi = new List<LichPhanPhoiLeadModel>();
            long? hoi = Convert.ToInt64(sanPham);
            var context = new Lead_GWEntities();
            var listPhanPhoi = context.CD_PhanPhoiLead.Where(T => T.IsDeleted == false && T.HeThongNhanLead == sanPham && T.MaChienDich==maChienDich).Select(x => x).ToList();
            if (listPhanPhoi.Count>0)
            {
                long temp = listPhanPhoi[0].Id;
                var listLichPhanPhoi = context.CD_LichPhanPhoiLead.Where(T => T.IsDeleted == false && T.PhanPhoiLeadId == temp).Select(x => x).ToList();
                var listBoLoc = context.CD_BoLocPhanPhoi.Where(T => T.IsDeleted == false && T.PhanPhoiLeadId == temp).Select(x => x).ToList();

                foreach (var item in listLichPhanPhoi)
                {
                    LichPhanPhoiLeadModel row = new LichPhanPhoiLeadModel();

                    row.CachPhanPhoi = item.CachPhanPhoiLeadId;
                    row.GioPhanPhoi = item.GioPhanPhoi;
                    row.NgayPhanPhoi = item.NgayPhanPhoi;
                    row.ThangPhanPhoi = item.ThangPhanPhoi;
                    row.SoLuong = item.SoLuong;
                    lisLichPhanPhoi.Add(row);
                }
                foreach (var item in listBoLoc)
                {
                    BoLocModel row = new BoLocModel();
                    row.Den = item.Den;
                    row.Tu = item.Tu;
                    row.TenBoLoc = item.TenBoLoc;
                    row.GiaTri = item.GiaTri;

                    lisBoLoc.Add(row);
                }
                foreach (var item in listPhanPhoi)
                {
                    ViewPhanPhoiLeadModel row = new ViewPhanPhoiLeadModel();
                    row.HeThong = item.HeThongNhanLead;
                    row.DuAn = item.MaChienDich;
                    row.MaChienDich = item.MaChienDich;
                    row.BoLoc = lisBoLoc;
                    row.LichPhanPhoi = lisLichPhanPhoi;
                    lis.Add(row);
                }
            }
           
            

            return lis;
        }

        public void LayLead()
        {
            //b1 sẽ lấy lead theo bảng chi tiết
            var context = new Lead_GWEntities();
            DateTime ngayPhanBo = new DateTime();
            long maLich = 0;
            var layPPLeadID = context.CD_PhanPhoiLead.Where(T => T.IsDeleted == false).Select(x => x).ToList();
            foreach (var item in layPPLeadID)
            {
                long phanPhoiLeadId = 0;
                if (layPPLeadID.Count > 0)
                {
                    phanPhoiLeadId = item.Id;
                }
                var layLichPPLeadId = context.CD_LichPhanPhoiLead.Where(T => T.IsDeleted == false && T.PhanPhoiLeadId == phanPhoiLeadId).Select(x => x).ToList();
                foreach (var item1 in layLichPPLeadId)
                {
                    if (item1.CachPhanPhoiLeadId == 1)
                    {
                        maLich = item1.Id;

                    }
                    else if (item1.CachPhanPhoiLeadId == 2)
                    {
                        maLich = item1.Id;
                        var lichLeadConLai1212 = context.CD_PhanPhoiChiTiet.Where(T => T.IsDeleted == false && T.LichPhanPhoiId == maLich).Select(x => x).ToList();
                        if (lichLeadConLai1212.Count>0)
                        {
                            if (lichLeadConLai1212[0].NgayChuyen.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                GetBoLocLich(phanPhoiLeadId, 0);
                            }
                        }
                       

                    }
                    else if (item1.CachPhanPhoiLeadId == 3)
                    {

                        int lastDayOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                        if (item1.NgayPhanPhoi <= lastDayOfMonth)
                        {
                            ngayPhanBo = new DateTime(Convert.ToInt16(item1.NamPhanPhoi), Convert.ToInt16(item1.ThangPhanPhoi), Convert.ToInt16(item1.NgayPhanPhoi));
                            if (ngayPhanBo.ToString("MM/dd/yyyy") == DateTime.Now.ToString("MM/dd/yyyy"))
                            {
                                maLich = item1.Id;
                            }
                        }


                    }

                }
                if (maLich!=0)
                {
                    ChuyenLeadChoDayAPI(maLich);
                }
                

            }

            
            var layLead = context.CD_PhanPhoiChiTiet.Where(T => T.IsDeleted == false).Select(x => x).ToList();

            var lichLeadConLai = context.CD_PhanPhoiChiTiet.Where(T => T.IsDeleted == false).Select(x => x).ToList();


            StringBuilder sbQueryImportLead = new StringBuilder("");
            if (layLead.Count>0)
            {
                if (layLead[0].SoLuongCauHinh > layLead[0].SoLuongDaChuyen && layLead[0].SoLuongCanChuyen > 0)
                {

                    sbQueryImportLead.Append(layLead[0].QuerySoLuong);
                    sbQueryImportLead.Append(layLead[0].SoLuongCanChuyen);
                    sbQueryImportLead.Append(layLead[0].QueryDieuKien);

                }
                string a = sbQueryImportLead.ToString();
                //b2 sẽ update lại số lead đã đẩy qua CRM
            }

        }
        public void GetBoLocLich(long? maLichPP,int soLuongDaChuyen)
        {
            DateTime ngayPhanBo = new DateTime();
            DateTime ngayPhanBo1 = new DateTime();
            var db = new Lead_GWEntities();
            StringBuilder sbQueryImportLead = new StringBuilder("");
            //update thong tin nhan lead 
            sbQueryImportLead.Append("select top(");
            string querSoLuong = sbQueryImportLead.ToString();
            //db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
            var context = new Lead_GWEntities();
            int SoLuong = 0;
            int cachChuyen =0;
            //int soLuongDaChuyen = 0;
            long maLich = 0;
            //tạm thời số lượng đã chuyển =0 , logic sau này sẽ lấy số lượng đã chuyển của lịch bắn đang còn active nếu khi khách hàng có thay đổi cấu hình bắn
            //lấy lịch ở bảng :CD_PhanPhoiChiTiet


            var storedBoLoc = context.CD_BoLocPhanPhoi.Where(T => T.IsDeleted == false && T.PhanPhoiLeadId==maLichPP).Select(x => x).ToList();
            var storedLich = context.CD_LichPhanPhoiLead.Where(T => T.IsDeleted == false && T.PhanPhoiLeadId == maLichPP).Select(x => x).ToList();
           
            var lichLeadConLai = context.CD_PhanPhoiChiTiet.Where(T => T.IsDeleted == false && T.CachChuyen!=1).Select(x => x).ToList();
  
           // soLuongDaChuyen = lichLeadConLai[0].SoLuongDaChuyen;
            
            foreach (var item in storedLich)
            {
                if (item.CachPhanPhoiLeadId==1)
                {
                    cachChuyen = Convert.ToInt16( item.CachPhanPhoiLeadId);
                    ngayPhanBo1 = DateTime.Now;
                    SoLuong = 100000;
                    maLich = item.Id;

                }
                else if (item.CachPhanPhoiLeadId == 2)
                {
                    cachChuyen = Convert.ToInt16(item.CachPhanPhoiLeadId);
                    ngayPhanBo1 = DateTime.Now;
                    SoLuong = Convert.ToInt32(item.SoLuong);
                    maLich = item.Id;
                }
                else if (item.CachPhanPhoiLeadId == 3)
                {

                    int lastDayOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                    if (item.NgayPhanPhoi <= lastDayOfMonth)
                    {
                        ngayPhanBo = new DateTime(Convert.ToInt16(item.NamPhanPhoi), Convert.ToInt16(item.ThangPhanPhoi), Convert.ToInt16(item.NgayPhanPhoi));

                        cachChuyen = Convert.ToInt16(item.CachPhanPhoiLeadId);

                        if (ngayPhanBo.ToString("MM/dd/yyyy") == DateTime.Now.ToString("MM/dd/yyyy"))
                        {
                            ngayPhanBo1 = new DateTime(Convert.ToInt16(item.NamPhanPhoi), Convert.ToInt16(item.ThangPhanPhoi), Convert.ToInt16(item.NgayPhanPhoi));
                            SoLuong = Convert.ToInt32(item.SoLuong);
                            maLich = item.Id;
                            //sbQueryImportLead.Append("top(" + item.SoLuong + ") * from  Lead  ");
                        }
                    }


                }
                
            }
            sbQueryImportLead = new StringBuilder("");
            sbQueryImportLead.Append(") * from  Lead  where 1=1  and Transfer_To is null and  Status_DuplicateID =1 and ( 1=1 ");
            sbQueryImportLead.Append("  and ( ");
            StringBuilder sbQueryImportLead12 = new StringBuilder("");
            string abcd2 = "";
            foreach (var item in storedBoLoc)
            {
                if (item.TenBoLoc == "Bound_Code" && item.GiaTri != "")
                {
                    sbQueryImportLead12.Append(" or    " + item.TenBoLoc + "  =  ''" + item.GiaTri + "''");
                    
                }
                abcd2 = sbQueryImportLead12.ToString();
            }
            string bcd2 = abcd2.Remove(1, 3);
            sbQueryImportLead.Append(bcd2);
            sbQueryImportLead.Append("  )  ");

            sbQueryImportLead.Append("  and (  ");

            StringBuilder sbQueryImportLead1 = new StringBuilder("");
            string abcd = "";
            foreach (var item in storedBoLoc)
            {
                if (item.TenBoLoc == "Province_Code" && item.GiaTri != "")
                {

                    sbQueryImportLead1.Append(" or    " + item.TenBoLoc + "  =  ''" + item.GiaTri + "''");
                }
                
                //


            }
            abcd = sbQueryImportLead1.ToString();
            string bcd = abcd.Remove(1, 3);
            sbQueryImportLead.Append(bcd);
            sbQueryImportLead.Append("  )  ");

            foreach (var item in storedBoLoc)
            {
                if (item.GiaTri != null  && item.TenBoLoc != "Bound_Code" && item.TenBoLoc != "Province_Code" && item.GiaTri!="")
                {
                    sbQueryImportLead.Append("  and   " + item.TenBoLoc + "  =  ''" + item.GiaTri + "''");
                }
                else if (item.GiaTri == null && item.Den!=null && item.Tu !=null && item.GiaTri != "")
                {
                    sbQueryImportLead.Append("  and   " + item.TenBoLoc + "  BETWEEN  " + item.Tu + "   and "+item.Den+"");
                }
               
            }
            sbQueryImportLead.Append("  )  ");
            int soLuongCanChuyen = SoLuong - soLuongDaChuyen;
            string queryDieuKien = sbQueryImportLead.ToString();


            sbQueryImportLead = new StringBuilder("");
            //insert thong tin phan phoi
            sbQueryImportLead.Append("INSERT INTO CD_PhanPhoiChiTiet(QuerySoLuong, QueryDieuKien, SoLuongCauHinh,SoLuongDaChuyen,SoLuongCanChuyen,NgayChuyen,CachChuyen,LichPhanPhoiId, IsActive, IsDeleted, CreationTime ) VALUES (");
            sbQueryImportLead.Append("'"+querSoLuong+"',");
            sbQueryImportLead.Append("'"+queryDieuKien+"',");
            sbQueryImportLead.Append(SoLuong + ",");
            sbQueryImportLead.Append(soLuongDaChuyen+ ",");
            sbQueryImportLead.Append(soLuongCanChuyen + ",");
            sbQueryImportLead.Append("N'" + ngayPhanBo1 + "',");
            sbQueryImportLead.Append(cachChuyen + ",");
            sbQueryImportLead.Append(maLich + ",");
            sbQueryImportLead.Append("1,");
            sbQueryImportLead.Append("0,");
            sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
            string abc = sbQueryImportLead.ToString();

            sbQueryImportLead = new StringBuilder("");
            sbQueryImportLead.Append("UPDATE CD_PhanPhoiChiTiet set IsActive = 0 ,IsDeleted =1 , DeletionTime ='" + DateTime.Now + "' ,LastModificationTime='" + DateTime.Now + "' where LichPhanPhoiId ="+maLich+" ");
           
            db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

            db.Database.ExecuteSqlCommand(abc);

            
            ChuyenLeadChoDayAPI(maLich);

        }
        public void ChuyenLeadChoDayAPI(long maLich)
        {
            var context = new Lead_GWEntities();
            var lichLeadConLai = context.CD_PhanPhoiChiTiet.Where(T => T.IsDeleted == false && T.LichPhanPhoiId==maLich).Select(x => x).ToList();
            long abo =0;
            if (lichLeadConLai.Count >0)
            {
                abo = lichLeadConLai[0].LichPhanPhoiId;
            }
            else
            {
                var malichID =context.CD_LichPhanPhoiLead.Where(T => T.IsDeleted == false && T.Id == maLich).Select(x => x).ToList();
                GetBoLocLich(malichID[0].PhanPhoiLeadId, 0);
                lichLeadConLai = context.CD_PhanPhoiChiTiet.Where(T => T.IsDeleted == false && T.LichPhanPhoiId == maLich).Select(x => x).ToList();
                if (lichLeadConLai.Count > 0)
                {
                    abo = lichLeadConLai[0].LichPhanPhoiId;
                }
            }

            var tam = context.CD_LichPhanPhoiLead.Where(T => T.IsDeleted == false && T.Id == abo).Select(x => x).ToList();
            long sa = 0;
            if (tam.Count  >0)
            {
                sa = tam[0].PhanPhoiLeadId;
            }
           
            var storedLich = context.CD_PhanPhoiLead.Where(T => T.IsDeleted == false && T.Id== sa).Select(x => x).ToList();

            StringBuilder sbQueryTranferLead = new StringBuilder("");
            sbQueryTranferLead.Append(lichLeadConLai[0].QuerySoLuong);
            if (Convert.ToInt32(lichLeadConLai[0].SoLuongCanChuyen)>0)
            {
                sbQueryTranferLead.Append(lichLeadConLai[0].SoLuongCanChuyen);
            }
            else
            {
                sbQueryTranferLead.Append(0);
            }
            
            sbQueryTranferLead.Append(lichLeadConLai[0].QueryDieuKien);
            
            string eIu = sbQueryTranferLead.ToString();


            string a111 = sbQueryTranferLead.ToString();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText =
            string.Format(a111);
            cmd.CommandTimeout = 100000;
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable tbLead = objData.TranferTableLead();
            var dt = new DataTable();
            tbLead.Load(reader);
            conn.Close();

           
            string a = "";
            string strgroupids = "";
            for (int i = 0; i < tbLead.Rows.Count; i++)
            {
                a = a + "" + tbLead.Rows[i]["Id"].ToString() + ",";
                 strgroupids = a.Remove(a.Length - 1);
                tbLead.Rows[i]["cc_code"] = storedLich[0].MaChienDich;
                //DataRow _ravi = tbLead.NewRow();
                //_ravi["id"] = dt.Rows[i]["Id"].ToString();
                //_ravi["cc_code"] = storedLich[0].MaChienDich;
                //_ravi["band_cc"] = "B"+dt.Rows[i]["Band_CC"].ToString();
                //_ravi["band_upl"] ="B"+ dt.Rows[i]["Band_UPL"].ToString();
                //_ravi["bound_code"] = dt.Rows[i]["Bound_Code"].ToString();
                //_ravi["cc_limit"] = dt.Rows[i]["CC_Limit"].ToString();
                //_ravi["email"] = dt.Rows[i]["Email"].ToString();
                ////_ravi["income_level"] = dt.Rows[i]["Income_Level"].ToString();
                //_ravi["phone_number"] = dt.Rows[i]["Phone_Number"].ToString();
                //_ravi["post_date"] = dt.Rows[i]["Post_Date"].ToString();
                //_ravi["score_range"] = dt.Rows[i]["Score_Range"].ToString();
                //_ravi["upl_interest"] = dt.Rows[i]["UPL_Interest"].ToString();
                //string assssbc= dt.Rows[i]["UPL_Limit"].ToString();
                //decimal? agg = null;
                //agg = Convert.ToDecimal( dt.Rows[i]["UPL_Interest"].ToString());
                //_ravi["upl_limit"] = agg;
                //_ravi["valid_date"] = dt.Rows[i]["Valid_Date"].ToString();
                //_ravi["province_code"] = dt.Rows[i]["Province_Code"].ToString();
                //tbLead.Rows.Add(_ravi);

            }
            

            string conString = string.Empty;
            conString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    sqlBulkCopy.DestinationTableName = "GW_Tranfer_CRM";
                    sqlBulkCopy.ColumnMappings.Add("id", "id");
                    sqlBulkCopy.ColumnMappings.Add("bound_code", "bound_code");
                    sqlBulkCopy.ColumnMappings.Add("band_cc", "band_cc");
                    sqlBulkCopy.ColumnMappings.Add("band_upl", "band_upl");
                    sqlBulkCopy.ColumnMappings.Add("cc_code", "cc_code");
                    sqlBulkCopy.ColumnMappings.Add("cc_limit", "cc_limit");
                    sqlBulkCopy.ColumnMappings.Add("email", "email");
                    sqlBulkCopy.ColumnMappings.Add("income_level", "income_level");
                    sqlBulkCopy.ColumnMappings.Add("phone_number", "phone_number");
                    sqlBulkCopy.ColumnMappings.Add("post_date", "post_date");
                    sqlBulkCopy.ColumnMappings.Add("score_range", "score_range");
                    sqlBulkCopy.ColumnMappings.Add("upl_interest", "upl_interest");
                    sqlBulkCopy.ColumnMappings.Add("upl_limit", "upl_limit");
                    sqlBulkCopy.ColumnMappings.Add("valid_date", "valid_date");
                    sqlBulkCopy.ColumnMappings.Add("province_code", "province_code");


                    con.Open();
                    sqlBulkCopy.WriteToServer(tbLead);
                    con.Close();
                }

            }
            sbQueryTranferLead = new StringBuilder("");
            sbQueryTranferLead.Append("UPDATE Lead SET Transfer_To = 'TF_CRM' ,Transfer_Date='" + DateTime.Now + "'  where Id   in ( " + strgroupids + ")");
            if (strgroupids!="")
            {
                context.Database.ExecuteSqlCommand(sbQueryTranferLead.ToString());
            }
            
            int temp = lichLeadConLai[0].SoLuongCanChuyen - tbLead.Rows.Count;
            int temp1= lichLeadConLai[0].SoLuongDaChuyen + tbLead.Rows.Count;
            sbQueryTranferLead = new StringBuilder("");
            sbQueryTranferLead.Append("UPDATE CD_PhanPhoiChiTiet SET SoLuongDaChuyen = "+ temp1 + " , SoLuongCanChuyen = " + temp + " where IsDeleted=0  and LichPhanPhoiId = "+maLich+"");
            context.Database.ExecuteSqlCommand(sbQueryTranferLead.ToString());
        }

        //Luu trang thai lich phan phoi
        public List<SavePhanPhoiLeadModel> Save(SavePhanPhoiLeadModel sanPham)
        {
            int bandcctu, banupltu, cclimittu, upllimittu, bandccden, banuplden, cclimitden, upllimitden;
            if (sanPham.BandCCTu ==null)
            {
                bandcctu = 1;
            }
            else
            {
                bandcctu = Convert.ToInt32( sanPham.BandCCTu);
            }
            if (sanPham.BandCCDen == null)
            {
                bandccden = 20;
            }
            else
            {
                bandccden = Convert.ToInt32(sanPham.BandCCDen);
            }
            if (sanPham.BandUPLTu == null)
            {
                banupltu = 1;
            }
            else
            {
                banupltu = Convert.ToInt32(sanPham.BandUPLTu);
            }
            if (sanPham.BandUPLDen == null)
            {
                banuplden = 20;
            }
            else
            {
                banuplden = Convert.ToInt32(sanPham.BandUPLDen);
            }
            if (sanPham.CCLimitTu == null)
            {
                cclimittu = 1;
            }
            else
            {
                cclimittu = Convert.ToInt32(sanPham.CCLimitTu);
            }
            if (sanPham.CCLimitDen == null)
            {
                cclimitden = 100000000;
            }
            else
            {
                cclimitden = Convert.ToInt32(sanPham.CCLimitDen);
            }
            if (sanPham.UPLLimitTu == null)
            {
                upllimittu = 1;
            }
            else
            {
                upllimittu = Convert.ToInt32(sanPham.UPLLimitTu);
            }
            if (sanPham.UPLLimitDen == null)
            {
                upllimitden = 1000000000;
            }
            else
            {
                upllimitden = Convert.ToInt32(sanPham.UPLLimitDen);
            }
            var context = new Lead_GWEntities();
           string q = string.Empty;
            var layPPLeadID = context.CD_PhanPhoiLead.Where(T => T.IsDeleted == false && T.MaChienDich == sanPham.MaChienDich).Select(x => x).ToList();
            long phanPhoiLeadId = 0;
            if (layPPLeadID.Count>0)
            {
                phanPhoiLeadId = layPPLeadID[0].Id;
            }
            var layLichPPLeadId =context.CD_LichPhanPhoiLead.Where(T => T.IsDeleted == false && T.PhanPhoiLeadId==phanPhoiLeadId).Select(x => x).ToList();
            long lichPPId = 0;
            if (layLichPPLeadId.Count==1 )
            {
                lichPPId = layLichPPLeadId[0].Id;
            }
            else
            {
                foreach (var item in layLichPPLeadId)
                {
                    if (DateTime.Now.Day== item.NgayPhanPhoi &&DateTime.Now.Month==item.ThangPhanPhoi &&DateTime.Now.Year==item.NamPhanPhoi )
                    {
                        lichPPId = item.Id;
                    }
                }
            }

            var layPPLChiTiet =context.CD_PhanPhoiChiTiet.Where(T => T.IsDeleted == false && T.LichPhanPhoiId == lichPPId).Select(x => x).ToList();
            int asss = 0;
            if (layPPLChiTiet.Count >0)
            {
                asss = layPPLChiTiet[0].SoLuongDaChuyen;
            }
            

            var db = new Lead_GWEntities();
            StringBuilder sbQueryImportLead = new StringBuilder("");
            //update thong tin nhan lead 
            sbQueryImportLead.Append("UPDATE CD_PhanPhoiLead set IsActive = 0 ,IsDeleted =1 , DeletionTime ='"+DateTime.Now+ "' ,LastModificationTime='" + DateTime.Now + "'  where HeThongNhanLead = " + sanPham.HeThong+ " and MaChienDich = '"+sanPham.MaChienDich+"'");
            q = sbQueryImportLead.ToString();
            db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
            sbQueryImportLead = new StringBuilder("");
            //insert thong tin phan phoi
            sbQueryImportLead.Append("INSERT INTO CD_PhanPhoiLead(HeThongNhanLead, DuAnID, MaChienDich, IsActive, IsDeleted, CreationTime ) VALUES (");
            sbQueryImportLead.Append(sanPham.HeThong + ",");
            sbQueryImportLead.Append( sanPham.DuAn +",");
            sbQueryImportLead.Append("'" + sanPham.MaChienDich+ "',");
            sbQueryImportLead.Append("1,");
            sbQueryImportLead.Append("0,");
            sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
            q =sbQueryImportLead.ToString();
            db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());


            var listPhanPhoi = context.CD_PhanPhoiLead.Where(T => T.IsDeleted == false && T.HeThongNhanLead == sanPham.HeThong && T.MaChienDich == sanPham.MaChienDich).Select(x => x).ToList();
            //update Bo loc theo phanphoiid
            sbQueryImportLead = new StringBuilder("");
            sbQueryImportLead.Append("UPDATE CD_BoLocPhanPhoi set IsActive = 0 ,IsDeleted =1 , DeletionTime ='" + DateTime.Now + "' ,LastModificationTime='" + DateTime.Now + "'  where PhanPhoiLeadId = " + listPhanPhoi[0].Id + "");
            q = sbQueryImportLead.ToString();
            db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

            //insert thong tin bo loc boundCode
            for (int i = 0; i < sanPham.Bound_Code.Count; i++)
            {

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_BoLocPhanPhoi(PhanPhoiLeadId, TenBoLoc, GiaTri,  IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append("'Bound_Code',");
                sbQueryImportLead.Append("'" + sanPham.Bound_Code[i] + "',");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
                
            }
            //insert thong tin bo loc Province
            for (int i = 0; i < sanPham.Province_Code.Count; i++)
            {

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_BoLocPhanPhoi(PhanPhoiLeadId, TenBoLoc, GiaTri,  IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append("'Province_Code',");
                sbQueryImportLead.Append("'" + sanPham.Province_Code[i] + "',");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
               
            }
            //insert thong tin bo loc LaiXuat
            sbQueryImportLead = new StringBuilder("");
            sbQueryImportLead.Append("INSERT INTO CD_BoLocPhanPhoi(PhanPhoiLeadId, TenBoLoc, GiaTri,  IsActive, IsDeleted, CreationTime ) VALUES (");
            sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
            sbQueryImportLead.Append("'UPL_Interest',");
            sbQueryImportLead.Append("'" + sanPham.UPL_Interest + "',");
            sbQueryImportLead.Append("1,");
            sbQueryImportLead.Append("0,");
            sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
            q = sbQueryImportLead.ToString();
            db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

            //insert thong tin bo loc PostDate
            if (sanPham.Post_Date!=null && sanPham.Post_Date!="")
            {
                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_BoLocPhanPhoi(PhanPhoiLeadId, TenBoLoc, GiaTri,  IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append("'Post_Date',");
                sbQueryImportLead.Append("'" + DateTime.Parse(sanPham.Post_Date, CultureInfo.CreateSpecificCulture("fr-FR")).ToString("yyyy-MM-dd") + "',");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

            }
            

            //insert thong tin bo loc ValidDate\
            if (sanPham.Valid_Date!=null && sanPham.Valid_Date!="")
            {
                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_BoLocPhanPhoi(PhanPhoiLeadId, TenBoLoc, GiaTri,  IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append("'Valid_Date',");
                sbQueryImportLead.Append("'" + DateTime.Parse(sanPham.Valid_Date, CultureInfo.CreateSpecificCulture("fr-FR")).ToString("yyyy-MM-dd") + "',");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
            }
           

            //insert thong tin bo loc PartnerCode
            sbQueryImportLead = new StringBuilder("");
            sbQueryImportLead.Append("INSERT INTO CD_BoLocPhanPhoi(PhanPhoiLeadId, TenBoLoc, GiaTri,  IsActive, IsDeleted, CreationTime ) VALUES (");
            sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
            sbQueryImportLead.Append("'PartnerCode',");
            sbQueryImportLead.Append("'" + sanPham.PartnerCode + "',");
            sbQueryImportLead.Append("1,");
            sbQueryImportLead.Append("0,");
            sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
            q = sbQueryImportLead.ToString();
            db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

            if (sanPham.CCLimitTu != null && sanPham.CCLimitDen != null)
            {
                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_BoLocPhanPhoi(PhanPhoiLeadId, TenBoLoc, Tu ,Den,  IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append("'CC_Limit',");
                sbQueryImportLead.Append("'" + cclimittu + "',");
                sbQueryImportLead.Append("'" + cclimitden + "',");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
            }
            //insert thong tin bo loc CCLimit
          

            //insert thong tin bo loc BandCC
            if (sanPham.BandCCTu != null && sanPham.BandCCDen != null)
            {


                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_BoLocPhanPhoi(PhanPhoiLeadId, TenBoLoc, Tu ,Den,  IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append("'Band_CC',");
                sbQueryImportLead.Append("'" + bandcctu + "',");
                sbQueryImportLead.Append("'" + bandccden + "',");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
            }
            //insert thong tin bo loc UPLLimit
            if (sanPham.UPLLimitTu != null && sanPham.UPLLimitDen != null)
            {
                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_BoLocPhanPhoi(PhanPhoiLeadId, TenBoLoc, Tu ,Den , IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append("'UPL_Limit',");
                sbQueryImportLead.Append("'" + upllimittu + "',");
                sbQueryImportLead.Append("'" + upllimitden + "',");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
            }
            if (sanPham.BandUPLTu != null && sanPham.BandUPLDen != null)
            {
                //insert thong tin bo loc BandUPL
                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_BoLocPhanPhoi(PhanPhoiLeadId, TenBoLoc, Tu ,Den , IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append("'Band_UPL',");
                sbQueryImportLead.Append("'" + banupltu + "',");
                sbQueryImportLead.Append("'" + banuplden + "',");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
            }
            //update lich theo phanphoiid
            sbQueryImportLead = new StringBuilder("");
            sbQueryImportLead.Append("UPDATE CD_LichPhanPhoiLead set IsActive = 0 ,IsDeleted =1 , DeletionTime ='" + DateTime.Now + "' ,LastModificationTime='" + DateTime.Now + "'  where PhanPhoiLeadId = " + listPhanPhoi[0].Id + "");
            q = sbQueryImportLead.ToString();
            db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

            //insert lich phan phoi
            if (sanPham.CachPhanPhoi=="1")
            {
                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId ,PhanPhoiLeadId , IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append( "5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
            }
            if (sanPham.CachPhanPhoi == "2")
            {
                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong,PhanPhoiLeadId , IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",");
                sbQueryImportLead.Append(sanPham.SoLuong + ",");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
            }
            if (sanPham.CachPhanPhoi == "3")
            {
                
                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID1 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi+1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());


                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID2 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("2,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID3 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("3,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID4 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("4,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID5 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID6 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("6,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID7 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("7,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID8 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("8,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID9 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("9,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID10 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("10,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID11 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("11,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID12 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("12,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID13 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("13,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID14 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("14,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID15 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("15,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID16 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("16,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID17 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("17,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID18 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("18,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID19 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("19,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID20 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("20,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID21 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("21,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID22 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("22,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID23 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("23,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID24 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("24,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID25 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("25,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID26 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("26,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID27 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("27,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID28 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("28,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID29 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("29,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID30 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("30,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());

                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_LichPhanPhoiLead( GioPhanPhoi ,CachPhanPhoiLeadId, SoLuong ,PhanPhoiLeadId ,ThangPhanPhoi,NamPhanPhoi, NgayPhanPhoi, IsActive, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("5,");
                sbQueryImportLead.Append(sanPham.CachPhanPhoi + ",'");
                sbQueryImportLead.Append(sanPham.ID31 + "',");
                sbQueryImportLead.Append(listPhanPhoi[0].Id + ",");
                sbQueryImportLead.Append(sanPham.ThangPhanPhoi + 1 + ",");
                sbQueryImportLead.Append(sanPham.NamPhanPhoi + ",");
                sbQueryImportLead.Append("31,");
                sbQueryImportLead.Append("1,");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                q = sbQueryImportLead.ToString();
                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());



            }
            GetBoLocLich(listPhanPhoi[0].Id,asss);
            return null;
        }

        }
}
