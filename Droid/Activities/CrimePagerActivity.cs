using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Widget;
using CriminalIntentXamarin.Droid.Data;
using Java.Util;

namespace CriminalIntentXamarin.Droid.Activities
{
    [Activity(Label = "CriminalIntentXamarin", Theme = "@style/AppTheme")]
    public class CrimePagerActivity : AppCompatActivity
    {
        private const string ExtraCrimeId = "com.companyname.criminalintentxamarin.crime_id";

        private ViewPager _viewPager;
        private ImageButton _leftmostButton;
        private ImageButton _rightmostButton;
        private List<Crime> _crimes;

        public static Intent NewIntent(Context packageContext, UUID crimeId)
        {
            var intent = new Intent(packageContext, typeof(CrimePagerActivity));
            intent.PutExtra(ExtraCrimeId, crimeId);

            return intent;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_crime_pager);

            var crimeId = (UUID)this.Intent.GetSerializableExtra(ExtraCrimeId);

            _viewPager = FindViewById<ViewPager>(Resource.Id.crime_view_pager);
            _rightmostButton = FindViewById<ImageButton>(Resource.Id.jump_to_last_button);
            _leftmostButton = FindViewById<ImageButton>(Resource.Id.jump_to_first_button);

            _crimes = CrimeLab.Get(this).Crimes;
            _rightmostButton.Click += RightmostButtonClicked;
            _leftmostButton.Click += LeftmostButtonClicked;

            var fm = SupportFragmentManager;
            _viewPager.Adapter = new CrimePagerAdapter(_crimes, fm);
            _viewPager.SetCurrentItem(_crimes.IndexOf(_crimes.FirstOrDefault(crime => crime.Id.Equals(crimeId))), false);
        }

        private void LeftmostButtonClicked(object sender, EventArgs e)
        {
            _viewPager.SetCurrentItem(0, false);
        }

        private void RightmostButtonClicked(object sender, EventArgs e)
        {
            _viewPager.SetCurrentItem(_crimes.Count - 1, false);
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
