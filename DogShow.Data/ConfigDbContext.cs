using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace DogShow.Data
{
    public class ConfigDbContext
    {
        private const string DbName = "dogShow.db";
        private readonly string _path;

        public string Log { get; set; }

        public ConfigDbContext()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            _path = Path.Combine(folder, DbName);
            if (!File.Exists(_path)) Log = CreateDatabase().Result;
        }

        private async Task<string> CreateDatabase()
        {
            try
            {
                var db = new SQLiteAsyncConnection(_path);
                {
                    await db.CreateTableAsync<UserModel>();
                    await db.CreateTableAsync<DogModel>();
                    await db.CreateTableAsync<DogBattleModel>();
                    return "Database created";
                }
            }
            catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        public async void InsertData<T>(T data)
        {
            try
            {
                var db = new SQLiteAsyncConnection(_path);
                {
                    await db.InsertAsync(data);
                }
            }
            catch (SQLiteException)
            {
                
            }
        }
        public async void UpdateData<T>(T data)
        {
            try
            {
                var db = new SQLiteAsyncConnection(_path);
                {
                    await db.UpdateAsync(data);
                }
            }
            catch (SQLiteException)
            {
                
            }
        }

        public async void DeleteTableData<T>(T data)
        {
            try
            {
                var db = new SQLiteAsyncConnection(_path);
                {
                    await db.DeleteAsync(data);
                }
            }
            catch (SQLiteException)
            {

            }
        }

        public async Task<string> SelectUserGuid()
        {
            try
            {
                var db = new SQLiteAsyncConnection(_path);
                return await db.ExecuteScalarAsync<string>("SELECT Guid from UserModel");
            }
            catch (SQLiteException)
            {
                return null;
            }
        }
    }
}
