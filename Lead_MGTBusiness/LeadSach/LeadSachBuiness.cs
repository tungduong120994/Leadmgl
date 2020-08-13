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
using Lead_MGTValueObject.LeadSachModel;

namespace Lead_MGTBusiness.LeadSach
{
    public class LeadSachBuiness 
    {
        List<ImportLeadModel> lis = new List<ImportLeadModel>();
        DatatableLead objData = new DatatableLead();
        DataTableToList objCommom = new DataTableToList();
       
        public List<ImportLeadModel> DatatableToList(DataTable dt)
        {

            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ImportLeadModel row = new ImportLeadModel();
                row.Phone_Number = dt.Rows[i]["Phone_Number"].ToString();
                row.Province_Code = dt.Rows[i]["Province_Code"].ToString();
                if (dt.Rows[i]["CC_Band"].ToString()!=null || dt.Rows[i]["CC_Band"].ToString()!="")
                {
                    row.CC_Band =Convert.ToDecimal( dt.Rows[i]["CC_Band"].ToString());
                }
                
                row.CC_Limit = dt.Rows[i]["CC_Limit"].ToString();
                if (dt.Rows[i]["Scoring_CC"].ToString() != null || dt.Rows[i]["Scoring_CC"].ToString() != "")
                {
                    row.Scoring_CC = Convert.ToDateTime(dt.Rows[i]["Scoring_CC"].ToString());
                }
                
                row.UPL_Band = dt.Rows[i]["UPL_Band"].ToString();
                if (dt.Rows[i]["UPL_Limit"].ToString() != null || dt.Rows[i]["UPL_Limit"].ToString() != "")
                {
                    row.UPL_Limit = Convert.ToDecimal(dt.Rows[i]["UPL_Limit"].ToString());
                }
                if (dt.Rows[i]["Scoring_UPL"].ToString() != null || dt.Rows[i]["Scoring_UPL"].ToString() != "")
                {
                    row.Scoring_UPL = Convert.ToDateTime(dt.Rows[i]["Scoring_UPL"].ToString());
                }
                
                row.Lai_Suat = dt.Rows[i]["Lai_Suat"].ToString();
                row.Bound_Code = dt.Rows[i]["Bound_Code"].ToString();
                row.San_Pham = dt.Rows[i]["San_Pham"].ToString();

                //if (dt.Rows[i]["San_Pham"].ToString()=="CC")
                //{
                //    if (dt.Rows[i]["Scoring_CC"].ToString() == null || dt.Rows[i]["Scoring_CC"].ToString() == "")
                //    {
                        
                        
                //    }
                //    else
                //    {
                //        row.Validate_CC= Convert.ToDateTime(dt.Rows[i]["Scoring_CC"].ToString()).AddDays(61);
                //        DateTime a = Convert.ToDateTime(dt.Rows[i]["Scoring_CC"].ToString()).AddDays(61);
                //        if (a>DateTime.Now)
                //        {
                //            if (dt.Rows[i]["Scoring_UPL"].ToString() == null || dt.Rows[i]["Scoring_UPL"].ToString() == "")
                //            {
                //                row.UPL_Band = "";
                //                row.UPL_Limit = "";
                //                row.Lai_Suat = "";
                //            }
                //            else
                //            {
                //                DateTime b = (Convert.ToDateTime(dt.Rows[i]["Scoring_UPL"].ToString())).AddDays(61);

                //                if (b < DateTime.Now)
                //                {
                //                    row.UPL_Band = "";
                //                    row.UPL_Limit = "";
                //                    row.Lai_Suat = "";

                //                }
                //            }
                //        }
                //    }
                //}
                //if (dt.Rows[i]["San_Pham"].ToString() == "UPL")
                //{
                //    row.Validate_UPL = Convert.ToDateTime(dt.Rows[i]["Scoring_UPL"].ToString()).AddDays(61);
                //}
                lis.Add(row);

            }
            return lis;

        }
        public List<ListLeadSachModel> Index(LeadSachFillterModel a)
        {
            List<ListLeadSachModel> lis = new List<ListLeadSachModel>();
            var db = new Lead_GWEntities();
            StringBuilder sbQueryfn = new StringBuilder("");
            sbQueryfn.Append(sbQueryfn);

            sbQueryfn.Append("SELECT top(1000) l.Phone_Number,l.Province_Code,l.UPL_Limit, l.Band_CC,l.Bound_Code ,l.CC_Limit , l.Band_UPL  , l.UPL_Interest , l.Valid_Date , l.DuAnID , d.TenDuAn  FROM[Lead_MGL].[dbo].[Lead] l  join DA_DuAn d on d.Id = l.DuAnID  where  l.Status_DuplicateID =1");
            if (a.Post_Date_From != null)
            {
                sbQueryfn.Append(" and  l.Created_Date > '" + a.Post_Date_From + "'");
            }
            if (a.Post_Date_To != null)
            {
                sbQueryfn.Append(" and  l.Created_Date < '" + a.Post_Date_To + "'");
            }
            if (a.Valid_Date_From != null)
            {
                sbQueryfn.Append(" and  l.Valid_Date > '" + a.Valid_Date_From + "'");
            }
            if (a.Valid_Date_To != null)
            {
                sbQueryfn.Append(" and  l.Valid_Date < '" + a.Valid_Date_To + "'");
            }

            if (a.Phone_Number != null)
            {
                sbQueryfn.Append("  and l.Phone_Number = '" + a.Phone_Number + "'");
            }
            if (a.Bound_Code != null)
            {
                sbQueryfn.Append(" and l.Bound_Code  ='" + a.Bound_Code + "'");
            }
            if (a.Province != null)
            {
                sbQueryfn.Append("  and l.Province_Code ='" + a.Province + "'");
            }
            if (a.DuAn != null)
            {
                sbQueryfn.Append("  and d.TenDuAn ='" + a.DuAn + "'");
            }

            string a111 = sbQueryfn.ToString();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText =
            string.Format(a111);
            cmd.CommandTimeout = 100000;

            SqlDataReader reader = cmd.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);
            conn.Close();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListLeadSachModel rowMail = new ListLeadSachModel();
                rowMail.band_cc = dt.Rows[i]["Band_CC"].ToString();
                rowMail.band_upl = dt.Rows[i]["Band_UPL"].ToString();
                rowMail.bound_code = dt.Rows[i]["Bound_Code"].ToString();
                rowMail.cc_limit = dt.Rows[i]["CC_Limit"].ToString();
                rowMail.upl_interest = dt.Rows[i]["UPL_Interest"].ToString();
                rowMail.phone_number = dt.Rows[i]["Phone_Number"].ToString();
                rowMail.upl_limit = dt.Rows[i]["UPL_Limit"].ToString();
                rowMail.valid_date = dt.Rows[i]["Valid_Date"].ToString();
                rowMail.province_code = dt.Rows[i]["Province_Code"].ToString();

