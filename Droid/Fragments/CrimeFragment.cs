using System;
using Android.App;
using Android.Content;
using Android.Icu.Text;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using CriminalIntentXamarin.Droid.Activities;
using CriminalIntentXamarin.Droid.Fragments;
using Java.Util;

namespace CriminalIntentXamarin.Droid.Data
{
    public class CrimeFragment : Android.Support.V4.App.Fragment
    {
        private const string ArgCrimeId = "crime_id";
        private const string DialogDate = "date_picker";
        private const string DialogTime = "time_picker";
        private const int RequestDate = 0;
        private const int RequestTime = 1;

        private Crime _crime;
        private CrimeLab _crimeLab;
        private EditText _titleField;
        private Button _dateButton;
        private Button _timeButton;
        private CheckBox _solvedCheckBox;

        public static CrimeFragment NewInstance(UUID crimeId)
        {
            var args = new Bundle();
            args.PutSerializable(ArgCrimeId, crimeId);
            var fragment = new CrimeFragment
            {
                Arguments = args
            };

            return fragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var crimeId = (UUID)Arguments.GetSerializable(ArgCrimeId);
            _crimeLab = CrimeLab.Get(Activity.ApplicationContext);
            _crime = _crimeLab.GetCrime(crimeId);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_crime, container, false);
            InitFields(view);

            SetDateButtonText();
            _dateButton.Click += DateButtonClicked;

            SetTimeButtonText();
            _timeButton.Click += TimeButtonClicked;

            _titleField.Text = _crime.Title;
            _titleField.TextChanged += TextChanged;

            _solvedCheckBox.Checked = _crime.Solved;
            _solvedCheckBox.CheckedChange += CheckBoxChecked;

            return view;
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (resultCode != (int)Result.Ok)
            {
                return;
            }

            var date = (Date)data.GetSerializableExtra(DatePickerFragment.ExtraDate);
            _crime.Date = date;

            if (requestCode == RequestDate)
            {
                SetDateButtonText();
            }

            if (requestCode == RequestTime)
            {
                SetTimeButtonText();
            }
        }

        private void TimeButtonClicked(object sender, EventArgs e)
        {
            var fm = Activity.SupportFragmentManager;
            var dialog = TimePickerFragment.NewInstance(_crime.Date);
            dialog.SetTargetFragment(this, RequestTime);
            dialog.Show(fm, DialogTime);
        }

        private void DateButtonClicked(object sender, EventArgs e)
        {
            var fm = Activity.SupportFragmentManager;
            var intent = PickerActivity.NewIntent(Activity, _crime.Date);
            StartActivityForResult(intent, RequestDate);
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            _crime.Title = e.Text.ToString();
        }

        private void CheckBoxChecked(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            _crime.Solved = e.IsChecked;
        }

        private void InitFields(View view)
        {
            _titleField = view.FindViewById<EditText>(Resource.Id.crime_title);
            _dateButton = view.FindViewById<Button>(Resource.Id.crime_date);
            _timeButton = view.FindViewById<Button>(Resource.Id.crime_time);
            _solvedCheckBox = view.FindViewById<CheckBox>(Resource.Id.crime_solved);
        }

        private void SetDateButtonText()
        {
            var simpleFormat = new SimpleDateFormat("EEEE, MMM d, yyyy");
            _dateButton.Text = simpleFormat.Format(_crime.Date);
        }

        private void SetTimeButtonText()
        {
            var timeFormat = new SimpleDateFormat("h:mm a");
            _timeButton.Text = timeFormat.Format(_crime.Date);
        }
    }
}
