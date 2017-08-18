using System;
using System.IO;
using System.Threading.Tasks;
using SQLite;

namespace DogShow.Data
{
    public class ConfigDbContext
    {
        private const string DbPath = "dogShow.db";
        private static string _folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        private readonly SQLiteAsyncConnection _sqLiteConnection = new SQLiteAsyncConnection(Path.Combine(_folder, DbPath));

        public string Log { get; set; }

        public ConfigDbContext()
        {
            Log = CreateDatabase();
        }

        private string CreateDatabase()
        {
            var returnRes = string.Empty;
            try
            {
                _sqLiteConnection.CreateTableAsync<LogingUser>().ContinueWith(t =>
                {
                    returnRes += "Table 'LogingUser' created\n";
                });
                _sqLiteConnection.CreateTableAsync<UserModel>().ContinueWith(t =>
                {
                    returnRes += "Table 'UserModel' created\n";
                });
                _sqLiteConnection.CreateTableAsync<DogModel>().ContinueWith(t =>
                {
                    returnRes += "Table 'DogModel' created\n";
                });
                _sqLiteConnection.CreateTableAsync<DogBattleModel>().ContinueWith(t =>
                {
                    returnRes += "Table 'DogBattleModel' created\n";
                });

                return "Database created";
            }

            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
        private async Task UpdateData<T>(T data)
        {
            try
            {
                var db = new SQLiteAsyncConnection(DbPath);
                await db.UpdateAsync(data);
            }
            catch (SQLiteException)
            {
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
