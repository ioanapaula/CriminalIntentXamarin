using Android.App;
using Android.Content;
using CriminalIntentXamarin.Droid.Data;
using Java.Util;

namespace CriminalIntentXamarin.Droid
{
    [Activity(Label = "CriminalIntentXamarin", Theme = "@style/AppTheme")]
    public class CrimeActivity : SingleFragmentActivity
    {
        private const string ExtraCrimeId = "com.companyname.criminalintentxamarin.crime_id";

        public static Intent NewIntent(Context packageContext, UUID crimeId)
        {
            var intent = new Intent(packageContext, typeof(CrimeActivity));
            intent.PutExtra(ExtraCrimeId, crimeId);
            return intent;
        }

        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            var crimeId = (UUID)this.Intent.GetSerializableExtra(ExtraCrimeId);
            return CrimeFragment.NewInstance(crimeId);
        }
    }
}