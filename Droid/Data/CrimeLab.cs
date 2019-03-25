using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Java.Util;

namespace CriminalIntentXamarin.Droid.Data
{
    public class CrimeLab
    {
        private static CrimeLab crimeLab;

        private CrimeLab(Context context)
        {
        }

        public List<Crime> Crimes { get; } = new List<Crime>();

        public static CrimeLab Get(Context context)
        {
            if (crimeLab == null)
            {
                crimeLab = new CrimeLab(context);
            }

            return crimeLab;
        }

        public Crime GetCrime(UUID id)
        {
            return Crimes.FirstOrDefault(crime => crime.Id.Equals(id));
        }

        public void AddCrime(Crime crime)
        {
            Crimes.Add(crime);
        }
    }
}
