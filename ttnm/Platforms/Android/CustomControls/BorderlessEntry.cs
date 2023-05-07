using Android.Content;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using ttnm.CustomControls;
using BorderlessEntryRenderer = ttnm.Platforms.Android.CustomControls.BorderlessEntry;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace ttnm.Platforms.Android.CustomControls
{
    public class BorderlessEntry : EntryRenderer
    {
        public BorderlessEntry(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.Background = null;
            }
        }
    }
}
