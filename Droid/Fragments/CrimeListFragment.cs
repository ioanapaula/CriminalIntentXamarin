using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Icu.Text;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
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
        private bool _subtitleVisible;
        private ICallbacks _callbacks;

        public interface ICallbacks
        {
            void OnCrimeSelected(Crime crime);
        }

        public override void OnAttach(Context context)
        {
            base.OnAttach(context);

            _callbacks = (ICallbacks)context;
        }

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

        public override void OnDetach()
        {
            base.OnDetach();

            _callbacks = null;
        }

        public override void OnResume()
        {
            base.OnResume();

            UpdateUI();
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
                    var crime = new Crime();
                    CrimeLab.Get(Activity).AddCrime(crime);
                    UpdateUI();
                    _callbacks.OnCrimeSelected(crime);

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

        public void UpdateUI()
        {
            CrimeLab crimeLab = CrimeLab.Get(Activity);
            List<Crime> crimes = crimeLab.Crimes;

            if (_adapter == null)
            {
                _adapter = new CrimeAdapter(crimes, _callbacks);
                _crimeRecyclerView.SetAdapter(_adapter);
                var touchHelperCallback = new CrimeItemtouchHelper(_adapter);
                var itemTouchHelper = new ItemTouchHelper(touchHelperCallback);
                itemTouchHelper.AttachToRecyclerView(_crimeRecyclerView);
            }
            else
            {
                _adapter.SetCrimes(crimes);
                _adapter.NotifyDataSetChanged();
            }

            UpdateSubtitle();
        }

        private void UpdateSubtitle()
        {
            CrimeLab crimeLab = CrimeLab.Get(Activity);
            var crimeCount = crimeLab.Crimes.Count;
            var subtitle = GetString(Resource.String.subtitle_format, crimeCount);

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
            private ICallbacks _callbacks;

            public CrimeAdapter(List<Crime> crimes, ICallbacks callbacks)
            {
                _crimes = crimes;
                _callbacks = callbacks;
            }

            public override int ItemCount => _crimes.Count;

            public void SetCrimes(List<Crime> crimes)
            {
                _crimes = crimes;
            }

            public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                LayoutInflater layoutInflater = LayoutInflater.From(parent.Context);

                return new CrimeHolder(layoutInflater, parent, _callbacks);
            }

            public override void OnBindViewHolder(ViewHolder holder, int position)
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
            private ICallbacks _callbacks;

            public CrimeHolder(LayoutInflater inflater, ViewGroup parent, ICallbacks callbacks) : base(inflater.Inflate(Resource.Layout.list_item_crime, parent, false))
            {
                _titleTextView = ItemView.FindViewById<TextView>(Resource.Id.crime_title);
                _dateTextView = ItemView.FindViewById<TextView>(Resource.Id.crime_date);
                _solvedImageView = ItemView.FindViewById<ImageView>(Resource.Id.imageView);
                _callbacks = callbacks;
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
                _callbacks.OnCrimeSelected(_crime);
            }
        }

        private class CrimeItemtouchHelper : ItemTouchHelper.Callback
        {
            private CrimeAdapter _adapter;

            public CrimeItemtouchHelper(CrimeAdapter adapter)
            {
                _adapter = adapter;
            }

            public override int GetMovementFlags(RecyclerView p0, ViewHolder p1)
            {
                return MakeMovementFlags(0, ItemTouchHelper.Right);
            }

            public override bool OnMove(RecyclerView recyclerView, ViewHolder viewHolder, ViewHolder target)
            {
                return false;
            }

            public override void OnSwiped(ViewHolder viewHolder, int direction)
            {
                var crimeLab = CrimeLab.Get(Application.Context);
                int position = viewHolder.AdapterPosition;
                var crime = crimeLab.Crimes[position];
                crimeLab.DeleteCrime(crime);
                _adapter.NotifyDataSetChanged();
            }
        }
    }
}
