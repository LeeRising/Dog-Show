using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DogShow.Data.DataDb
{
    public class GetData
    {
        private readonly MySqlConnection _connection = new MySqlConnection($"Data Source={Constans.ServerIp};User Id={Constans.User};Password={Constans.Password};Database={Constans.DbName};Convert Zero Datetime=True");
        private MySqlCommand Command { get; set; }

        public UserModel GetLoginUser(object login, object hashPassword)
        {
            try
            {
                using (_connection)
                {
                    _connection.OpenAsync();
                    Command = new MySqlCommand("SELECT guid,rights FROM users WHERE login=@login AND password=@hashPassword", _connection);
                    Command.Parameters.AddWithValue("login", login);
                    Command.Parameters.AddWithValue("hashPassword", hashPassword);
                    var usersReader = Command.ExecuteReader();
                    if (usersReader == null) return null;
                    string guid, rights, club = string.Empty, expertState = string.Empty;
                    using (usersReader)
                    {
                        usersReader.ReadAsync();
                        guid = usersReader[0] as string;
                        rights = usersReader[1] as string;
                    }
                    Command = new MySqlCommand($"SELECT Request,Club_id FROM experts WHERE guid='{guid}'", _connection);
                    var expertReader = Command.ExecuteReader();
                    if (expertReader != null)
                    {
                        object clubId;
                        using (expertReader)
                        {
                            expertReader.ReadAsync();
                            clubId = expertReader[1];
                            expertState = expertReader[0] as string;
                        }
                        Command = new MySqlCommand($"SELECT Club_name FROM clubs WHERE id='{clubId}'", _connection);
                        club = Command.ExecuteScalar() as string;
                    }
                    Command = new MySqlCommand($"SELECT Name,Surname,Fathername,Passport_info FROM masters WHERE guid='{guid}'", _connection);
                    var dataReader = Command.ExecuteReader();
                    using (dataReader)
                    {
                        dataReader.ReadAsync();
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
        public object RegisterNewUser(object registerModel, object login, object hashPassword)
        {
            try
            {
                var registerUserModel = (UserModel) registerModel;
                var guid = Guid.NewGuid();
                using (_connection)
                {
                    _connection.OpenAsync();
                    Command = new MySqlCommand($"INSERT INTO users (guid,login,password) VALUES ('{guid}',@login,@hashPassword)", _connection);
                    Command.Parameters.AddWithValue("login", login);
                    Command.Parameters.AddWithValue("hashPassword", hashPassword);
                    Command.ExecuteNonQueryAsync();

                    Command = new MySqlCommand("INSERT INTO masters (guid,Name,Surname,Fathername,Passport_info) VALUES " +
                                               $"('{guid}',@name,@surname,@fathername,@passport_info)", _connection);
                    Command.Parameters.AddWithValue("name", registerUserModel.Name);
                    Command.Parameters.AddWithValue("surname", registerUserModel.Surname);
                    Command.Parameters.AddWithValue("fathername", registerUserModel.Fathername);
                    Command.Parameters.AddWithValue("passport_info", registerUserModel.PassportInfo);
                    Command.ExecuteNonQueryAsync();

                    Command = new MySqlCommand("SELECT id FROM clubs WHERE Club_name=@clubname", _connection);
                    Command.Parameters.AddWithValue("clubname", registerUserModel.ClubName);
                    var clubId = Command.ExecuteScalarAsync();

                    Command = new MySqlCommand($"INSERT INTO experts (guid,Club_id,Request) VALUES ('{guid}','{clubId}',@expert)", _connection);
                    Command.Parameters.AddWithValue("expert", registerUserModel.ExpertState);
                    Command.ExecuteNonQueryAsync();
                }
                return "User Succesfull Registered";
            }
            catch (Exception e)
            {
                return e;
            }
        }
        public object RegisterNewDog(DogModel registerDogModel, object guid)
        {
            try
            {
                using (_connection)
                {
                    _connection.OpenAsync();
                    Command = new MySqlCommand("SELECT id FROM clubs WHERE Club_name=@clubname", _connection);
                    Command.Parameters.AddWithValue("clubname", registerDogModel.ClubName);
                    var clubId = Command.ExecuteScalarAsync();

                    Command = new MySqlCommand("INSERT INTO dogs " +
                                               "(Club_id,Name,Breed,Age,Document_info,Parents_info,Date_last_vaccenation,Master_guid,Photo,About) VALUES " +
                                               $"('{clubId}',@name,@breed,@age,@doc,@parents,@date,'{guid}',@photo,@about)", _connection);
                    Command.Parameters.AddWithValue("name", registerDogModel.Name);
                    Command.Parameters.AddWithValue("breed", registerDogModel.Breed);
                    Command.Parameters.AddWithValue("age", registerDogModel.Age);
                    Command.Parameters.AddWithValue("doc", registerDogModel.DocumentInfo);
                    Command.Parameters.AddWithValue("parents", registerDogModel.ParentsName);
                    Command.Parameters.AddWithValue("date", registerDogModel.DateLastVaccenation);
                    Command.Parameters.AddWithValue("photo", registerDogModel.PhotoUrl);
                    Command.Parameters.AddWithValue("about", registerDogModel.About);
                    Command.ExecuteNonQueryAsync();
                    return "Dog Succesfull Register";
                }
            }
            catch (Exception e)
            {
                return e;
            }
        }
        public object UpdatePassword(string hashPassword, object guid)
        {
            try
            {
                using (_connection)
                {
                    _connection.OpenAsync();
                    Command = new MySqlCommand("UPDATE users SET password=@hashpassword WHERE guid=@guid", _connection);
                    Command.Parameters.AddWithValue("hashPassword", hashPassword);
                    Command.Parameters.AddWithValue("guid", guid);
                    Command.ExecuteNonQueryAsync();
                    return "Password was updated";
                }
            }
            catch (Exception e)
            {
                return e;
            }
        }
        public object UpdateDogInfo(DogModel updateDogModel)
        {
            try
            {
                using (_connection)
                {
                    _connection.OpenAsync();
                    Command = new MySqlCommand($"",_connection);
                    return "Dog info was updated";
                }
            }
            catch (Exception e)
            {
                return e;
            }
        }
        public List<string> GetClubsName()
        {
            try
            {
                using (_connection)
                {
                    _connection.OpenAsync();
                    Command = new MySqlCommand("SELECT Club_name FROM clubs", _connection);
                    var dataReader = Command.ExecuteReader();
                    var list = new List<string>();
                    for (var i = 0; i < dataReader.RecordsAffected; i++)
                    {
                        list.Add(dataReader[i].ToString());
                    }
                    return list;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}