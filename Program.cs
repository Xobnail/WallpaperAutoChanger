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
            using (wallpapersdbContext db = new wallpapersdbContext())
            {
                wallpapers = db.Wallpapers.ToList();
                
                foreach (Wallpaper wp in wallpapers)
                {
                    Console.WriteLine($"{wp.Id}.{wp.Url}");
                }
            }

            wallpapersEnumerator = wallpapers.GetEnumerator();

            SetTimer();
            Console.WriteLine("Press any key to exit");
            Console.Read();
            timer.Stop();
            timer.Dispose();
        }
        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            timer = new System.Timers.Timer(5000);
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if(!wallpapersEnumerator.MoveNext())
            {
                wallpapersEnumerator.Reset();
                wallpapersEnumerator.MoveNext();
            }
            var imageUrl = (Wallpaper)wallpapersEnumerator.Current;
            ShellHelper.Bash("gsettings set org.gnome.desktop.background picture-uri file://" + imageUrl.Url);


        }
    }
}