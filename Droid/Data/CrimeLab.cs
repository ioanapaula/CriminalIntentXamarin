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
            for (int i = 0; i < 100; i++)
            {
                Crime crime = new Crime();
                crime.Title = "Crime #" + i;
                crime.Solved = i % 2 == 0;
                crime.RequiresPolice = i % 3 == 0;
                Crimes.Add(crime);
            }
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
    }
}
