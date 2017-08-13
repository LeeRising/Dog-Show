using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DogShow.Models;

namespace DogShow.Data.DataDb
{
    public static class GetData
    {
        private static readonly MySqlConnection Connection = new MySqlConnection($"Data Source={Constans.ServerIp};User Id={Constans.User};Password={Constans.Password};Database={Constans.DbName};Convert Zero Datetime=True");
        private static MySqlCommand Command { get; set; }

        public static async Task<UserModel> GetLoginUser(string login, string hashPassword)
        {
            try
            {
                using (Connection)
                {
                    await Connection.OpenAsync();
                    Command = new MySqlCommand("SELECT guid,rights FROM users WHERE login=@login AND password=@hashPassword", Connection);
                    Command.Parameters.AddWithValue("login", login);
                    Command.Parameters.AddWithValue("hashPassword", hashPassword);
                    var usersReader = await Command.ExecuteReaderAsync() as MySqlDataReader;
                    if (usersReader == null) return null;
                    string guid, rights, club = string.Empty, expertState = string.Empty;
                    using (usersReader)
                    {
                        await usersReader.ReadAsync();
                        guid = usersReader[0] as string;
                        rights = usersReader[1] as string;
                    }
                    Command = new MySqlCommand($"SELECT Request,Club_id FROM experts WHERE guid='{guid}'", Connection);
                    var expertReader = await Command.ExecuteReaderAsync() as MySqlDataReader;
                    if (expertReader != null)
                    {
                        object clubId;
                        using (expertReader)
                        {
                            await expertReader.ReadAsync();
                            clubId = expertReader[1];
                            expertState = expertReader[0] as string;
                        }
                        Command = new MySqlCommand($"SELECT Club_name FROM clubs WHERE id='{clubId}'", Connection);
                        club = await Command.ExecuteScalarAsync() as string;
                    }
                    Command = new MySqlCommand($"SELECT Name,Surname,Fathername,Passport_info FROM masters WHERE guid='{guid}'", Connection);
                    var dataReader = await Command.ExecuteReaderAsync() as MySqlDataReader;
                    using (dataReader)
                    {
                        await dataReader.ReadAsync();
                        return new UserModel
                        {
                            Guid = guid,
                            Name = dataReader["Name"] as string,
                            Surname = dataReader["Surname"] as string,
                            Fathername = dataReader["Fathername"] as string,
                            PassportInfo = dataReader["Passport_info"] as string,
                            Rights = rights,
                            ExpertState = expertState,
                            ClubName = club
                        };
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static async Task<object> RegisterNewUser(UserModel registerUserModel, string login, string hashPassword)
        {
            try
            {
                var guid = Guid.NewGuid();
                using (Connection)
                {
                    await Connection.OpenAsync();
                    Command = new MySqlCommand($"INSERT INTO users (guid,login,password) VALUES ('{guid}',@login,@hashPassword)", Connection);
                    Command.Parameters.AddWithValue("login", login);
                    Command.Parameters.AddWithValue("hashPassword", hashPassword);
                    await Command.ExecuteNonQueryAsync();

                    Command = new MySqlCommand("INSERT INTO masters (guid,Name,Surname,Fathername,Passport_info) VALUES " +
                                               $"('{guid}',@name,@surname,@fathername,@passport_info)", Connection);
                    Command.Parameters.AddWithValue("name", registerUserModel.Name);
                    Command.Parameters.AddWithValue("surname", registerUserModel.Surname);
                    Command.Parameters.AddWithValue("fathername", registerUserModel.Fathername);
                    Command.Parameters.AddWithValue("passport_info", registerUserModel.PassportInfo);
                    await Command.ExecuteNonQueryAsync();

                    Command = new MySqlCommand("SELECT id FROM clubs WHERE Club_name=@clubname", Connection);
                    Command.Parameters.AddWithValue("clubname", registerUserModel.ClubName);
                    var clubId = await Command.ExecuteScalarAsync();

                    Command = new MySqlCommand($"INSERT INTO experts (guid,Club_id,Request) VALUES ('{guid}','{clubId}',@expert)", Connection);
                    Command.Parameters.AddWithValue("expert", registerUserModel.ExpertState);
                    await Command.ExecuteNonQueryAsync();
                }
                return "User Succesfull Registered";
            }
            catch (Exception e)
            {
                return e;
            }
        }
        public static async Task<object> RegisterNewDog(DogModel registerDogModel,string guid)
        {
            try
            {
                using (Connection)
                {
                    await Connection.OpenAsync();
                    Command = new MySqlCommand("SELECT id FROM clubs WHERE Club_name=@clubname", Connection);
                    Command.Parameters.AddWithValue("clubname", registerDogModel.ClubName);
                    var clubId = await Command.ExecuteScalarAsync();

                    Command = new MySqlCommand("INSERT INTO dogs " +
                                               "(Club_id,Name,Breed,Age,Document_info,Parents_info,Date_last_vaccenation,Master_guid,Photo,About) VALUES " +
                                               $"('{clubId}',@name,@breed,@age,@doc,@parents,@date,'{guid}',@photo,@about)", Connection);
                    Command.Parameters.AddWithValue("name", registerDogModel.Name);
                    Command.Parameters.AddWithValue("breed", registerDogModel.Breed);
                    Command.Parameters.AddWithValue("age", registerDogModel.Age);
                    Command.Parameters.AddWithValue("doc", registerDogModel.DocumentInfo);
                    Command.Parameters.AddWithValue("parents", registerDogModel.ParentsName);
                    Command.Parameters.AddWithValue("date", registerDogModel.DateLastVaccenation);
                    Command.Parameters.AddWithValue("photo", registerDogModel.PhotoUrl);
                    Command.Parameters.AddWithValue("about", registerDogModel.About);
                    await Command.ExecuteNonQueryAsync();
                    return "Dog Succesfull Register";
                }
            }
            catch (Exception e)
            {
                return e;
            }
        }
        public static async Task<object> UpdatePassword(string hashPassword,string guid)
        {
            try
            {
                using (Connection)
                {
                    await Connection.OpenAsync();
                    Command = new MySqlCommand("UPDATE users SET password=@hashpassword WHERE guid=@guid", Connection);
                    Command.Parameters.AddWithValue("hashPassword", hashPassword);
                    Command.Parameters.AddWithValue("guid", guid);
                    await Command.ExecuteNonQueryAsync();
                    return "Password was updated";
                }
            }
            catch (Exception e)
            {
                return e;
            }
        }
        public static async Task<object> UpdateDogInfo(DogModel updateDogModel)
        {
            try
            {
                using (Connection)
                {
                    await Connection.OpenAsync();
                    Command = new MySqlCommand($"",Connection);
                    return "Dog info was updated";
                }
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}