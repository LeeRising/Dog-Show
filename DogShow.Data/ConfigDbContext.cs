using System.IO;
using SQLite;

namespace DogShow.Data
{
    public class ConfigDbContext
    {
        private const string DbPath = "user.sqlite3";
        public string Log { get; set; }

        public LogingUser NullUser => new LogingUser
        {
            Login = "null",
            Password = "null"
        };

        public ConfigDbContext()
        {
            if (File.Exists(DbPath)) return;
            Log = createDatabase();
            Log += insertUpdateData(NullUser);
        }

        private string createDatabase()
        {
            try
            {
                var connection = new SQLiteAsyncConnection(DbPath);
                connection.CreateTableAsync<LogingUser>();
                return "Database created";
            }

            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
        private string insertUpdateData(LogingUser data)
        {
            try
            {
                var db = new SQLiteAsyncConnection(DbPath);
                db.UpdateAsync(data);
                return "Single data file inserted or updated";
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
        public int SelectUser(string login, string password)
        {
            try
            {
                var db = new SQLiteAsyncConnection(DbPath);
                var count = db.ExecuteScalarAsync<int>($"SELECT Count(*) FROM LogingUser WHERE Login={login} AND Password={password}");
                return count.Result;
            }
            catch (SQLiteException)
            {
                return -1;
            }
        }
    }
}
