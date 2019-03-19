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
        private const string ExtraPosition = "com.companyname.criminalintentxamarin.crime_position";

        public static Intent NewIntent(Context packageContext, int crimePosition, UUID crimeId)
        {
            var intent = new Intent(packageContext, typeof(CrimeActivity));
            intent.PutExtra(ExtraCrimeId, crimeId);
            intent.PutExtra(ExtraPosition, crimePosition);

            return intent;
        }

        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            var crimeId = (UUID)this.Intent.GetSerializableExtra(ExtraCrimeId);
            var crimePosition = this.Intent.GetIntExtra(ExtraPosition, 0);

            return CrimeFragment.NewInstance(crimePosition, crimeId);
        }
    }
}