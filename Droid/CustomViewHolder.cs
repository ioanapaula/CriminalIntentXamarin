﻿using System;
using Android.Views;
using Android.Widget;
using CriminalIntentXamarin.Droid.Data;
using static Android.Support.V7.Widget.RecyclerView;

namespace CriminalIntentXamarin.Droid
{
    public abstract class CustomViewHolder : ViewHolder
    {
        private TextView _titleTextView;
        private TextView _dateTextView;
        private Crime _crime;

        protected CustomViewHolder(View view) : base(view)
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

        protected void ItemViewClicked(object sender, EventArgs e)
        {
            var toastText = ItemView.FindViewById<TextView>(Resource.Id.crime_title).Text;
            Toast.MakeText(ItemView.Context, toastText, ToastLength.Short).Show();
        }
    }
}