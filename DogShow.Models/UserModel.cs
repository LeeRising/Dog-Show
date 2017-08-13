using System.Collections.Generic;

namespace DogShow.Models
{
    public class UserModel
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fathername { get; set; }
        public string Rights { get; set; }
        public string PassportInfo { get; set; }
        public string ExpertState { get; set; } = "none";
        public string ClubName { get; set; }
    }
}
