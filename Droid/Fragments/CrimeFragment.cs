using System;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Util;

namespace CriminalIntentXamarin.Droid.Data
{
    public class CrimeFragment : Fragment
    {
        private const string ArgCrimeId = "crime_id";
        private const string ArgCrimePosition = "crime_position";
        private const string ExtraPosition = "crime_position";
        private Crime _crime;
        private CrimeLab _crimeLab;
        private EditText _titleField;
        private Button _dateButton;
        private CheckBox _solvedCheckBox;

        public static CrimeFragment NewInstance(int crimePosition, UUID crimeId)
        {
            var args = new Bundle();
            args.PutSerializable(ArgCrimeId, crimeId);
            args.PutInt(ArgCrimePosition, crimePosition);

            var fragment = new CrimeFragment
            {
                Arguments = args
            };

            return fragment;
        }

        public static int GetPosition(Intent result)
        {
            return result.GetIntExtra(ExtraPosition, 0);
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
            SetActivityResult();

            _titleField.Text = _crime.Title;
            _titleField.TextChanged += TextChanged;
            _dateButton.Text = _crime.Date.ToString();
            _dateButton.Enabled = false;
            _solvedCheckBox.Checked = _crime.Solved;
            _solvedCheckBox.CheckedChange += CheckBoxChecked;

            return view;
        }

        public void SetActivityResult()
        {
            var data = new Intent();
            data.PutExtra(ExtraPosition, Arguments.GetInt(ArgCrimePosition));
            Activity.SetResult(Android.App.Result.Ok, data);
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
