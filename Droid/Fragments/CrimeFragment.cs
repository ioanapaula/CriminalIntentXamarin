using System;
using Android.Content;
using Android.Content.PM;
using Android.Icu.Text;
using Android.OS;
using Android.Provider;
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
        private const int RequestContact = 1;

        private Intent _pickContactIntent;
        private Crime _crime;
        private CrimeLab _crimeLab;
        private EditText _titleField;
        private Button _dateButton;
        private Button _reportButton;
        private Button _suspectButton;
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

            _reportButton.Click += ReportButtonClicked;
            _suspectButton.Click += SuspectButtonClicked;

            if (_crime.Suspect != null)
            {
                _suspectButton.Text = _crime.Suspect;
            }

            var packageManager = Activity.PackageManager;
            if (packageManager.ResolveActivity(_pickContactIntent, PackageInfoFlags.MatchDefaultOnly) == null)
            {
                _suspectButton.Enabled = false;
            }

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
            else if (requestCode == RequestContact && data != null)
            {
                var contactUri = data.Data;
                string[] queryFields = new string[] { ContactsContract.Contacts.InterfaceConsts.DisplayName };

                var cursor = Activity.ContentResolver.Query(contactUri, queryFields, null, null, null);
                try
                {
                    if (cursor.Count == 0)
                    {
                        return;
                    }

                    cursor.MoveToFirst();
                    var suspect = cursor.GetString(0);
                    _crime.Suspect = suspect;
                    _suspectButton.Text = suspect;
                }
                finally
                {
                    cursor.Close();
                }
            }
        }

        private void DateButtonClicked(object sender, EventArgs e)
        {
            var fm = Activity.SupportFragmentManager;
            var dialog = DatePickerFragment.NewInstance(_crime.Date);
            dialog.SetTargetFragment(this, RequestDate);
            dialog.Show(fm, DialogDate);
        }

        private void ReportButtonClicked(object sender, EventArgs e)
        {
            var intent = new Intent(Intent.ActionSend);
            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraText, GetCrimeReport());
            intent.PutExtra(Intent.ExtraSubject, GetString(Resource.String.crime_report_subject));
            intent = Intent.CreateChooser(intent, GetString(Resource.String.send_report));
            StartActivity(intent);
        }

        private void SuspectButtonClicked(object sender, EventArgs e)
        {
            StartActivityForResult(_pickContactIntent, RequestContact);
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            _crime.Title = e.Text.ToString();
        }

        private void CheckBoxChecked(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            _crime.Solved = e.IsChecked;
        }

        private string GetCrimeReport()
        {
            string solvedString = null;

            if (_crime.Solved)
            {
                solvedString = GetString(Resource.String.crime_report_solved);
            }
            else
            {
                solvedString = GetString(Resource.String.crime_report_unsolved);
            }

            var dateFormat = new SimpleDateFormat("EEE, MMM dd");
            var dateString = dateFormat.Format(_crime.Date);

            var suspect = _crime.Suspect;

            if (suspect == null)
            {
                suspect = GetString(Resource.String.crime_report_no_suspect);
            }
            else
            {
                suspect = GetString(Resource.String.crime_report_suspect, suspect);
            }

            var report = GetString(Resource.String.crime_report, _crime.Title, dateString, solvedString, suspect);

            return report;
        }

        private void InitFields(View view)
        {
            _titleField = view.FindViewById<EditText>(Resource.Id.crime_title);
            _dateButton = view.FindViewById<Button>(Resource.Id.crime_date);
            _solvedCheckBox = view.FindViewById<CheckBox>(Resource.Id.crime_solved);
            _reportButton = view.FindViewById<Button>(Resource.Id.crime_report);
            _suspectButton = view.FindViewById<Button>(Resource.Id.crime_suspect);
            _pickContactIntent = new Intent(Intent.ActionPick, ContactsContract.Contacts.ContentUri);
        }
    }
}
