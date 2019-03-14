using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using CriminalIntentXamarin.Droid.Data;

namespace CriminalIntentXamarin.Droid
{
    [Activity(Label = "CriminalIntentXamarin", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/AppTheme")]
    public class CrimeActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var fm = SupportFragmentManager;
            var fragment = fm.FindFragmentById(Resource.Id.fragment_container);

            if (fragment == null)
            {
                fragment = new CrimeFragment();
                fm.BeginTransaction().Add(Resource.Id.fragment_container, fragment).Commit();
            }
        }
    }
}