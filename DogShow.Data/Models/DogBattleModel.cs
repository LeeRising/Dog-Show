using System;
using SQLite;

namespace DogShow.Data
{
    public class DogBattleModel
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Breed { get; set; }
        public string Members_id { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime Date_end { get; set; }
        public string Experts_id { get; set; }
    }
}
