using System;
using Java.Util;

namespace CriminalIntentXamarin.Droid.Data
{
    public class Crime
    {
        public Crime()
        {
            this.Id = UUID.RandomUUID();
            this.Date = new Date();
        }

        public UUID Id { get; }

        public string Title { get; set; }

        public Date Date { get; set; }

        public bool Solved { get; set; }

        public bool RequiresPolice { get; set; }
    }
}
