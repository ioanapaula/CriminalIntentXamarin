using System;
using System.Collections.Generic;
using Android.App;
using Android.Icu.Text;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using CriminalIntentXamarin.Droid.Activities;
using Java.Util;
using static Android.Support.V7.Widget.RecyclerView;

namespace CriminalIntentXamarin.Droid.Data
{
    public class CrimeListFragment : Android.Support.V4.App.Fragment
    {
        private const string SavedSubtitleVisible = "subtitle";
        private const int RequestCrime = 1;
        private RecyclerView _crimeRecyclerView;
        private CrimeAdapter _adapter;
        private LinearLayout _newCrimeLayout;
        private Button _addCrimeButton;
        private bool _subtitleVisible;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_crime_list, container, false);
            _crimeRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.crime_recycler_view);
            _crimeRecyclerView.SetLayoutManager(new LinearLayoutManager(container.Context));

            _newCrimeLayout = view.FindViewById<LinearLayout>(Resource.Id.empty_list_layout);
            _addCrimeButton = view.FindViewById<Button>(Resource.Id.add_crime);
            _addCrimeButton.Click += NewCrimeButtonClicked;

            if (savedInstanceState != null)
            {
                _subtitleVisible = savedInstanceState.GetBoolean(SavedSubtitleVisible);
            }

            UpdateUI();

            return view;
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutBoolean(SavedSubtitleVisible, _subtitleVisible);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);

            inflater.Inflate(Resource.Menu.fragment_crime_list, menu);
            var subtitleItem = menu.FindItem(Resource.Id.show_subtitle);

            if (_subtitleVisible)
            {
                subtitleItem.SetTitle(Resource.String.hide_subtitle);
            }
            else
            {
                subtitleItem.SetTitle(Resource.String.show_subtitle);
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        { 
           switch (item.ItemId)
            {
                case Resource.Id.new_crime:
                    OpenCrime();

                    return true;

                case Resource.Id.show_subtitle:
                    _subtitleVisible = !_subtitleVisible;
                    Activity.InvalidateOptionsMenu();
                    UpdateSubtitle();

                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnResume()
        {
            base.OnResume();

            UpdateUI();
        }

        private void UpdateUI()
        {
            CrimeLab crimeLab = CrimeLab.Get(Activity);
            List<Crime> crimes = crimeLab.Crimes;

            if (_adapter == null)
            {
                _adapter = new CrimeAdapter(crimes);
                _crimeRecyclerView.SetAdapter(_adapter);
            }
            else
            {
                _adapter.NotifyDataSetChanged();

                if (_adapter.ItemCount == 0)
                {
                    _newCrimeLayout.Visibility = ViewStates.Visible;
                }
                else
                {
                    _newCrimeLayout.Visibility = ViewStates.Gone;
                }
            }

            UpdateSubtitle();
        }

        private void NewCrimeButtonClicked(object sender, EventArgs e)
        {
            OpenCrime();
        }

        private void OpenCrime()
        {
            var crime = new Crime();
            CrimeLab.Get(Activity).AddCrime(crime);
            var intent = CrimePagerActivity.NewIntent(Activity, crime.Id);
            StartActivity(intent);
        }

        private void UpdateSubtitle()
        {
            CrimeLab crimeLab = CrimeLab.Get(Activity);
            var crimeCount = crimeLab.Crimes.Count;
            var subtitle = Resources.GetQuantityString(Resource.Plurals.subtitle_plural, crimeCount, crimeCount);

            if (!_subtitleVisible)
            {
                subtitle = null;
            }

            var activity = (AppCompatActivity)Activity;
            activity.SupportActionBar.Subtitle = subtitle;
        }

        private class CrimeAdapter : RecyclerView.Adapter
        {
            private List<Crime> _crimes;

            public CrimeAdapter(List<Crime> crimes)
            {
                _crimes = crimes;
            }

            public override int ItemCount => _crimes.Count;

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                LayoutInflater layoutInflater = LayoutInflater.From(parent.Context);

                return new CrimeHolder(layoutInflater, parent);
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                var crimeHolder = holder as CrimeHolder;
                var crime = _crimes[position];
                crimeHolder.Bind(crime);
            }
        }

        private class CrimeHolder : ViewHolder
        {
            private TextView _titleTextView;
            private TextView _dateTextView;
            private ImageView _solvedImageView;
            private Crime _crime;

            public CrimeHolder(LayoutInflater inflater, ViewGroup parent) : base(inflater.Inflate(Resource.Layout.list_item_crime, parent, false))
            {
                _titleTextView = ItemView.FindViewById<TextView>(Resource.Id.crime_title);
                _dateTextView = ItemView.FindViewById<TextView>(Resource.Id.crime_date);
                _solvedImageView = ItemView.FindViewById<ImageView>(Resource.Id.imageView);
                ItemView.Click += ItemViewClicked;
            }

            public void Bind(Crime crime)
            {
                var simpleFormat = new SimpleDateFormat("EEEE, MMM d, yyyy");
                _crime = crime;
                _titleTextView.Text = _crime.Title;
                _dateTextView.Text = simpleFormat.Format(_crime.Date);
                _solvedImageView.Visibility = crime.Solved ? ViewStates.Visible : ViewStates.Gone;
            }

            private void ItemViewClicked(object sender, EventArgs e)
            {
                var intent = CrimePagerActivity.NewIntent(ItemView.Context, _crime.Id);
                ItemView.Context.StartActivity(intent);
            }
        }
    }
}
