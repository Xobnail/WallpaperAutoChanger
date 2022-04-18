using System.Timers;
using System.Collections;

namespace WallpaperAutoChanger
{
    class Program
    {
        private static System.Timers.Timer timer;
        private static List<Wallpaper>? wallpapers;
        private static IEnumerator wallpapersEnumerator;

        static void Main(string[] args)
        {
            int period;

            if (args.Length == 0)
            {
                period = 60000;
                System.Console.WriteLine("Using default value for period - 60000 ms");
            }
            else
            {
                if (!Int32.TryParse(args[0], out period))
                {
                    System.Console.WriteLine("Error! Incorrect period value. Try again. Specify in milliseconds.");
                    System.Console.WriteLine("For example: ../WallpaperAutoChanger 10000");
                    return;
                }
            }
            using (wallpapersdbContext db = new wallpapersdbContext())
            {
                wallpapers = db.Wallpapers.ToList();
            }
            wallpapersEnumerator = wallpapers.GetEnumerator();
            SetTimer(period);
            Console.WriteLine("Press any key to exit");
            Console.Read();
            timer.Stop();
            timer.Dispose();

        }
        private static void SetTimer(int period)
        {
            timer = new System.Timers.Timer(period);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (!wallpapersEnumerator.MoveNext())
            {
                wallpapersEnumerator.Reset();
                wallpapersEnumerator.MoveNext();
            }
            var imageUrl = (Wallpaper)wallpapersEnumerator.Current;
            ShellHelper.Bash("gsettings set org.gnome.desktop.background picture-uri file://" + imageUrl.Url);
        }
    }
}