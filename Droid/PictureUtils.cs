using System;
using Android.App;
using Android.Graphics;

namespace CriminalIntentXamarin.Droid
{
    public class PictureUtils
    {
        public static Bitmap GetScaledBitmap(string path, int destWidth, int destHeight)
        {
            var options = new BitmapFactory.Options();
            options.InJustDecodeBounds = true;
            BitmapFactory.DecodeFile(path, options);

            float srcWidth = options.OutWidth;
            float srcHeight = options.OutHeight;

            var inSampleSize = 1;

            if (srcHeight > destHeight || srcWidth > destWidth)
            {
                var heightScale = srcHeight / destHeight;
                var widthScale = srcWidth / destWidth;

                inSampleSize = (int)Math.Round(heightScale > widthScale ? heightScale : widthScale);
            }

            options = new BitmapFactory.Options();
            options.InSampleSize = inSampleSize;

            return BitmapFactory.DecodeFile(path, options);
        }

        public static Bitmap GetScaledBitmap(string path, Activity activity)
        {
            var size = new Point();
            activity.WindowManager.DefaultDisplay.GetSize(size);

            return GetScaledBitmap(path, size.X, size.Y);
        }
    }
}
