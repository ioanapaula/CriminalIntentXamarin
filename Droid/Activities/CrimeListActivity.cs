using System;

using Android.App;
using CriminalIntentXamarin.Droid.Data;

namespace CriminalIntentXamarin.Droid.Activities
{
    [Activity(Label = "CriminalIntentXamarin", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/AppTheme")]
    public class CrimeListActivity : SingleFragmentActivity, CrimeListFragment.ICallbacks, CrimeFragment.ICallbacks
    {
        public void OnCrimeSelected(Crime crime)
        {
            if (FindViewById(Resource.Id.detail_fragment_container) == null)
            {
                var intent = CrimePagerActivity.NewIntent(this, crime.Id);
                StartActivity(intent);
            }
            else
            {
                var newDetail = CrimeFragment.NewInstance(crime.Id);
                SupportFragmentManager.BeginTransaction().Replace(Resource.Id.detail_fragment_container, newDetail).Commit();
            }
        }

        public void OnCrimeUpdated(Crime crime)
        {
            var listFragment = (CrimeListFragment)SupportFragmentManager.FindFragmentById(Resource.Id.fragment_container);
            listFragment.UpdateUI();
        }

        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            return new CrimeListFragment();
        }

        protected override int GetLayoutResId()
        {
            return Resource.Layout.activity_masterdetail;
        }
    }
}
