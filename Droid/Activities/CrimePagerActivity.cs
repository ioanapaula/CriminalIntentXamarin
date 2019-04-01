using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.App;
using CriminalIntentXamarin.Droid.Data;
using Java.Util;

namespace CriminalIntentXamarin.Droid.Activities
{
    [Activity(Label = "CriminalIntentXamarin", Theme = "@style/AppTheme", ParentActivity = typeof(CrimeListActivity))]
    public class CrimePagerActivity : AppCompatActivity, CrimeFragment.ICallbacks
    {
        private const string ExtraCrimeId = "com.companyname.criminalintentxamarin.crime_id";

        private ViewPager _viewPager;
        private List<Crime> _crimes;

        public static Intent NewIntent(Context packageContext, UUID crimeId)
        {
            var intent = new Intent(packageContext, typeof(CrimePagerActivity));
            intent.PutExtra(ExtraCrimeId, crimeId);

            return intent;
        }

        public void OnCrimeUpdated(Crime crime)
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_crime_pager);

            var crimeId = (UUID)this.Intent.GetSerializableExtra(ExtraCrimeId);

            _viewPager = FindViewById<ViewPager>(Resource.Id.crime_view_pager);
            _crimes = CrimeLab.Get(this).Crimes;

            var fm = SupportFragmentManager;
            _viewPager.Adapter = new CrimePagerAdapter(_crimes, fm);
            _viewPager.SetCurrentItem(_crimes.IndexOf(_crimes.FirstOrDefault(crime => crime.Id.Equals(crimeId))), true);
        }

        private class CrimePagerAdapter : FragmentStatePagerAdapter
        {
            private List<Crime> _crimes;

            public CrimePagerAdapter(List<Crime> crimes, Android.Support.V4.App.FragmentManager fm) : base(fm)
            {
                _crimes = crimes;
            }

            public override int Count => _crimes.Count;

            public override Android.Support.V4.App.Fragment GetItem(int position)
            {
                var crime = _crimes[position];

                return CrimeFragment.NewInstance(crime.Id);
            }
        }
    }
}
