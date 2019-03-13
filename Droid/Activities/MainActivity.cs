using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;

namespace CriminalIntentXamarin.Droid
{
    [Activity(Label = "CriminalIntentXamarin", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/AppTheme.NoActionBar")]
    public class MainActivity : AppCompatActivity
    {
        private int count = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += delegate { button.Text = $"{count++} clicks!"; };
        }
    }
}