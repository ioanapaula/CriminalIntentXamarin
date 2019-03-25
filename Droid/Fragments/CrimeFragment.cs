using System;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using CriminalIntentXamarin.Droid.Fragments;
using Java.Util;

namespace CriminalIntentXamarin.Droid.Data
{
    public class CrimeFragment : Fragment
    {
        private const string ArgCrimeId = "crime_id";
        private const string DialogDate = "date_picker";
        private const int RequestDate = 0;

        private Crime _crime;
        private CrimeLab _crimeLab;
        private EditText _titleField;
        private Button _dateButton;
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
            _crimeLab = CrimeLab.Get(Activity);
            _crime = _crimeLab.GetCrime(crimeId);
        }

        public override void OnPause()
        {
            base.OnPause();

            CrimeLab.Get(Activity).UpdateCrime(_crime);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_crime, container, false);
            InitFields(view);

            _titleField.Text = _crime.Title;
            _titleField.TextChanged += TextChanged;
            _dateButton.Text = _crime.Date.ToString();
            _dateButton.Click += DateButtonClicked;
            _solvedCheckBox.Checked = _crime.Solved;
            _solvedCheckBox.CheckedChange += CheckBoxChecked;

            return view;
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (requestCode == RequestDate)
            {
                var date = (Date)data.GetSerializableExtra(DatePickerFragment.ExtraDate);
                _crime.Date = date;
                _dateButton.Text = _crime.Date.ToString();
            }
        }

        private void DateButtonClicked(object sender, EventArgs e)
        {
            var fm = Activity.SupportFragmentManager;
            var dialog = DatePickerFragment.NewInstance(_crime.Date);
            dialog.SetTargetFragment(this, RequestDate);
            dialog.Show(fm, DialogDate);
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
            _solvedCheckBox = view.FindViewById<CheckBox>(Resource.Id.crime_solved);
        }
    }
}
