

using ImportExcel.Context;
using System;
using System.Web;

namespace ImportExcel.Infrastructure
{
   
    public class TestCoreFrameDBEntities
    {
       
     
        public static TestCoreFrameEntities GetDbContext()
        {

            TestCoreFrameEntities DbContext = HttpContext.Current.Items["DbContext"] as TestCoreFrameEntities;

            if (DbContext == null)
            {
                DbContext = new TestCoreFrameEntities();
                HttpContext.Current.Items["DbContext"] = DbContext;
            }

            return DbContext;
        }

        public static void DisposeDbContext()
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            if (HttpContext.Current.Items == null)
            {
                return;
            }

            TestCoreFrameEntities DbContext = HttpContext.Current.Items["DbContext"] as TestCoreFrameEntities;

            if (DbContext != null)
            {
                DbContext.Dispose();
            }
        }

        public static void ExecuteSQLCommand(String cmd)
        {
            GetDbContext().Database.SqlQuery<String>(cmd);
        }
    }
}
