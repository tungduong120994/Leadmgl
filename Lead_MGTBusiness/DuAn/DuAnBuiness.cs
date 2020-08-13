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

namespace Lead_MGTBusiness.DuAn
{
    public class DuAnBuiness
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
                if (dt.Rows[i]["CC_Band"].ToString() != null || dt.Rows[i]["CC_Band"].ToString() != "")
                {
                    row.CC_Band = Convert.ToDecimal(dt.Rows[i]["CC_Band"].ToString());
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
            var storesListID = context.Lead_Import.Where(T => T.StatusTransfer == null).Select(x => x.Id).ToList();

            var db = new Lead_GWEntities();
            foreach (var item in storesListID)
            {
                StringBuilder sbQueryfn = new StringBuilder("Update [Lead_GW].[dbo].[Lead_Import] set StatusTransfer =1 ,TransferDate= GETDATE()  where (StatusTransfer =0 or StatusTransfer is NULL) and Id =" + item + "  ");

                db.Database.ExecuteSqlCommand(sbQueryfn.ToString());
            }

            var storedCheckDeDup = context.CD_DieuKienCheckDuAn.Where(T => T.IsDeleted == false).Select(x => x).ToList();
            DeDup(storedCheckDeDup);
            
            //TransferLead(storesList);



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

            var storesList = context.Lead_Import.Select(x => x).Where(t => t.StatusTransfer == null).ToList();
            //TransferLead(storesList);
            var storesListID = context.Lead_Import.Select(x => x.Id).ToList();
            //  TransferLead(storesList);


            string qr = "";

            foreach (var item in storesListID)
            {
                qr = qr + item.ToString();
            }
            var db = new Lead_GWEntities();
            StringBuilder sbQueryfn = new StringBuilder("Update [Lead_GW].[dbo].[Lead_Import] set ");
            sbQueryfn.Append("");
            sbQueryfn.Append(sbQueryfn);
            db.Database.CommandTimeout = 100000;
            return storesList;
        }
        public void DeDup(List<CD_DieuKienCheckDuAn> lstcheck)
        {
            string a = "";
            foreach (var item in lstcheck)
            {
                
                a = a + "" + item.TenDieuKien + ",";
                string strgroupids = a.Remove(a.Length - 1);
                var db = new Lead_GWEntities();
                StringBuilder sbQueryfn = new StringBuilder("");
                sbQueryfn.Append(sbQueryfn);
                sbQueryfn.Append(";with mycte");

                sbQueryfn.Append(" as(");

                sbQueryfn.Append(" select id, ROW_NUMBER() over(partition by "+strgroupids+" order by id) as Duplicate from Lead");

                sbQueryfn.Append(" )");

                sbQueryfn.Append(" UPDATE A");

                sbQueryfn.Append(" SET a.Status_DuplicateID = B.Duplicate ,a.Process_DuplicateID="+item.GiaTri+" ");

                sbQueryfn.Append(" FROM Lead A");

                sbQueryfn.Append(" JOIN mycte B  ON A.id = B.id");

                sbQueryfn.Append(" where Status_DuplicateID <>1 and A.Process_DuplicateID =" + (Convert.ToInt16( item.GiaTri)-1)+" ");
                string a111 = sbQueryfn.ToString();
                db.Database.ExecuteSqlCommand(sbQueryfn.ToString());

            }
        }
        public DataTable TransferLead(List<Lead_Import> listLeadIP)
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
                rowLead.UPL_Interest = Convert.ToDouble(item.Lai_Suat);
                rowLead.UPL_Limit = item.UPL_Limit;
                //////////////////////////////////////////////
                if (item.San_Pham == "CC")
                {
                    if (item.Scoring_CC == null)
                    {

                    }
                    else
                    {
                        rowLead.Valid_Date = Convert.ToDateTime(item.Scoring_CC).AddDays(61);
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
                rowLead.Lead_Code = Convert.ToString(item.Id);
                listLead.Add(rowLead);


            }
            DataTable tbLead = objData.CreeateTableLead();
            tbLead = objCommom.ToDataTable(listLead);
            BulkInsertLead(tbLead);



            return null;
        }


    }
}
