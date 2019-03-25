using System;
using Java.Util;

namespace CriminalIntentXamarin.Droid.Data
{
    public class Crime
    {
        public Crime() : this(UUID.RandomUUID())
        {
        }

        public Crime(UUID id)
        {
            Id = id;
            Date = new Date();
        }

        public UUID Id { get; }

        public string Title { get; set; }

        public Date Date { get; set; } 

        public bool Solved { get; set; }
    }
}
