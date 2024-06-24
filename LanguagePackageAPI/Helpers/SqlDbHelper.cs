using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Helpers
{

    public class SqlConnHelper
    {

        //public static string ConnLocalDb = "";

        public static string ConnRealDb = "";

        //public static void SetDbConn(string connLocal, string connReal)
        public static void SetDbConn(string connReal)
        {
            //ConnLocalDb = connLocal;
            ConnRealDb = connReal;
        }

        public static void CloseConnection()
        {
            //scSQLBaglanti.Close();
            //scSQLBaglanti.Dispose();
        }

        public static SqlConnection SQLBaglanti(string connStr)
        {
            string _connetionString = connStr;

            //if (scSQLBaglanti != null)
            //{
            //    scSQLBaglanti.Close();
            //    scSQLBaglanti.Dispose();
            //}

            var scSQLBaglanti1 = new SqlConnection(_connetionString);
            if (scSQLBaglanti1.State != ConnectionState.Open) scSQLBaglanti1.Open();
            //else
            //    scSQLBaglanti1.ConnectionString = _connetionString;

            return scSQLBaglanti1;
        }


        public static SqlConnection SQLBaglanti()
        {
            //if (scSQLBaglanti != null)
            //{
            //    scSQLBaglanti.Close();
            //    scSQLBaglanti.Dispose();
            //}

            var scSQLBaglanti = new SqlConnection(ConnRealDb);
            if (scSQLBaglanti.State != ConnectionState.Open) scSQLBaglanti.Open();
            //else
            //    scSQLBaglanti.ConnectionString = _connetionString;
            //}

            ////if (scSQLBaglanti.State != ConnectionState.Open) return scSQLBaglanti;

            ////try
            ////{
            ////	var sqlCmd1 = new SqlCommand("SET DATEFORMAT DMY", scSQLBaglanti);
            ////	sqlCmd1.ExecuteNonQuery();

            ////	var sqlCmd2 = new SqlCommand("SET LANGUAGE Turkish", scSQLBaglanti);
            ////	sqlCmd2.ExecuteNonQuery();
            ////}
            ////catch { /* ignored */ }

            return scSQLBaglanti;
        }
    }


    public class SqlDbHelper
    {
        #region CreateCommandObject

        private static SqlCommand CreateCommandObject(string sSorgu, CommandType ctSorguTipi, string sBaglantiCumlesi, SqlConnection scSQLBaglanti = null)
        {
            SqlCommand scKomut;

            if (scSQLBaglanti == null)
            {
                scKomut = SqlConnHelper.SQLBaglanti(sBaglantiCumlesi).CreateCommand();
            }
            else
            {
                scKomut = scSQLBaglanti.CreateCommand();
            }

            scKomut.CommandType = ctSorguTipi;
            scKomut.CommandText = sSorgu;

            return scKomut;
        }

        private static SqlCommand CreateCommandObject(string sSorgu, CommandType ctSorguTipi, SqlConnection scSQLBaglanti = null)
        {
            SqlCommand scKomut;

            if (scSQLBaglanti == null)
            {
                scKomut = SqlConnHelper.SQLBaglanti().CreateCommand();
            }
            else
            {
                scKomut = scSQLBaglanti.CreateCommand();
            }

            scKomut.CommandType = ctSorguTipi;
            scKomut.CommandText = sSorgu;

            return scKomut;
        }
        #endregion

        #region ExecuteScalar

        public static int ExecuteScalar(string sSorgu, List<SqlParameter> lstParametre, string connStr, CommandType ctSorguTipi = CommandType.Text)
        {
            try
            {
                sSorgu += ";SELECT SCOPE_IDENTITY();";
                var objCommand = CreateCommandObject(sSorgu, ctSorguTipi, connStr);
                objCommand.Parameters.AddRange(lstParametre.ToArray());

                var retVal = Convert.ToInt32(objCommand.ExecuteScalar());

                //if (objCommand.Connection.State != ConnectionState.Closed) objCommand.Connection.Close();
                objCommand.Connection.Close();
                objCommand.Connection.Dispose();
                objCommand.Dispose();
                return retVal;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                SqlConnHelper.CloseConnection();
            }
        }

        public static int ExecuteScalar(string sSorgu, List<SqlParameter> lstParametre, CommandType ctSorguTipi = CommandType.Text)
        {
            try
            {
                sSorgu += ";SELECT SCOPE_IDENTITY();";
                var objCommand = CreateCommandObject(sSorgu, ctSorguTipi, SqlConnHelper.ConnRealDb);
                objCommand.Parameters.AddRange(lstParametre.ToArray());

                var retVal = Convert.ToInt32(objCommand.ExecuteScalar());

                //if (objCommand.Connection.State != ConnectionState.Closed) objCommand.Connection.Close();
                objCommand.Connection.Close();
                objCommand.Connection.Dispose();
                objCommand.Dispose();
                return retVal;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                SqlConnHelper.CloseConnection();
            }
        }

        #endregion

        #region ExecuteQuery

        public static bool ExecuteQuery(string sSorgu, CommandType ctSorguTipi = CommandType.Text)
        {
            try
            {
                var objCommand = CreateCommandObject(sSorgu, ctSorguTipi);
                var retVal = (objCommand.ExecuteNonQuery() > 0);

                if (objCommand.Connection.State != ConnectionState.Closed) objCommand.Connection.Close();

                return retVal;
            }
            catch (Exception e)
            {
                return false;
            }

            finally
            {
                SqlConnHelper.CloseConnection();
            }
        }

        public static bool ExecuteQuery(string sSorgu, string connStr, CommandType ctSorguTipi = CommandType.Text)
        {
            try
            {
                var objCommand = CreateCommandObject(sSorgu, ctSorguTipi, connStr);
                var retVal = (objCommand.ExecuteNonQuery() > 0);

                if (objCommand.Connection.State != ConnectionState.Closed) objCommand.Connection.Close();

                return retVal;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                SqlConnHelper.CloseConnection();
            }
        }

        public static bool ExecuteQuery(string sSorgu, List<SqlParameter> lstParametre, CommandType ctSorguTipi = CommandType.Text)
        {
            try
            {
                var objCommand = CreateCommandObject(sSorgu, ctSorguTipi);
                objCommand.Parameters.AddRange(lstParametre.ToArray());

                var retVal = (objCommand.ExecuteNonQuery() > 0);

                if (objCommand.Connection.State != ConnectionState.Closed) objCommand.Connection.Close();

                return retVal;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                SqlConnHelper.CloseConnection();
            }

        }
        public static bool ExecuteQuery(string sSorgu, string connStr, List<SqlParameter> lstParametre, CommandType ctSorguTipi = CommandType.Text)
        {
            try
            {
                var objCommand = CreateCommandObject(sSorgu, ctSorguTipi, connStr);
                objCommand.Parameters.AddRange(lstParametre.ToArray());

                var retVal = (objCommand.ExecuteNonQuery() > 0);

                //if (objCommand.Connection.State != ConnectionState.Closed) objCommand.Connection.Close();
                objCommand.Connection.Close();
                objCommand.Connection.Dispose();
                objCommand.Dispose();
                return retVal;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                SqlConnHelper.CloseConnection();
            }
        }
        #endregion


        #region GetDataTable
        public static DataTable GetDataTable(string sSorgu, CommandType ctSorguTipi = CommandType.Text)
        {
            var objDataTable = new DataTable();

            using (var objCommand = CreateCommandObject(sSorgu, ctSorguTipi))
            {
                using (var objDataAdapter = new SqlDataAdapter(objCommand))
                {
                    try
                    {
                        objDataAdapter.Fill(objDataTable);
                    }
                    catch (Exception e)
                    {
                        objDataTable = null;
                    }
                }
                objCommand.Connection.Close();
                objCommand.Connection.Dispose();
                objCommand.Dispose();
            }
            SqlConnHelper.CloseConnection();
            return objDataTable;
        }

        public static DataTable GetDataTable(string sSorgu, string connStr, CommandType ctSorguTipi = CommandType.Text)
        {
            var objDataTable = new DataTable();

            using (var objCommand = CreateCommandObject(sSorgu, ctSorguTipi, connStr))
            {
                using (var objDataAdapter = new SqlDataAdapter(objCommand))
                {
                    try
                    {
                        objDataAdapter.Fill(objDataTable);
                    }
                    catch (Exception e)
                    {
                        objDataTable = null;
                    }
                }
                objCommand.Connection.Close();
                objCommand.Connection.Dispose();
                objCommand.Dispose();
            }
            SqlConnHelper.CloseConnection();
            return objDataTable;
        }

        public static DataTable GetDataTable(string sSorgu, List<SqlParameter> lstParametre, string connStr, CommandType ctSorguTipi = CommandType.Text)
        {
            var objDataTable = new DataTable();

            using (var objCommand = CreateCommandObject(sSorgu, ctSorguTipi, connStr))
            {
                objCommand.Parameters.AddRange(lstParametre.ToArray());
                using (var objDataAdapter = new SqlDataAdapter(objCommand))
                {
                    try
                    {
                        objDataAdapter.Fill(objDataTable);
                    }
                    catch (Exception e)
                    {
                        objDataTable = null;
                    }
                }
                objCommand.Connection.Close();
                objCommand.Connection.Dispose();
                objCommand.Dispose();
            }
            SqlConnHelper.CloseConnection();
            return objDataTable;
        }

        public static DataTable GetDataTable(string sSorgu, List<SqlParameter> lstParametre, CommandType ctSorguTipi = CommandType.Text)
        {
            var objDataTable = new DataTable();

            using (var objCommand = CreateCommandObject(sSorgu, ctSorguTipi))
            {
                objCommand.Parameters.AddRange(lstParametre.ToArray());

                using (var objDataAdapter = new SqlDataAdapter(objCommand))
                {
                    try
                    {
                        objDataAdapter.Fill(objDataTable);
                    }
                    catch (SqlException ex)
                    {
                        objDataTable = null;
                    }
                }
                objCommand.Connection.Close();
                objCommand.Connection.Dispose();
                objCommand.Dispose();
            }
            SqlConnHelper.CloseConnection();
            return objDataTable;
        }

        public static DataTable GetDataTable(string sSorgu, string connStr, List<SqlParameter> lstParametre, CommandType ctSorguTipi = CommandType.Text)
        {
            var objDataTable = new DataTable();

            using (var objCommand = CreateCommandObject(sSorgu, ctSorguTipi, connStr))
            {
                objCommand.Parameters.AddRange(lstParametre.ToArray());

                using (var objDataAdapter = new SqlDataAdapter(objCommand))
                {
                    try
                    {
                        objDataAdapter.Fill(objDataTable);
                    }
                    catch (SqlException ex)
                    {
                        objDataTable = null;
                    }
                }
                objCommand.Connection.Close();
                objCommand.Connection.Dispose();
                objCommand.Dispose();
            }
            SqlConnHelper.CloseConnection();
            return objDataTable;
        }

        #endregion

        #region GetDataSet

        public static DataSet GetDataSet(string sSorgu, CommandType ctSorguTipi, List<SqlParameter> lstParametre = null)
        {
            var objDataSet = new DataSet();

            using (var objCommand = CreateCommandObject(sSorgu, ctSorguTipi))
            {
                if (lstParametre != null)
                    objCommand.Parameters.AddRange(lstParametre.ToArray());
                using (var objDataAdapter = new SqlDataAdapter(objCommand))
                {
                    try
                    {

                        objDataAdapter.Fill(objDataSet);
                    }
                    catch (Exception e)
                    {
                        objDataSet = null;
                    }
                }

                objCommand.Connection.Close();
                objCommand.Connection.Dispose();
                objCommand.Dispose();
            }
            SqlConnHelper.CloseConnection();
            return objDataSet;
        }



        public static DataSet GetDataSet(string sSorgu, string connStr, CommandType ctSorguTipi, List<SqlParameter> lstParametre = null)
        {
            var objDataTable = new DataTable();
            var objDataSet = new DataSet();

            using (var objCommand = CreateCommandObject(sSorgu, ctSorguTipi, connStr))
            {
                if (lstParametre != null)
                    objCommand.Parameters.AddRange(lstParametre.ToArray());

                using (var objDataAdapter = new SqlDataAdapter(objCommand))
                {
                    try
                    {
                        objDataAdapter.Fill(objDataTable);
                        objDataSet.Tables.Add(objDataTable);
                    }
                    catch (Exception e)
                    {
                        objDataSet = null;
                    }
                }
                objCommand.Connection.Close();
                objCommand.Connection.Dispose();
                objCommand.Dispose();
            }
            SqlConnHelper.CloseConnection();
            return objDataSet;
        }


        #endregion

    }
}