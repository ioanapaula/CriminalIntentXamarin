using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace CriminalIntentXamarin.Droid.Fragments
{
    public class TimePickerFragment : Android.Support.V4.App.DialogFragment
    {
        public const string ExtraDate = "com.companyname.criminalintentxamarin.crime_date";
        private const string ArgDate = "date";

        private Date _date;
        private TimePicker _timePicker;

        public static TimePickerFragment NewInstance(Date date)
        {
            var args = new Bundle();
            args.PutSerializable(ArgDate, date);

            var fragment = new TimePickerFragment
            {
                Arguments = args
            };

            return fragment;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            _date = (Date)Arguments.GetSerializable(ArgDate);
            var calendar = Calendar.Instance;
            calendar.Time = _date;
            var hour = calendar.Get(CalendarField.Hour);
            var minutes = calendar.Get(CalendarField.Minute);

            var view = LayoutInflater.From(Activity).Inflate(Resource.Layout.dialog_time, null);

            _timePicker = view.FindViewById<TimePicker>(Resource.Id.dialog_time_picker);
            _timePicker.Hour = hour;
            _timePicker.Minute = minutes;

            return new Android.Support.V7.App.AlertDialog.Builder(Activity)
                .SetView(view)
                .SetTitle(Resource.String.time_picker_title)
                .SetPositiveButton(Android.Resource.String.Ok, OkAction)
                .Create();
        }

        private void OkAction(object sender, DialogClickEventArgs e)
        {
            var hour = _timePicker.Hour;
            var minute = _timePicker.Minute;

            var calendar = Calendar.Instance;
            calendar.Time = _date;
            calendar.Set(CalendarField.Hour, hour);
            calendar.Set(CalendarField.Minute, minute);

            SendResult((int)Result.Ok, calendar.Time);
        }

        private void SendResult(int resultCode, Date date)
        {
            if (TargetFragment == null)
            {
                return;
            }

            var intent = new Intent();
            intent.PutExtra(ExtraDate, date);
            TargetFragment.OnActivityResult(TargetRequestCode, resultCode, intent);
        }
    }
}
