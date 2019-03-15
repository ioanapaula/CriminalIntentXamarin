using Android.App;
using CriminalIntentXamarin.Droid.Data;

namespace CriminalIntentXamarin.Droid
{
    [Activity(Label = "CriminalIntentXamarin")]
    public class CrimeActivity : SingleFragmentActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return new CrimeFragment();
        }
    }
}