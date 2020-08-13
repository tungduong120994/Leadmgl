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
using Lead_MGTValueObject.DieuKienDuAnModel;

namespace Lead_MGTBusiness.DieuKienDuAn
{
    public class DieuKienDuAnBuiness
    {
        List<ImportLeadModel> lis = new List<ImportLeadModel>();
        DatatableLead objData = new DatatableLead();
        DataTableToList objCommom = new DataTableToList();

        public List<CD_DieuKienCheckDuAn> DataLead_Import()
        {
            Lead_GWEntities objEntity = new Lead_GWEntities();
            // var context = new Lead_GWEntities();
            /// lstEmp = objEntity.Lead_GW_Compare.ToList();
            var context = new Lead_GWEntities();
            var user = objEntity.Lead_Import;

            var storesList = context.CD_DieuKienCheckDuAn.Select(x => x).Take(100).ToList();
            return storesList;
        }
        public List<BoundCodeDeDupModel> getDataBouCodeDeDup()
        {
            List<BoundCodeDeDupModel> lisBoundCodeDeDup = new List<BoundCodeDeDupModel>();
            Lead_GWEntities objEntity = new Lead_GWEntities();
            // var context = new Lead_GWEntities();
            /// lstEmp = objEntity.Lead_GW_Compare.ToList();
            var context = new Lead_GWEntities();
            var user = objEntity.Lead_Import;

            var storesList = context.CD_DieuKienCheckBoundCode.Select(x => x).Where(T => T.IsDeleted == false).ToList();
            for (int i = 0; i < storesList.Count; i++)
            {
                BoundCodeDeDupModel row = new BoundCodeDeDupModel();
                row.BoundCodeId = Convert.ToInt16(storesList[i].BoundCodeId);
                row.BoundCodeTrung = Convert.ToInt16(storesList[i].BoundCodeCheckId);
                row.TongID = "" + storesList[i].BoundCodeId + "-" + storesList[i].BoundCodeCheckId + "";
                lisBoundCodeDeDup.Add(row);
            }

            return lisBoundCodeDeDup;
        }
        public List<DieuKienCheckDeDupModel> getDataDieuKienCheck()
        {
            List<DieuKienCheckDeDupModel> lisDeDup = new List<DieuKienCheckDeDupModel>();
            Lead_GWEntities objEntity = new Lead_GWEntities();
            // var context = new Lead_GWEntities();
            /// lstEmp = objEntity.Lead_GW_Compare.ToList();
            var context = new Lead_GWEntities();
            var user = objEntity.Lead_Import;

            var storesList = context.CD_DieuKienCheckDuAn.Select(x => x).Where(T => T.IsDeleted == false).ToList();
            for (int i = 0; i < storesList.Count; i++)
            {
                DieuKienCheckDeDupModel row = new DieuKienCheckDeDupModel();
                row.TenDieuKien = storesList[i].TenDieuKien;
                row.GiaTri = storesList[i].GiaTri;
                
                lisDeDup.Add(row);
            }

            return lisDeDup;
        }
        public void SaveCauHinhBoundCode(List<CachGhepModel> cachGhep)
        {
            var db = new Lead_GWEntities();

            StringBuilder sbQueryImportLead = new StringBuilder("");

            sbQueryImportLead.Append("UPDATE CD_DieuKienCheckBoundCode set IsActive = 0 ,IsDeleted =1 , DeletionTime ='" + DateTime.Now + "' ,LastModificationTime='" + DateTime.Now + "'");

            db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
            sbQueryImportLead = new StringBuilder("");
            if (cachGhep!=null)
            {
                for (int i = 0; i < cachGhep.Count; i++)
                {
                    string[] arrListStr = cachGhep[i].CachGhep.Split('-');

                    sbQueryImportLead = new StringBuilder("");
                    sbQueryImportLead.Append("INSERT INTO CD_DieuKienCheckBoundCode( BoundCodeId ,BoundCodeCheckId, IsActive, IsDeleted, CreationTime ) VALUES (");
                    sbQueryImportLead.Append(arrListStr[0] + ",");
                    sbQueryImportLead.Append(arrListStr[1] + ",");
                    sbQueryImportLead.Append("1,");
                    sbQueryImportLead.Append("0,");
                    sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");
                    db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
                }

            }
            
        }

        public void SaveCauHinhDuAn(SaveCauHinhDuAnModel cauHinh)
        {
            var db = new Lead_GWEntities();
            int a = 0;
            StringBuilder sbQueryImportLead = new StringBuilder("");

            sbQueryImportLead.Append("UPDATE CD_DieuKienCheckDuAn set IsDeleted =1 , DeletionTime ='" + DateTime.Now + "' ,LastModificationTime='" + DateTime.Now + "'");

            db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
            if (cauHinh.Phone_Number != null)
            {
                a += 1;
                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_DieuKienCheckDuAn( TenDieuKien ,GiaTri, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("N'" + cauHinh.Phone_Number + "',");
                sbQueryImportLead.Append("'" + a + "',");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");

                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
            }
           
            if (cauHinh.DuAnID != null)
            {
                a += 1;
                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_DieuKienCheckDuAn( TenDieuKien ,GiaTri, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("N'" + cauHinh.DuAnID + "',");
                sbQueryImportLead.Append("'" + a + "',");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");

                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
            }
            if (cauHinh.Bound_Code != null)
            {
                a += 1;
                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_DieuKienCheckDuAn( TenDieuKien ,GiaTri, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("N'" + cauHinh.Bound_Code + "',");
                sbQueryImportLead.Append("'" + a + "',");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");

                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
            }
            if (cauHinh.Valid_Date != null)
            {
                a += 1;
                sbQueryImportLead = new StringBuilder("");
                sbQueryImportLead.Append("INSERT INTO CD_DieuKienCheckDuAn( TenDieuKien ,GiaTri, IsDeleted, CreationTime ) VALUES (");
                sbQueryImportLead.Append("N'" + cauHinh.Valid_Date + "',");
                sbQueryImportLead.Append("'" + a + "',");
                sbQueryImportLead.Append("0,");
                sbQueryImportLead.Append("N'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "');");

                db.Database.ExecuteSqlCommand(sbQueryImportLead.ToString());
            }
        }

    }
}