                lis.Add(rowMail);

            }

            return lis;

        }
        public  List<ListLeadSachModel> Search(LeadSachFillterModel a)
        {
            List<ListLeadSachModel> lis = new List<ListLeadSachModel>();
            var db = new Lead_GWEntities();
            StringBuilder sbQueryfn = new StringBuilder("");
            sbQueryfn.Append(sbQueryfn);

            sbQueryfn.Append("SELECT l.* , d.TenDuAn  FROM[Lead_MGL].[dbo].[Lead] l  join DA_DuAn d on d.Id = l.DuAnID  where    l.Status_DuplicateID =1");
            if(a.Post_Date_From !=null)
            {
                sbQueryfn.Append(" and  l.Created_Date > ='"+a.Post_Date_From + "'");
            }
            if (a.Post_Date_To   != null)
            {
                sbQueryfn.Append(" and  l.Created_Date <= '" + a.Post_Date_To + "'");
            }
            if (a.Valid_Date_From != null)
            {
                sbQueryfn.Append(" and  l.Valid_Date >= '" + a.Valid_Date_From + "'");
            }
            if (a.Valid_Date_To != null)
            {
                sbQueryfn.Append(" and  l.Valid_Date <= '" + a.Valid_Date_To + "'");
            }

            if (a.Phone_Number != null)
            {
                sbQueryfn.Append("  and l.Phone_Number = '" + a.Phone_Number + "'");
            }
            if (a.Bound_Code != null)
            {
                sbQueryfn.Append(" and l.Bound_Code  ='" + a.Bound_Code + "'");
            }
            if (a.Province != null)
            {
                sbQueryfn.Append("  and l.Province_Code ='" + a.Province + "'");
            }
            if (a.DuAn != null)
            {
                sbQueryfn.Append("  and d.TenDuAn ='" + a.DuAn + "'");
            }
            sbQueryfn.Append(" Order By l.Id desc ");

            string a111 = sbQueryfn.ToString();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText =
            string.Format(a111);
            cmd.CommandTimeout = 100000;

            SqlDataReader reader = cmd.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);
            conn.Close();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListLeadSachModel rowMail = new ListLeadSachModel();
                rowMail.band_cc = dt.Rows[i]["Band_CC"].ToString();
                rowMail.band_upl = dt.Rows[i]["Band_UPL"].ToString();
                rowMail.bound_code = dt.Rows[i]["Bound_Code"].ToString();
                rowMail.cc_limit = dt.Rows[i]["CC_Limit"].ToString();
                rowMail.upl_interest = dt.Rows[i]["UPL_Interest"].ToString();
                rowMail.phone_number = dt.Rows[i]["Phone_Number"].ToString();
                rowMail.upl_limit = dt.Rows[i]["UPL_Limit"].ToString();
                rowMail.valid_date = dt.Rows[i]["Valid_Date"].ToString();
                rowMail.province_code = dt.Rows[i]["Province_Code"].ToString();
                rowMail.post_date = dt.Rows[i]["post_date"].ToString();

                lis.Add(rowMail);

            }

            return lis;

        }
        public void BulkInsertLead_Import(DataTable dt)
        {
            string conString = string.Empty;
            conString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    sqlBulkCopy.DestinationTableName = "Lead_Import";
                    sqlBulkCopy.ColumnMappings.Add("Phone_Number", "Phone_Number");
                    sqlBulkCopy.ColumnMappings.Add("Province_Code", "Province_Code");
                    sqlBulkCopy.ColumnMappings.Add("CC_Band", "CC_Band");
                    sqlBulkCopy.ColumnMappings.Add("CC_Limit", "CC_Limit");
                    sqlBulkCopy.ColumnMappings.Add("Scoring_CC", "Scoring_CC");
                    sqlBulkCopy.ColumnMappings.Add("UPL_Band", "UPL_Band");
                    sqlBulkCopy.ColumnMappings.Add("UPL_Limit", "UPL_Limit");
                    sqlBulkCopy.ColumnMappings.Add("Lai_Suat", "Lai_Suat");
                    sqlBulkCopy.ColumnMappings.Add("Scoring_UPL", "Scoring_UPL");
                    sqlBulkCopy.ColumnMappings.Add("Bound_Code", "Bound_Code");
                    sqlBulkCopy.ColumnMappings.Add("San_Pham", "San_Pham");

                    con.Open();
                    sqlBulkCopy.WriteToServer(dt);
                    con.Close();
                }
            }
            Lead_GWEntities objEntity = new Lead_GWEntities();
            // var context = new Lead_GWEntities();
            /// lstEmp = objEntity.Lead_GW_Compare.ToList();
            var context = new Lead_GWEntities();
            var user = objEntity.Lead_Import;
            //var asass = context.
            var storesList = context.Lead_Import.Where(T => T.StatusTransfer == null).Select(x => x).ToList();
           // var storesListID = context.Lead_Import.Select(x => x.Id).ToList();
            TransferLead(storesList);
            var storedCheckDeDup = context.CD_DieuKienCheckDuAn.Where(T => T.IsDeleted == false && T.DuAnId == 5).ToList();

            var storesListID = context.Lead_Import.Where(T => T.StatusTransfer == null).Select(x => x.Id).ToList();
            //TransferLead(storesList);



            var db = new Lead_GWEntities();
            foreach (var item in storesListID)
            {
                StringBuilder sbQueryfn = new StringBuilder("Update [Lead_MGL].[dbo].[Lead_Import] set StatusTransfer =1 ,TransferDate= GETDATE()  where (StatusTransfer =0 or StatusTransfer is NULL) and Id =" + item + "  ");

                db.Database.ExecuteSqlCommand(sbQueryfn.ToString());
            }
        }

        public void BulkInsertLead(DataTable dt)
        {
            string conString = string.Empty;
            conString = ConfigurationManager.ConnectionStrings["MySQLConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    sqlBulkCopy.DestinationTableName = "Lead";
                    sqlBulkCopy.ColumnMappings.Add("Address1", "Address1");
                    sqlBulkCopy.ColumnMappings.Add("Band_CC", "Band_CC");
                    sqlBulkCopy.ColumnMappings.Add("Band_UPL", "Band_UPL");
                    sqlBulkCopy.ColumnMappings.Add("Bound_Code", "Bound_Code");
                    sqlBulkCopy.ColumnMappings.Add("CC_Limit", "CC_Limit");
                    sqlBulkCopy.ColumnMappings.Add("DoiTacId", "DoiTacId");
                    sqlBulkCopy.ColumnMappings.Add("DuAnID", "DuAnID");
                    sqlBulkCopy.ColumnMappings.Add("Created_Id", "Created_Id");
                    sqlBulkCopy.ColumnMappings.Add("Created_Date", "Created_Date");
                    sqlBulkCopy.ColumnMappings.Add("Email", "Email");
                    sqlBulkCopy.ColumnMappings.Add("Income_Level", "Income_Level");
                    sqlBulkCopy.ColumnMappings.Add("LastModifierUserId", "LastModifierUserId");
                    sqlBulkCopy.ColumnMappings.Add("LastModificationTime", "LastModificationTime");
                    sqlBulkCopy.ColumnMappings.Add("Lead_Name", "Lead_Name");
                    sqlBulkCopy.ColumnMappings.Add("National_Id", "National_Id");
                    sqlBulkCopy.ColumnMappings.Add("Phone_Number", "Phone_Number");
                    sqlBulkCopy.ColumnMappings.Add("Post_Date", "Post_Date");
                    sqlBulkCopy.ColumnMappings.Add("Province", "Province");
                    sqlBulkCopy.ColumnMappings.Add("Province_Code", "Province_Code");
                    sqlBulkCopy.ColumnMappings.Add("Score_Range", "Score_Range");
                    sqlBulkCopy.ColumnMappings.Add("UPL_Interest", "UPL_Interest");
                    sqlBulkCopy.ColumnMappings.Add("UPL_Limit", "UPL_Limit");
                    sqlBulkCopy.ColumnMappings.Add("Valid_Date", "Valid_Date");
                    sqlBulkCopy.ColumnMappings.Add("Transfer_To", "Transfer_To");
                    sqlBulkCopy.ColumnMappings.Add("Transfer_Date", "Transfer_Date");
                    sqlBulkCopy.ColumnMappings.Add("District_Code", "District_Code");
                    sqlBulkCopy.ColumnMappings.Add("Process_DuplicateID", "Process_DuplicateID");
                    sqlBulkCopy.ColumnMappings.Add("Status_DuplicateID", "Status_DuplicateID");
                    sqlBulkCopy.ColumnMappings.Add("Description1", "Description1");
                    sqlBulkCopy.ColumnMappings.Add("Description2", "Description2");
                    sqlBulkCopy.ColumnMappings.Add("Description3", "Description3");
                    sqlBulkCopy.ColumnMappings.Add("Description4", "Description4");
                    sqlBulkCopy.ColumnMappings.Add("Lead_Code", "Lead_Code");
                   
                    con.Open();
                    sqlBulkCopy.WriteToServer(dt);
                    con.Close();
                }
            }
        }
        public List<Lead_Import> DataLead_Import()
        {
            Lead_GWEntities objEntity = new Lead_GWEntities();
           // var context = new Lead_GWEntities();
            /// lstEmp = objEntity.Lead_GW_Compare.ToList();
            var context = new Lead_GWEntities();
            var user = objEntity.Lead_Import;

            var storesList = context.Lead_Import.Select(x => x).Take(100).ToList();
 
           
           
           
            // TransferLead(storesList);
            return storesList;
        }
        public List<Lead_Import> GetDataLead_Import()
        {
            Lead_GWEntities objEntity = new Lead_GWEntities();
            // var context = new Lead_GWEntities();
            /// lstEmp = objEntity.Lead_GW_Compare.ToList();
            var context = new Lead_GWEntities();
            var user = objEntity.Lead_Import;

            var storesList = context.Lead_Import.Select(x => x).Where(t=>t.StatusTransfer ==null).ToList();
            //TransferLead(storesList);
            var storesListID = context.Lead_Import.Select(x => x.Id).ToList();
            //  TransferLead(storesList);


            string qr = "";

            foreach (var item in storesListID)
            {
                qr = qr + item.ToString();
            }
            var db = new Lead_GWEntities();
            StringBuilder sbQueryfn = new StringBuilder("Update [Lead_MGL].[dbo].[Lead_Import] set ");
            sbQueryfn.Append("");
            sbQueryfn.Append(sbQueryfn);
            db.Database.CommandTimeout = 100000;
            return storesList;
        }
        public DataTable TransferLead (List<Lead_Import> listLeadIP)
        {
            List<Lead> listLead = new List<Lead>();
           
            foreach (var item in listLeadIP)
            {
                Lead rowLead = new Lead();
                rowLead.Band_CC = item.CC_Band;
                rowLead.Band_UPL = item.UPL_Band;
                rowLead.Bound_Code = item.Bound_Code;
                rowLead.CC_Limit = item.CC_Limit;
                rowLead.DoiTacId = 1;
                rowLead.DuAnID = 5;
                rowLead.Created_Id = 1;
                rowLead.Created_Date = DateTime.Now;
                rowLead.Phone_Number = item.Phone_Number;
                rowLead.Post_Date = DateTime.Now;
                rowLead.Province = item.Province_Code;
                rowLead.Province_Code = item.Province_Code;
                rowLead.UPL_Interest =Convert.ToDouble( item.Lai_Suat);
                rowLead.UPL_Limit = item.UPL_Limit;
                //////////////////////////////////////////////
                if (item.San_Pham == "CC")
                {
                    if (item.Scoring_CC == null )
                    {

                    }
                    else
                    {
                        rowLead.Valid_Date =Convert.ToDateTime( item.Scoring_CC).AddDays(61);
                    }
                }
                if (item.San_Pham == "UPL")
                {
                    if (item.Scoring_UPL == null)
                    {

                    }
                    else
                    {            
                        rowLead.Valid_Date = Convert.ToDateTime(item.Scoring_UPL).AddDays(61);
                    }
                }
                ///////////////////////////////////////////////////////
                rowLead.Process_DuplicateID = 0;
                rowLead.Status_DuplicateID = 0;
                rowLead.Lead_Code = Convert.ToString( item.Id);
                listLead.Add(rowLead);


            }
           DataTable tbLead= objData.CreeateTableLead();
            tbLead= objCommom.ToDataTable(listLead);
            BulkInsertLead(tbLead);



            return null;
        }
       
       
    }
}
