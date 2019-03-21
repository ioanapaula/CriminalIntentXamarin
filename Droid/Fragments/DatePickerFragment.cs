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

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var date = (Date)Arguments.GetSerializable(ArgDate);
            var calendar = Calendar.Instance;
            calendar.Time = date;
            var year = calendar.Get(CalendarField.Year);
            var month = calendar.Get(CalendarField.Month);
            var day = calendar.Get(CalendarField.DayOfMonth);

            var view = LayoutInflater.From(Activity).Inflate(Resource.Layout.dialog_date, null);

            _datePicker = view.FindViewById<DatePicker>(Resource.Id.dialog_date_picker);
            _datePicker.Init(year, month, day, null);

            return new Android.Support.V7.App.AlertDialog.Builder(Activity)
                .SetView(view)
                .SetTitle(Resource.String.date_picker_title)
                .SetPositiveButton(Android.Resource.String.Ok, OkAction)
                .Create();
        }

        private void OkAction(object sender, DialogClickEventArgs e)
        {
            var year = _datePicker.Year;
            var month = _datePicker.Month;
            var day = _datePicker.DayOfMonth;
            var date = new GregorianCalendar(year, month, day).Time;
            SendResult((int)Result.Ok, date);
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
