using System;
using System.Linq;


using System.Diagnostics;

using System.Linq.Expressions;

using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;



using System.Reflection;
using System.Runtime.CompilerServices;
using System.Data.Entity;
using ImportExcel.Context;
using System.Collections.Generic;
using System.Data;

namespace ImportExcel.Infrastructure
{
    public class ModuleManager<T> where T : class
    {
        
        protected TestCoreFrameEntities DBContext = null;
        protected DbSet<T> module = null;
        protected string moduleName;
        protected string auditFieldName = typeof(T).FullName;
     
        //public static log4net.ILog log = log4net.LogManager.GetLogger(typeof(T));
        public static Exception ex = new Exception();
        //public static StackTrace stackTrace = new StackTrace(ex, true);
        //protected static string grandParentFunction = stackTrace.GetFrames().LastOrDefault().GetMethod().Name;
        //protected string grandParentFunction = new StackFrame(1, true).GetMethod().Name;

        public ModuleManager()
        {
            this.DBContext = TestCoreFrameDBEntities.GetDbContext();
            module = DBContext.Set<T>();
        }

        public virtual IQueryable<T> GetAll()
        {
            var objList =  module.AsQueryable();
            
            SaveLogInfo();
            return objList;
        }


        public virtual IQueryable<T> GetAllRestricted()
        {
            return module.AsQueryable();
        }


        public virtual T GetById(int id)
        {
            var obj = module.Find(id);
      
            return obj;
        }
        public virtual T GetByGuId(string Id)
        {
            var obj = module.Find(Id);
      
            return obj;
        }
        public virtual void Insert(T obj)
        {
           
            module.Add(obj);
            Save();
      
        }

        public virtual void Update(T obj, int id)
        {
            T objToUpdate = module.Find(id);
            if (objToUpdate != null)
            {
                //module.Update(objToUpdate);
                Save();
             
            }
        }

        public virtual void Delete(int id)
        {
            T objToDelete = module.Find(id);
            if (objToDelete != null)
            {


                module.Remove(objToDelete);
                Save();
               
            }

        }

        public virtual void Save()
        {
            DBContext.SaveChanges();
        }
        public List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        
       

        public static void LogEvent()
        {

        }

        //public static string GetCaller([CallerMemberName] string caller = null)
        //{
        //    return caller;
        //}

        public void SaveLogInfo()
        {
            //log.Info(DateTime.Now.ToString() + " Module Function "+ moduleName + "Class Name "+ typeof(T).Name);
    
            //StackTrace stackTrace = new StackTrace(null,true);
            //var parentFunction = stackTrace.GetFrames().LastOrDefault().GetMethod().Name;

            //var parentFunction = new StackFrame(1, true).GetMethod().Name;
            //var className = auditFieldName;
            //if (obj != null)
            //{
            //    string userName = "", createdDate = "";
            //    createdDate = DateTime.Now.ToString("DD-MMM-YYYY");
            //var createdBy = obj.GetType().GetProperty("CreatedBy");
            //if (createdBy != null)
            //{
            //    userName = createdBy.GetValue(obj, null).ToString();
            //}

            //var date = obj.GetType().GetProperty("CreatedDate");
            //if (date != null)
            //{
            //    createdDate = date.GetValue(obj, null).ToString();
            //}

            //2017-04-12 14:29:53,662 INFO  - OnActionExecuted: Username: Anonymous, Class Name:Account, Action: LogIn
            //log.Info(grandParentFunction + ": " + className + "- " + parentFunction + "- " +
            //         userName + "- " + createdDate);
            //return userName;
            //}
            //else
            //{
            //    log.Info(grandParentFunction + ":-" + parentFunction + "-" + "Action is invalid or object is empty");
            //}
        }


    }
}
