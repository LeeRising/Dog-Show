using SQLite;

namespace DogShow.Data
{
    public class LogingUser
    {
        [PrimaryKey]
        public int Id => 1;

        public string Login { get; set; }

        public string Password { get; set; }
    }
}