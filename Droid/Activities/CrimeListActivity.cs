using System;

using Android.App;
using CriminalIntentXamarin.Droid.Data;

namespace CriminalIntentXamarin.Droid.Activities
{
    [Activity(Label = "CriminalIntentXamarin", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/AppTheme")]
    public class CrimeListActivity : SingleFragmentActivity
    {
        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return new CrimeListFragment();
        }
    }
}
