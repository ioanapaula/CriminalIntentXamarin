using System;
using Android.App;
using Android.Runtime;

namespace CriminalIntentXamarin.Droid
{
    [Application(AllowBackup = false)]
    public class CriminalIntentApplication : Application
    {
        public CriminalIntentApplication(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
        {
        }
    }
}
