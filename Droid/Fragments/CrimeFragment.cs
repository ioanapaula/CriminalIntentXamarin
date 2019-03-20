using System;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace CriminalIntentXamarin.Droid.Data
{
    public class CrimeFragment : Fragment
    {
        private Crime _crime;
        private EditText _titleField;
        private Button _dateButton;
        private CheckBox _solvedCheckBox;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _crime = new Crime();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_crime, container, false);
            InitFields(view);

            _titleField.TextChanged += TextChanged;
            _dateButton.Text = _crime.Date.ToString();
            _dateButton.Enabled = false;
            _solvedCheckBox.CheckedChange += CheckBoxChecked;

            return view;
        }

        private void TextChanged(object sender, EventArgs e)
        {
            _crime.Title = _titleField.Text;
        }

        private void CheckBoxChecked(object sender, EventArgs e)
        {
            _solvedCheckBox.Checked = true;
        }

        private void InitFields(View view)
        {
            _titleField = view.FindViewById<EditText>(Resource.Id.crime_title);
            _dateButton = view.FindViewById<Button>(Resource.Id.crime_date);
        }
    }
}
