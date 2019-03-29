using System;
using Android.Views;
using Android.Widget;
using CriminalIntentXamarin.Droid.Listeners;

namespace CriminalIntentXamarin.Droid.Extensions
{
    public static class ViewExtensions
    {
        public static void AddGlobalLayoutListener(this View self, Action action)
        {
            self.ViewTreeObserver.AddOnGlobalLayoutListener(new OnGlobalLayoutListener(self, action));
        }
    }
}
