using System.Collections.Generic;
using DogShow.Data;

namespace DogShow.Android
{
    public static class DataHolder
    {
        public static UserModel User { get; set; } = null;
        public static List<string> ClubList { get; set; } = new List<string>();
    }
}