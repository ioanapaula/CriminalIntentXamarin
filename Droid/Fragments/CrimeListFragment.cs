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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_crime_list, container, false);
            _crimeRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.crime_recycler_view);
            _crimeRecyclerView.SetLayoutManager(new LinearLayoutManager(container.Context));

            UpdateUI();

            return view;
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
            }
        }

        private class CrimeAdapter : RecyclerView.Adapter
        {
            private const int SimpleCrimeViewType = 0;
            private const int SeriousCrimeViewType = 1;

            private List<Crime> _crimes;

            public CrimeAdapter(List<Crime> crimes)
            {
                _crimes = crimes;
            }

            public override int ItemCount => _crimes.Count;

            public override int GetItemViewType(int position)
            {
                if (_crimes[position].RequiresPolice)
                {
                    return SeriousCrimeViewType;
                }

                return SimpleCrimeViewType;
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                LayoutInflater layoutInflater = LayoutInflater.From(parent.Context);

                if (viewType == SimpleCrimeViewType)
                {
                    return new SimpleCrimeHolder(layoutInflater, parent);
                }
                else if (viewType == SeriousCrimeViewType)
                {
                    return new SeriousCrimeHolder(layoutInflater, parent);
                }
                else
                {
                    return null;
                }
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                var crimeHolder = holder as CustomViewHolder;
                var crime = _crimes[position];
                crimeHolder.Bind(crime);
            }
        }

        private class SimpleCrimeHolder : CustomViewHolder
        {
            public SimpleCrimeHolder(LayoutInflater inflater, ViewGroup parent) : base(inflater.Inflate(Resource.Layout.list_item_crime, parent, false))
            {
            }
        }

        private class SeriousCrimeHolder : CustomViewHolder
        {
            public SeriousCrimeHolder(LayoutInflater inflater, ViewGroup parent) : base(inflater.Inflate(Resource.Layout.list_item_serious_crime, parent, false))
            {
            }
        }
    }
}
