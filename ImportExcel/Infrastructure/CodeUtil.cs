using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ImportExcel.Infrastructure
{
    public static class CodeUtil
    {
        /*Converts DataTable To List*/
        public static List<TSource> ToList<TSource>(this DataTable dataTable) where TSource : new()
        {
            var dataList = new List<TSource>();

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            var objFieldNames = (from PropertyInfo aProp in typeof(TSource).GetProperties(flags)
                                 select new { Name = aProp.Name, Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType }).ToList();
            var dataTblFieldNames = (from DataColumn aHeader in dataTable.Columns
                                     select new { Name = aHeader.ColumnName, Type = aHeader.DataType }).ToList();
            var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();

            foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
            {

                var aTSource = new TSource();
                foreach (var aField in objFieldNames)
                {

                    PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
                    bool? isVirtual = propertyInfos.IsVirtual();
                    if (propertyInfos.PropertyType == typeof(int))
                    {
                         if (aField.Name.Equals("GCMID"))
                        {
                        }
                        else
                            propertyInfos.SetValue(aTSource, Convert.ToInt32(dataRow[aField.Name]), null);
                    }
                
                    else if (isVirtual != true)
                    {
                        var targetType = IsNullableType(propertyInfos.PropertyType) ? Nullable.GetUnderlyingType(propertyInfos.PropertyType) : propertyInfos.PropertyType;
                        if (aField.Name.Equals("Sub_cat_pk"))
                        {
                            propertyInfos.SetValue(aTSource, dataRow["Sub-cat_pk"], null);
                        }
                        else if (aField.Name.Equals("Sub_Category_name"))
                        {
                            propertyInfos.SetValue(aTSource, dataRow["Sub-Category_name"], null);
                        }
                        else if (aField.Name.Equals("SUB_SUB_Cat_PK"))
                        {
                            propertyInfos.SetValue(aTSource, dataRow["SUB-SUB-Cat_PK"], null);
                        }
                        else if (aField.Name.Equals("SUB_sub_category_NAME"))
                        {
                            propertyInfos.SetValue(aTSource, dataRow["SUB-sub-category_NAME"], null);
                        }
                       
                        else if (string.IsNullOrEmpty(dataRow[aField.Name].ToString()) || aField.Name.Equals("MERCH_COMP"))
                        {
                            propertyInfos.SetValue(aTSource, null, null);
                        }
                        else
                            propertyInfos.SetValue(aTSource, dataRow[aField.Name], null);
                        
                    }
                }
                dataList.Add(aTSource);
            }
            return dataList;
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        /*Check if property is virtual */
        public static bool? IsVirtual(this PropertyInfo self)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            bool? found = null;

            foreach (MethodInfo method in self.GetAccessors())
            {
                if (found.HasValue)
                {
                    if (found.Value != method.IsVirtual)
                        return false;
                }
                else
                {
                    found = method.IsVirtual;
                }
            }

            return found;
        }

        /* check if two object have each value equal except virtual property value*/
        public static bool IfEqual<TSource>(TSource other, TSource another) where TSource : new()
        {
            var propertyList = typeof(TSource).GetProperties();
            //foreach (var prop in propertyList)
            //{
            //    if (other["sdfds"].Equals(another["sdfds"])
            //    {
            //    }
            //}

            Type myType = other.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
            bool isEqual = true;
            foreach (PropertyInfo prop in props)
            {
                bool? isVirtual = prop.IsVirtual();
            
               if (isVirtual != true)
                {
                    if (prop.Name.Equals("GCMID"))
                    {
                    }
                    else
                    {
                        var firstValue = prop.GetValue(another, null);
                        var secondValue = prop.GetValue(other, null);
                        if (firstValue == null && secondValue == null)
                        {

                        }
                        else
                        if (!firstValue.Equals(secondValue))
                        {
                            isEqual = false;
                            return false;

                        }
                    }
                }

                // Do something with propValue
            }

            //return this.CourseID == other.CourseID &&
            //       this.Credits == other.Credits &&
            //       this.Title == other.Title;

            return true;
        }
    }
}