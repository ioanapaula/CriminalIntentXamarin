using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace CriminalIntentXamarin.Droid.Fragments
{
    public class DatePickerFragment : Android.Support.V4.App.DialogFragment
    {
        public const string ExtraDate = "com.companyname.criminalintentxamarin.crime_date";
        private const string ArgDate = "date";
        private DatePicker _datePicker;
        private Button _datePickerButton;

        public static DatePickerFragment NewInstance(Date date)
        {
            var args = new Bundle();
            args.PutSerializable(ArgDate, date);

            var fragment = new DatePickerFragment
            {
                Arguments = args
            };

            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = LayoutInflater.From(Activity).Inflate(Resource.Layout.dialog_date, null);

            var date = (Date)Arguments.GetSerializable(ArgDate);
            var calendar = Calendar.Instance;
            calendar.Time = date;
            var year = calendar.Get(CalendarField.Year);
            var month = calendar.Get(CalendarField.Month);
            var day = calendar.Get(CalendarField.DayOfMonth);

            _datePicker = view.FindViewById<DatePicker>(Resource.Id.dialog_date_picker);
            _datePicker.Init(year, month, day, null);

            _datePickerButton = view.FindViewById<Button>(Resource.Id.date_picker_ok);
            _datePickerButton.Click += OkAction;

            return view;
        }

        private void OkAction(object sender, EventArgs e)
        {
            var year = _datePicker.Year;
            var month = _datePicker.Month;
            var day = _datePicker.DayOfMonth;
            var date = new GregorianCalendar(year, month, day).Time;
            SendResult(date);
        }

        private void SendResult(Date date)
        {
            var intent = new Intent();
            intent.PutExtra(ExtraDate, date);

            if (TargetFragment == null)
            {
                Activity.SetResult(Result.Ok, intent);
            }

            Activity.Finish();
        }
    }
}
