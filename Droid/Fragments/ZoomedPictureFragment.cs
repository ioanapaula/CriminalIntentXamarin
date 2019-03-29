using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Uri = Android.Net.Uri;

namespace CriminalIntentXamarin.Droid.Fragments
{
    public class ZoomedPictureFragment : Android.Support.V4.App.DialogFragment
    {
        private const string ArgUri = "pictureUri";
        private ImageView _zoomedImageView;

        public static ZoomedPictureFragment NewInstance(string uri)
        {
            var args = new Bundle();
            args.PutString(ArgUri, uri);

            var fragment = new ZoomedPictureFragment
            {
                Arguments = args
            };

            return fragment;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var view = LayoutInflater.From(Activity).Inflate(Resource.Layout.dialog_picture, null);
            var uri = Arguments.GetString(ArgUri);

            _zoomedImageView = view.FindViewById<ImageView>(Resource.Id.zoomed_picture);
            _zoomedImageView.SetImageURI(Uri.Parse(uri));

            return new Android.Support.V7.App.AlertDialog.Builder(Activity)
                .SetView(view)
                .SetPositiveButton(Android.Resource.String.Ok, (IDialogInterfaceOnClickListener)null)
                .Create();
        }
    }
}
