using SQLite;

namespace DogShow.Data
{
    public class LogingUser
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}