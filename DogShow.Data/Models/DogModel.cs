using System;

namespace DogShow.Data
{
    public class DogModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string ClubName { get; set; }
        public string Breed { get; set; }
        public string DocumentInfo { get; set; }
        public string ParentsName { get; set; }
        public string DateLastVaccenation { get; set; }
        public string MasterName { get; set; }
        public string About { get; set; } = string.Empty;
        public Uri PhotoUrl { get; set; }
        public int MedalsCount { get; set; }
        public int Mark { get; set; }
    }
}
