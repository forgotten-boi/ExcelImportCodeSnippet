using ImportExcel.Context;
using ImportExcel.Infrastructure;
using ImportExcel.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using NLog;

namespace ImportExcel.Controllers
{
    public class ImportExcelController : Controller
    {
        ModuleManager<tblEducation> excelHistoryManager = new ModuleManager<tblEducation>();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        // GET: ImportExcel
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImportExcel()
        {
            return View();

        }

        [HttpPost]
        public ActionResult ImportExcel(string TableName)
        {
            if (Request.Files["FileUpload1"].ContentLength > 0)
            {
                string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                string connString = "";


                DataTable dt = new DataTable();
                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

                string path1 = string.Format("{0}/{1}", Server.MapPath("~/ExcelData/Uploads"), Request.Files["FileUpload1"].FileName);
                if (!Directory.Exists(path1)) // if upload folder path does not exist, create one.
                {
                    Directory.CreateDirectory(Server.MapPath("~/ExcelData/Uploads"));
                }
                if (validFileTypes.Contains(extension))
                {
                    if (System.IO.File.Exists(path1))
                    {
                        System.IO.File.Delete(path1); // if file exist previously, delete previous one
                    }
                    Request.Files["FileUpload1"].SaveAs(path1);
                    //add different connection string for different types of excel
                    if (extension == ".csv")
                    {
                        dt = XmlUtil.ConvertCSVtoDataTable(path1);

                    }
                    //Connection String to Excel Workbook  
                    else if (extension.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        dt = XmlUtil.ConvertXSLXtoDataTable(path1, connString);

                    }
                    else if (extension.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        dt = XmlUtil.ConvertXSLXtoDataTable(path1, connString);

                    }
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TestCoreConnectString"].ConnectionString))
                    {
                        //int count = GetTableStructure(con, TableName).Count;
                        var tablestructure = GetTableStructure(con, TableName);
                        var count = tablestructure.Count;

                        string[] columnNames = dt.Columns.Cast<DataColumn>()
                                   .Select(x => x.ColumnName)
                                   .ToArray();
                        //foreach (var col in columnNames)
                        //{
                        //    if (!tablestructure.Keys.Contains(col.ToString()))
                        //    {
                        //        dt.Columns.Remove(col.ToString());
                        //    }
                        //}


                        try
                        {

                            //string clientTableName = GetXmlTableNameList().Where(p => string.Equals(p.TableName, TableName)).FirstOrDefault().ClientTableName;
                            string clientTableName = TableName;


                            using (SqlTransaction transaction = con.BeginTransaction())
                            {

                                SqlBulkCopy sbc = new SqlBulkCopy(con, SqlBulkCopyOptions.KeepIdentity, transaction);
                                sbc.DestinationTableName = TableName;

                                ////dt.Columns.Add("CreatedDate");
                                //dt.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                                //dt.Columns.Add(new DataColumn("CreatedBy", typeof(string)));

                                //foreach (DataRow row in dt.Rows)
                                //{
                                //    row["CreatedDate"] = DateTime.Now;
                                //    row["CreatedBy"] = Session["Username"].ToString();
                                //}
                                if (!TableName.Equals("Course"))
                                    dt = ModifyDataTable(dt);
                                if (count == (dt.Columns.Count + 1))
                                {

                                    //dt.Columns.Add(new DataColumn("GCMID", typeof(int)));
                                
                                    foreach (DataColumn col in dt.Columns)
                                    {
                                        sbc.ColumnMappings.Add(col.ColumnName.ToString(), col.ColumnName.ToString());
                                        //count++;
                                    }

                                    ModuleManager<GBS_CATEGORY_MATRIX> courseManager = new ModuleManager<GBS_CATEGORY_MATRIX>();
                                    sbc.BatchSize = 1000;
                                    sbc.NotifyAfter = 1000;
                                    sbc.WriteToServer(dt);
                                    transaction.Commit();
                                    var dataInDatabase = courseManager.GetAllRestricted().ToList();

                                    var courseList = CodeUtil.ToList<GBS_CATEGORY_MATRIX>(dt);
                                    //List<Course> courseList = new List<Course>();
                                    //courseList = courseManager.ConvertDataTable<Course>(dt);

                                    //var courseList =   (from DataRow dr in dt.Rows
                                    // select new Course()
                                    // {
                                    //     CourseID = Convert.ToInt32(dr["CourseID"]),
                                    //     Credits= Convert.ToInt32(dr["Credits"]),
                                    //     Title = dr["Title"].ToString()
                                    // }).ToList();



                                    //foreach (var course in courseList)
                                    //{
                                    //    if (!dataInDatabase.Any(p=>p.Equals(course)))
                                    //    {
                                    //        ModelState.AddModelError("", "Following row insert failed, Credits:" + course.Credits + "Title:" + course.Title);
                                    //        logger.Error("Following row insert failed, Credits:" + course.Credits + "Title: " + course.Title);
                                    //    }
                                    //}

                                    foreach (var course in courseList)
                                    {
                                        if (!dataInDatabase.Any(p => CodeUtil.IfEqual<GBS_CATEGORY_MATRIX>(p, course)))
                                        {
                                            ModelState.AddModelError("", "Following row insert failed, SUB_sub_category_NAME:" + course.SUB_sub_category_NAME.ToString() + "Category_name:" + course.Category_name.ToString());
                                            logger.Error("Following row insert failed, SUB_sub_category_NAME:" + course.SUB_sub_category_NAME.ToString() + "Category_name:" + course.Category_name.ToString());
                                        }
                                    }


                                    //foreach (DataRow data in dt.Rows)
                                    //{
                                    //    int Credits = Convert.ToInt32(data["Credits"]);
                                    //    string title = data["Title"].ToString();
                                    //    if (!dataInDatabase.Any(p => (p.Credits == Convert.ToInt32(data["Credits"])) && (p.Title.Equals(data["Title"].ToString()))))
                                    //    {
                                    //        ModelState.AddModelError("", "Following row insert failed, Credits:" + data["Credits"].ToString() + "Title:" + data["Title"].ToString());
                                    //        logger.Error("Following row insert failed, Credits:" + data["Credits"].ToString() + "Title:" + data["Title"].ToString());
                                    //    }

                                    //}




                                    tblEducation tblHistory = new tblEducation()
                                    {
                                        //CreatedBy = Session["Username"].ToString(),
                                        CreatedDate = DateTime.Now,
                                        TableName = TableName,
                                        ClientTableName = clientTableName

                                    };
                                    excelHistoryManager.Insert(tblHistory);
                                    //Data has been stored to respective table.
                                    TempData["Notification"] = "Data of '" + clientTableName + "' has been stored to respective table.";
                                    //log.Info("Data of '" + clientTableName + "' has been stored to respective table. Table Name is " + TableName);
                                    return RedirectToAction("ImportExcel");
                                }
                                else
                                {

                                    ModelState.AddModelError("", "Invalid Excel Format");
                                    //log.Error("Invalid XML Schema");
                                    List<XmlTable> xmlTableNames = GetXmlTableNameList();
                                    ViewBag.XmlTable = new SelectList(xmlTableNames, "TableName", "ClientTableName");
                                    return View();

                                }
                            }


                        }
                        catch (Exception ex)
                        {
                            //if error occurred
                            ModelState.AddModelError("", ex.Message);
                            //log.Error(ex.Message);

                        }
                        finally
                        {
                            //close connection
                            con.Close();
                        }
                        //}
                        //ModelState.AddModelError("", "Invalid excel File");
                        ////log.Error("Invalid excel File");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please Upload Files in .xls, .xlsx or .csv format");
                    ////log.Error("File uploaded is not in .xls, .xlsx or.csv format but " + extension + "format");

                }

            }
            List<XmlTable> xmlTableNameList = GetXmlTableNameList();
            ViewBag.XmlTable = new SelectList(xmlTableNameList, "TableName", "ClientTableName");
            return View();
        }


        #region Unwanted Code

        //paste it in course.cs for overriding equals
        //public bool Equals(Course other)
        //{
        //   
        //    return this.CourseID == other.CourseID &&
        //           this.Credits == other.Credits &&
        //           this.Title == other.Title;
        //}
        //paste in GBS_CATEGORY_MATRIX after code regeneration for overriding equals
        //public bool Equals(GBS_CATEGORY_MATRIX other)
        //{
        //    // Would still want to check for null etc. first.
        //    return this.Cat_pk == other.Cat_pk &&
        //           this.Category_name == other.Category_name &&
        //             this.Sub_cat_pk == other.Sub_cat_pk &&
        //               this.Sub_Category_name == other.Sub_Category_name &&
        //                 this.SUB_SUB_Cat_PK == other.SUB_SUB_Cat_PK &&
        //                   this.SUB_sub_category_NAME == other.SUB_sub_category_NAME &&
        //                     this.MERCH_Main == other.MERCH_Main &&
        //                       this.MERCH_Backup == other.MERCH_Backup &&
        //                         this.fk_company_bo == other.fk_company_bo &&
        //                     this.FK_MERCH == other.FK_MERCH &&
        //                     this.FK_sub_MERCH == other.FK_sub_MERCH &&
        //                     this.fk_Buying_team == other.fk_Buying_team &&
        //                      this.fk_Purchasing_category == other.fk_Purchasing_category &&
        //                     this.fk_Purchasing_Subcategory == other.fk_Purchasing_Subcategory &&
        //                     this.IS_DEFAULT == other.IS_DEFAULT &&
        //                      this.MERCH_COMP == other.MERCH_COMP &&
        //                     this.OLD_FK_purchasing_subcat == other.OLD_FK_purchasing_subcat &&
        //                     this.OLD_FK_purchasing_cat == other.OLD_FK_purchasing_cat;
        //}

        #endregion



        #region User Defined Function

        private DataTable ModifyDataTable(DataTable dt)
        {
            DataTable dTable = new DataTable();
            dTable.Columns.Add(new DataColumn("Cat_pk", typeof(string)));
            dTable.Columns.Add(new DataColumn("Category_name", typeof(string)));

            dTable.Columns.Add(new DataColumn("Sub-cat_pk", typeof(string)));
            dTable.Columns.Add(new DataColumn("Sub-Category_name", typeof(string)));

            dTable.Columns.Add(new DataColumn("SUB-SUB-Cat_PK", typeof(string)));
            dTable.Columns.Add(new DataColumn("SUB-sub-category_NAME", typeof(string)));

            dTable.Columns.Add(new DataColumn("MERCH_Main", typeof(string)));
            dTable.Columns.Add(new DataColumn("MERCH_Backup", typeof(string)));

            dTable.Columns.Add(new DataColumn("fk_company_bo", typeof(short)));
            dTable.Columns.Add(new DataColumn("FK_MERCH", typeof(short)));

            dTable.Columns.Add(new DataColumn("FK_sub_MERCH", typeof(short)));
            dTable.Columns.Add(new DataColumn("fk_Buying_team", typeof(int)));

            dTable.Columns.Add(new DataColumn("fk_Purchasing_category", typeof(string)));
            dTable.Columns.Add(new DataColumn("fk_Purchasing_Subcategory", typeof(string)));

            dTable.Columns.Add(new DataColumn("IS_DEFAULT", typeof(bool)));
            dTable.Columns.Add(new DataColumn("MERCH_COMP", typeof(short)));

            dTable.Columns.Add(new DataColumn("OLD_FK_purchasing_subcat", typeof(int)));
            dTable.Columns.Add(new DataColumn("OLD_FK_purchasing_cat", typeof(int)));

            var count = dt.Columns.Count;
            var countrlyListCount = count - 8;
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    string Cat_pk = row["TIER1"].ToString().Split('{', '}')[1];
                    string categoryName = row["TIER1"].ToString();

                    string Sub_cat_pk = row["TIER2"].ToString().Split('{', '}')[1];
                    string Sub_Category_name = row["TIER2"].ToString();

                    string SUB_SUB_Cat_PK = row["TIER3"].ToString().Split('{', '}')[1];
                    string SUB_sub_category_NAME = row["TIER3"].ToString();

                    string MERCH_Main = row["MERCH_Main"].ToString();
                    string MERCH_Backup = row["MERCH_Backup"].ToString();

                    string DES_TEAM = row["DES_TEAM"] == null ? "" : row["DES_TEAM"].ToString();
                    //get merchID

                    ModuleManager<USER> userManager = new ModuleManager<USER>();
                    int merchID = userManager.GetAllRestricted().Where(p => p.NAME.Equals(MERCH_Main, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault().PK_USER;

                    int subMerchID = userManager.GetAllRestricted().Where(p => p.NAME.Equals(MERCH_Backup, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault().PK_USER;

                    int fk_buying_team = 0;
                    if (!DES_TEAM.Equals(""))
                    {
                        ModuleManager<TEAM> teamManager = new ModuleManager<TEAM>();
                        fk_buying_team = teamManager.GetAllRestricted().Where(p => p.DES_TEAM.Equals(DES_TEAM, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault().PK_TEAM;
                    }

                    int catLength = Sub_cat_pk.Length;
                    int subCatlen = SUB_SUB_Cat_PK.Length;

                    var purchasingCatID = SUB_SUB_Cat_PK.ToString().Substring(0, subCatlen - catLength).ToString();


                    int purchasingCategoryID = Convert.ToInt32(purchasingCatID);

                    var subPurchasingCatID = SUB_SUB_Cat_PK.ToString().Substring(subCatlen - catLength + 1);
                    int subPurchasingCategoryID = Convert.ToInt32(subPurchasingCatID);
                    int MerchComp = 0;

                    string company = row["COMPANY"].ToString();
                    ModuleManager<COMPANy> companyManager = new ModuleManager<COMPANy>();
                    ModuleManager<COUNTRy> countryManager = new ModuleManager<COUNTRy>();
                    int compID = companyManager.GetAllRestricted().Where(p => p.DES_COMPANY.Equals(company)).FirstOrDefault().PK_COMPANY;
                    bool isSingleMerch = true;
                    bool isDefault = true;


                    if (row.ItemArray.ToList().Any(p => p.Equals("FALSE")))
                    {
                        for (int i = 8; i < count; i++)
                        {

                            if (row[i].ToString().Equals("TRUE"))
                            {
                                string currentColName = dt.Columns[i].ColumnName;
                                int merchCompID = countryManager.GetAllRestricted().Where(p => p.DES_COUNTRY.Equals(currentColName)).FirstOrDefault().PK_COUNTRY;
                                //add to datatable
                                dTable.Rows.Add(Cat_pk, categoryName, Sub_cat_pk, Sub_Category_name, SUB_SUB_Cat_PK, SUB_sub_category_NAME, MERCH_Main, MERCH_Backup, compID, merchID, subMerchID, fk_buying_team, purchasingCategoryID, subPurchasingCategoryID, true, MerchComp);
                            }



                        }
                    }
                    else
                    {
                        dTable.Rows.Add(Cat_pk, categoryName, Sub_cat_pk, Sub_Category_name, SUB_SUB_Cat_PK, SUB_sub_category_NAME, MERCH_Main, MERCH_Backup, compID, merchID, subMerchID, fk_buying_team, purchasingCategoryID, subPurchasingCategoryID, true, null);
                    }
                    //Add to datatable
                    if (!MERCH_Main.Equals(MERCH_Backup))
                    {




                        //ForBackupMerch
                        isDefault = false;


                        if (row.ItemArray.ToList().Any(p => p.Equals("FALSE")))
                        {
                            for (int i = 8; i < count; i++)
                            {

                                if (row[i].ToString().Equals("TRUE"))
                                {
                                    string currentColName = dt.Columns[i].ColumnName;
                                    int merchCompID = countryManager.GetAllRestricted().Where(p => p.DES_COUNTRY.Equals(currentColName)).FirstOrDefault().PK_COUNTRY;
                                    //add to datatable
                                    dTable.Rows.Add(Cat_pk, categoryName, Sub_cat_pk, Sub_Category_name, SUB_SUB_Cat_PK, SUB_sub_category_NAME, MERCH_Main, MERCH_Backup, compID, merchID, subMerchID, fk_buying_team, purchasingCategoryID, subPurchasingCategoryID, true, MerchComp);
                                }



                            }
                        }
                        else
                        {
                            dTable.Rows.Add(Cat_pk, categoryName, Sub_cat_pk, Sub_Category_name, SUB_SUB_Cat_PK, SUB_sub_category_NAME, MERCH_Main, MERCH_Backup, compID, merchID, subMerchID, fk_buying_team, purchasingCategoryID, subPurchasingCategoryID, true, null);
                        }
                    }

                  
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return dTable;

        }
        private List<XmlTable> GetXmlTableNameList()
        {
            var xmlTableNameList = new List<XmlTable>();
            // if there are multiple table for excel import with different name on client and database
            //name then add a xml file with tablename and clienttable name to be selected from drop downlist
            try
            {
                xmlTableNameList = XDocument.Load(Server.MapPath("~/Resources/XmlTableName.xml")).Element("XmlTable")
                             .Descendants("Table")
                             .Select(x => new XmlTable
                             {
                                 TableName = x.Element("TableName").Value,
                                 ClientTableName = x.Element("ClientTableName").Value
                             }).ToList();
                if (xmlTableNameList == null)
                {
                    xmlTableNameList.Add(new XmlTable
                    {
                        TableName = "Course",
                        ClientTableName = "Course"
                    });

                }

            }
            catch (Exception ex)
            {
                //log.Error(ex.Message);
                ModelState.AddModelError("", ex.Message);
            }
            return xmlTableNameList;
        }

        public Dictionary<string, string> GetTableStructure(SqlConnection con, string tableName)
        {
            Dictionary<string, string> Param = new Dictionary<string, string>();

            string sqlCheckTable = "SELECT c.name as 'ColumnName', CONCAT(t.Name,'(',c.max_length,')') as 'DataType' FROM sys.columns c INNER JOIN sys.types t ON c.user_type_id = t.user_type_id WHERE c.object_id = OBJECT_ID('" + tableName + "')";


            if (con.State != ConnectionState.Open)
                con.Open();
            try
            {
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = sqlCheckTable;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Param.Add(reader["ColumnName"].ToString(), reader["DataType"].ToString());
                            logger.Info("Column Names: " + reader["ColumnName"].ToString() + " Datatype:" + reader["DataType"].ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                //log.Error(ex.Message);
            }
            return Param;
        }

        #endregion  
    }
}