using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace CriminalIntentXamarin.Droid.Data
{
    public class CrimeListFragment : Fragment
    {
        private RecyclerView _crimeRecyclerView;
        private CrimeAdapter _adapter;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_crime_list, container, false);
            _crimeRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.crime_recycler_view);
            _crimeRecyclerView.SetLayoutManager(new LinearLayoutManager(container.Context));

            UpdateUI();
            return view;
        }

        private void UpdateUI()
        {
            CrimeLab crimeLab = CrimeLab.Get(Activity);
            List<Crime> crimes = crimeLab.Crimes;
            _adapter = new CrimeAdapter(crimes);
            _crimeRecyclerView.SetAdapter(_adapter);
            _adapter.NotifyDataSetChanged();
        }

        private class CrimeHolder : RecyclerView.ViewHolder
        {
            private TextView _titleTextView;
            private TextView _dateTextView;
            private Crime _crime;

            public CrimeHolder(LayoutInflater inflater, ViewGroup parent) : base(inflater.Inflate(Resource.Layout.list_item_crime, parent, false))
            {
                _titleTextView = ItemView.FindViewById<TextView>(Resource.Id.crime_title);
                _dateTextView = ItemView.FindViewById<TextView>(Resource.Id.crime_date);
                ItemView.Click += ItemViewClicked;
                }

            public void Bind(Crime crime)
            {
                _crime = crime;
                _titleTextView.Text = _crime.Title;
                _dateTextView.Text = _crime.Date.ToString();
            }

            private void ItemViewClicked(object sender, EventArgs e)
            {
                var toastText = ItemView.FindViewById<TextView>(Resource.Id.crime_title).Text;
                Toast.MakeText(ItemView.Context, toastText, ToastLength.Short).Show();
            }
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
    }
}
