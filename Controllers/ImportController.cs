using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace DBAdmin.Controllers
{
    public class ImportController : Controller
    {
        private DBAdminEntities db = new DBAdminEntities();

        // GET: Import
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                try
                {
                    string fileExtension = Path.GetExtension(postedFile.FileName);

                    //Validate uploaded file and return error.
                    if (fileExtension != ".csv")
                    {
                        ViewBag.Message = "Please select the csv file with .csv extension";
                        return View();
                    }


                    var tableColumns = new List<Row>();
                    var ColumnTypes = new SelectList(Shared.SharedLib.GetSqlDBType());
                    ViewBag.ColumnTypes = new SelectList(Shared.SharedLib.GetSqlDBType());

                    using (var sreader = new StreamReader(postedFile.InputStream))
                    {
                        //First line is header. If header is not passed in csv then we can neglect the below line.
                        string[] headers = sreader.ReadLine().Split(',');
                        //Loop through the records
                        while (!sreader.EndOfStream)
                        {
                            string[] rows = sreader.ReadLine().Split(',');

                            tableColumns.Add(new Row
                            {
                                ColumnName = rows[0].ToString(),
                                ColumnType = "",                               
                            });
                        }
                    }


                    DataTable dt = new DataTable
                    {
                        Rows = tableColumns,TableName = Path.GetFileNameWithoutExtension(postedFile.FileName)

                    };

                    ViewBag.ServerName = new SelectList(db.Servers, "ServerName", "ServerName");
                    ViewBag.DBName = new SelectList(db.DataBases, "DBName", "DBName");

                    return View("View", dt);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }
            }
            else
            {
                ViewBag.Message = "Please select the file first to upload.";
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit(IEnumerable<DBAdmin.Controllers.Row> row)
        public ActionResult Edit([Bind(Include = "Rows,DBName,ServerName,TableName")] DataTable dataTable)

        {
            if (ModelState.IsValid)
            {
                Server server = db.Servers.FirstOrDefault(m => m.ServerName.Trim() == dataTable.ServerName.Trim());

                var dataBases = Shared.SharedLib.CreateSqlTable(dataTable, server);
                //db.Entry(server).State = EntityState.Modified;
                //db.SaveChanges();
                if (dataBases)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("","some error");
                    return View();

                    return View("View", dataTable);
                }


                //ViewBag.DBType = new SelectList(db.DataBaseTypes, "DataBaseTypes", "DataBaseTypes", server.DBType);
                return View();
            }
            return View();
        }

        public ActionResult FillDatabase(string servername)
        {
            Server server = db.Servers.FirstOrDefault(m => m.ServerName.Trim() == servername.Trim());
            var dataBases = Shared.SharedLib.GetDataBases(server);
            return Json(dataBases, JsonRequestBehavior.AllowGet);
        }

    }

    public class DataTable
    {
        public List<Row> Rows { get; set; }

        [Required]
        public string DBName { get; set; }
        [Required]
        public string ServerName { get; set; }

        public string DBType { get; set; }

        public string TableName { get; set; }

    }

 
    public class Row
    {
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }

        //public List<string> ColumnTypes { get; set; } //Your list of designations  

        public IEnumerable<SelectListItem> ColumnTypes
        {
            get { return new SelectList(Shared.SharedLib.GetSqlDBType()); }
        }
    }

   




}
