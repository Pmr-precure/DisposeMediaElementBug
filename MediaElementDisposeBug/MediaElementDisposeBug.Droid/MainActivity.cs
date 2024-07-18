using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Hardware.Usb;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;

namespace MediaElementDisposeBug.Droid;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        RequestPermissions();
    }

    public async void RequestPermissions()
    {
        var status = await Permissions.RequestAsync<Permissions.PostNotifications>();
        if (status == PermissionStatus.Granted) 
        {
            StartService(new Intent(this, typeof(AndroidService)));
        }
       

    }
}

