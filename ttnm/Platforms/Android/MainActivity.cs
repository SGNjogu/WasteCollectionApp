using Android.App;
using Android.Content.PM;
using Android.OS;

namespace ttnm;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    //This override prevents the keyboard from collapsing the size of a page, when the keyboard launches
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Window.SetSoftInputMode(Android.Views.SoftInput.AdjustPan);
    }
}
