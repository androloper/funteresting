using Helpers;
using LanguagePackageAPI.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace LanguagePackageAPI.Methods
{
    public class LanguageBll
    {

        public static List<UpdateCheckerModel> GetUpdateInfo()
        {
            var lstWords = new List<UpdateCheckerModel>();
            try
            {
                var dtLst = SqlDbHelper.GetDataTable("SELECT * FROM TblUpdateChecker", SqlConnHelper.ConnRealDb);
                if (!dtLst.IsEmpty())
                {
                    lstWords = dtLst.ConvertDataTableToClassList<UpdateCheckerModel>();
                }
                return lstWords;
            }
            catch (Exception exx)
            {
                var a = exx.Message;
                return lstWords;
            }
        }
        
        public static List<KeyValueLanguageModel> GetWords()
        {
            var lstWords = new List<KeyValueLanguageModel>();
            try
            {
                var dtLst = SqlDbHelper.GetDataTable("SELECT KeyName, Value, LanguageCode FROM TblFixedText", SqlConnHelper.ConnRealDb);
                if (!dtLst.IsEmpty())
                {
                    lstWords = dtLst.ConvertDataTableToClassList<KeyValueLanguageModel>();
                }
                return lstWords;
            }
            catch (Exception exx)
            {
                var a = exx.Message;
                return lstWords;
            }
        }

        public static List<KeyValueModel> GetWordsByLanguageCode(string languageCode)
        {
            var lstWords = new List<KeyValueModel>();
            try
            {
                var prmSql = new List<SqlParameter> { new SqlParameter("@languageCode", languageCode)};
                var dtLst = SqlDbHelper.GetDataTable("SELECT KeyName, Value FROM TblFixedText where LanguageCode=@languageCode", prmSql, SqlConnHelper.ConnRealDb);
                if (!dtLst.IsEmpty())
                {
                    lstWords = dtLst.ConvertDataTableToClassList<KeyValueModel>();
                }
                return lstWords;
            }
            catch (Exception exx)
            {
                var a = exx.Message;
                return lstWords;
            }
        }

        public static bool IsThisKeyNameAdded(string keyName)
        {
            try
            {
                var prmSql = new List<SqlParameter> { new SqlParameter("@keyName", keyName) };
                var query = "SELECT COUNT(*) FROM TblFixedText WHERE KeyName = @keyName";

                using (var conn = new SqlConnection(SqlConnHelper.ConnRealDb))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddRange(prmSql.ToArray());
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                return false;
            }
        }

        public static List<KeyValueModel> GetValuesByKeyName(string keyName)
        {
            var lstWords = new List<KeyValueModel>();
            try
            {
                var prmSql = new List<SqlParameter> { new SqlParameter("@keyName", keyName) };
                var dtLst = SqlDbHelper.GetDataTable("SELECT KeyName, Value FROM TblFixedText where KeyName=@keyName", prmSql, SqlConnHelper.ConnRealDb);
                if (!dtLst.IsEmpty())
                {
                    lstWords = dtLst.ConvertDataTableToClassList<KeyValueModel>();
                }
                return lstWords;
            }
            catch (Exception exx)
            {
                var a = exx.Message;
                return lstWords;
            }
        }


        public static List<KeyModel> GetAddedKeyNameList()
        {
            var lstWords = new List<KeyModel>();
            try
            {
                var dtLst = SqlDbHelper.GetDataTable("WITH LatestEntries AS (SELECT KeyName, CreatedDate, ROW_NUMBER() OVER (PARTITION BY KeyName ORDER BY CreatedDate DESC) AS RowNum FROM TblFixedText) SELECT KeyName, CreatedDate FROM LatestEntries WHERE RowNum = 1 ORDER BY CreatedDate desc;", SqlConnHelper.ConnRealDb);
                if (!dtLst.IsEmpty())
                {
                    lstWords = dtLst.ConvertDataTableToClassList<KeyModel>();
                }
                return lstWords;
            }
            catch (Exception exx)
            {
                var a = exx.Message;
                return lstWords;
            }
        }
        public static List<KeyValueLanguageModel> GetNewWordsByLatestUpdate(DateTime latestUpdate)
        {
            var lstWords = new List<KeyValueLanguageModel>();
            try
            {
                var prmSql = new List<SqlParameter> { new SqlParameter("@latestUpdate", latestUpdate) };
                var dtLst = SqlDbHelper.GetDataTable("SELECT KeyName, Value, LanguageCode FROM TblFixedText where CreatedDate>=@latestUpdate", prmSql, SqlConnHelper.ConnRealDb);
                if (!dtLst.IsEmpty())
                {
                    lstWords = dtLst.ConvertDataTableToClassList<KeyValueLanguageModel>();
                }
                return lstWords;
            }
            catch (Exception exx)
            {
                var a = exx.Message;
                return lstWords;
            }
        }

        static string ToLowerFirstChar(string input)
        {
            if (string.IsNullOrEmpty(input) || char.IsLower(input[0]))
                return input;

            return char.ToLower(input[0]) + input.Substring(1);
        }

        static string ToCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input) || !input.Contains(' '))
                return input;

            var words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0)
                return input;

            // İlk kelimenin tamamı küçük
            string result = words[0].ToLower();

            // Geri kalan kelimeler ilk harfi büyük diğer harfler küçük
            for (int i = 1; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    result += char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
            }

            return result;
        }

        public static bool AddNewWordToAllLanguages(string keyName, string trValue, string enValue, string deValue)
        {
            keyName = ToLowerFirstChar(keyName);
            keyName = ToCamelCase(keyName);
            try
            {
                var prmSql = new List<SqlParameter> { new SqlParameter("@keyName", keyName), new SqlParameter("@trValue", trValue), new SqlParameter("@enValue", enValue), new SqlParameter("@deValue", deValue) };
                return SqlDbHelper.ExecuteQuery("DECLARE @CreatedDate DATETIME; " +
                    "INSERT INTO TblFixedText " +
                    "(KeyName, Value, LanguageCode) " +
                    "VALUES (@keyName, @trValue, 'tr'), (@keyName, @enValue, 'en'), (@keyName, @deValue, 'de'); " +
                    "SELECT TOP 1 @CreatedDate = CreatedDate FROM TblFixedText ORDER BY Id DESC; " +
                    "UPDATE TblUpdateChecker SET LatestUpdate = @CreatedDate WHERE Id = 5;", SqlConnHelper.ConnRealDb, prmSql);
            }
            catch (Exception ex)
            {
                var a = ex;
                return false;
            }
        }
        public static bool AddNewWordByLanguageCode(string keyName, string value, string languageCode)
        {
            keyName = ToLowerFirstChar(keyName);
            keyName = ToCamelCase(keyName);
            try
            {
                var prmSql = new List<SqlParameter> { new SqlParameter("@keyName", keyName), new SqlParameter("@value", value), new SqlParameter("@languageCode", languageCode) };
                return SqlDbHelper.ExecuteQuery("DECLARE @CreatedDate DATETIME; " +
                    "INSERT INTO TblFixedText " +
                    "(KeyName, Value, LanguageCode) " +
                    "VALUES (@keyName, @value, @languageCode); " +
                    "SELECT TOP 1 @CreatedDate = CreatedDate FROM TblFixedText ORDER BY Id DESC; " +
                    "UPDATE TblUpdateChecker SET LatestUpdate = @CreatedDate WHERE Id = 5;", SqlConnHelper.ConnRealDb, prmSql);
            }
            catch (Exception ex)
            {
                var a = ex;
                return false;
            }
        }

    }
}
