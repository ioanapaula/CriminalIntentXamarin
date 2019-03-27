using System;

namespace CriminalIntentXamarin.Droid.DataBase
{
    public class CrimeDbSchema
    {
        public sealed class CrimeTable
        {
            public const string Name = "crimes";

            public sealed class Cols
            {
                public const string Uuid = "uuid";
                public const string Title = "title";
                public const string Date = "date"; 
                public const string Solved = "solved";
                public const string Suspect = "suspect";
            }
        }
    }
}
