using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;

namespace Helpers
{
    public static class Extentions
    {
        /// <summary>
        /// Listler için null ya da count 0 ise true döndürür.En az 1 eleman varsa false döner
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this List<T> list)
        {
            if (list == null)
            {
                return true;
            }

            return !list.Any();
        }

        public static bool IsEmpty(this DataTable dataTable)
        {
            return dataTable == null || dataTable.Rows.Count < 1;

        }

        public static List<T> ConvertDataTableToClassList<T>(this DataTable dt)
        {
            var myObject = (T)Activator.CreateInstance(typeof(T));// new T();// CreateTypee<T>();
            var result = new List<T>();

            DataColumnCollection columns = dt.Columns;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    myObject = (T)Activator.CreateInstance(typeof(T));
                    foreach (PropertyInfo p in typeof(T).GetProperties())
                    {
                        if (columns.Contains(p.Name))
                        {
                            try
                            {
                                p.SetValue(myObject, dt.Rows[i][p.Name], null);
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                    result.Add(myObject);
                }
                catch (Exception ex)
                {
                    var c = ex;
                }
            }
            return result;
        }

        public static T MapObjectToObject<T>(this object objfrom) where T : new()
        {
            var objto = new T();
            var ToProperties = objto.GetType().GetProperties();
            var FromProperties = objfrom.GetType().GetProperties();
            try
            {
                ToProperties.ToList().ForEach(o =>
                {
                    var fromp = FromProperties.FirstOrDefault(x => x.Name == o.Name && x.PropertyType == o.PropertyType);
                    if (fromp != null)
                    {
                        o.SetValue(objto, fromp.GetValue(objfrom));
                    }
                });
            }
            catch (Exception ex)
            {


            }



            return objto;
        }
        /// <summary>
        /// Bir listeyi Diğer listeye çevirmek için kullanılır.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="F"></typeparam>
        /// <param name="objfrom"></param>
        /// <returns></returns>
        public static List<T> MapList<T, F>(this List<F> objfrom) where T : new()
        {
            var list = new List<T>();
            try
            {
                foreach (var item in objfrom)
                {
                    list.Add(item.MapObjectToObject<T>());
                }
            }
            catch (Exception ex)
            {
            }


            return list;
        }


        public static T ConvertDataRowToClassObject<T>(this DataRow dr)
        {
            var result = (T)Activator.CreateInstance(typeof(T));// new T();// CreateTypee<T>();

            try
            {
                var columns = dr.Table.Columns;
                foreach (PropertyInfo p in typeof(T).GetProperties())
                {
                    try
                    {
                        if (columns.Contains(p.Name))
                            p.SetValue(result, dr[p.Name], null);

                    }
                    catch (Exception ex)
                    {
                        var c = ex;
                    }
                }

            }
            catch (Exception ex)
            {
            }


            return result;
        }

        //public static List<T> MapToList<T>(this DataTable dt)
        //    {
        //        var objList = new List<T>();
        //        var props = typeof(T).GetRuntimeProperties();
        //    var dr = dt.Rows[0];
        //        var colMapping = dr.GetColumnSchema()
        //            .Where(x => props.Any(y =>
        //                string.Equals(y.Name, x.ColumnName, StringComparison.CurrentCultureIgnoreCase)))
        //            .ToDictionary(key => key.ColumnName.ToLower());

        //        if (dr.HasRows)
        //        {
        //            var propertyInfos = props.ToList();
        //            while (dr.Read())
        //            {
        //                T obj = Activator.CreateInstance<T>();
        //                foreach (var prop in propertyInfos)
        //                {
        //                    var columnOrdinal = colMapping[prop.Name.ToLower()].ColumnOrdinal;
        //                    if (columnOrdinal != null)
        //                    {
        //                        var val = dr.GetValue(columnOrdinal.Value);
        //                        prop.SetValue(obj, val == DBNull.Value ? null : val);
        //                    }
        //                }

        //                objList.Add(obj);
        //            }
        //        }

        //        return objList;
        //    }
    }

}