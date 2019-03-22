using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using CriminalIntentXamarin.Droid.Fragments;
using Java.Util;

namespace CriminalIntentXamarin.Droid.Activities
{
    [Activity(Label = "PickerActivity", Theme = "@style/AppTheme")]
    public class PickerActivity : SingleFragmentActivity
    {
        private const string ExtraDate = "com.companyname.criminalintentxamarin.crime_date";

        public static Intent NewIntent(Context packageContext, Date date)
        {
            var intent = new Intent(packageContext, typeof(PickerActivity));
            intent.PutExtra(ExtraDate, date);

            return intent;
        }

        protected override Android.Support.V4.App.Fragment CreateFragment()
        {
            var date = (Date)Intent.GetSerializableExtra(ExtraDate);

            return DatePickerFragment.NewInstance(date);
        }
    }
}
