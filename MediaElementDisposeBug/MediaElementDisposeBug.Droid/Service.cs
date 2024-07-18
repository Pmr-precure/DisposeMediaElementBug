using Android.App;
using Android.Content.PM;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;


namespace MediaElementDisposeBug.Droid
{
    [Service(ForegroundServiceType = global::Android.Content.PM.ForegroundService.TypeLocation)]
    public class AndroidService : Service
    {
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 33;
        private const string ForegroundChannelId = "21";
        public bool IsServiceRunning { get; set; }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        public override async void OnDestroy()
        {

            Process.KillProcess(Process.MyPid());

        }
        PendingIntent GetActionIntent(string action)
        {
            var i = new Intent(this, typeof(AndroidService));
            i.SetAction(action);
            return PendingIntent.GetService(this, 0, i, PendingIntentFlags.Immutable);
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {

            var notification =
                CreateNotification(
                "Test app",
               "Service running",
                "Stop"
                );


            // Start the service in the foreground
            // Android Q
            if (OperatingSystem.IsAndroidVersionAtLeast(29))
            {
                StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification, ForegroundService.TypeConnectedDevice);
            }
            // Android O
            else if (OperatingSystem.IsAndroidVersionAtLeast(26))
            {
                StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification);
            }
            else
            {
                StartService(intent);
            }

         
      

        return StartCommandResult.Sticky;
    }


    private Notification CreateNotification(string title, string content, string textAction = null, PendingIntent actionIntent = null)
    {
        var context = Platform.AppContext;


        var intent = new Intent(context, typeof(MainActivity));
        intent.AddFlags(ActivityFlags.SingleTop);

        PendingIntent pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.Immutable);

        var notificationBuilder =
            new NotificationCompat.Builder(context, ForegroundChannelId)
            .SetContentIntent(pendingIntent)
            .SetContentTitle(title)
            .SetStyle(new NotificationCompat.BigTextStyle())
        
            .SetContentText(content)
            .SetOngoing(true)
            ;

      


        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var notificationChannel = new NotificationChannel(ForegroundChannelId, "Test service", NotificationImportance.Low);
            notificationChannel.SetShowBadge(true);

            var notificationManager = Platform.AppContext.GetSystemService(NotificationService) as NotificationManager;
            if (notificationManager != null)
            {
                notificationBuilder.SetChannelId(ForegroundChannelId);
                notificationManager.CreateNotificationChannel(notificationChannel);
            }

        }

        return notificationBuilder.Build();
    }
}}
