using System.Collections;
using System.Collections.Generic;
using Android.Content;
using Java.Util;

namespace CriminalIntentXamarin.Droid.Data
{
    public class CrimeLab
    {
        private static CrimeLab crimeLab;

        private CrimeLab(Context context)
        {
            Crimes = new List<Crime>();
            for (int i = 0; i < 100; i++)
            {
                Crime crime = new Crime();
                crime.Title = "Crime #" + i;
                crime.Solved = i % 2 == 0;
                Crimes.Add(crime);
            }
        }

        public List<Crime> Crimes { get; }

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
            foreach (Crime crime in Crimes)
            {
                if (crime.Id.Equals(id))
                {
                    return crime;
                }
            }

            return null;
        }
    }
}
