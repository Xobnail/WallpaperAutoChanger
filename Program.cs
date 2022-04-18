using System.Timers;
using System.Collections;

namespace WallpaperAutoChanger
{
    class Program
    {
        private static System.Timers.Timer _timer;
        private static List<Wallpaper> _wallpapers;
        private static IEnumerator _wallpapersEnumerator;

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
                _wallpapers = db.Wallpapers.ToList();
            }
            _wallpapersEnumerator = _wallpapers.GetEnumerator();
            SetTimer(period);
            Console.WriteLine("Press any key to exit");
            Console.Read();
            _timer.Stop();
            _timer.Dispose();

        }
        private static void SetTimer(int period)
        {
            _timer = new System.Timers.Timer(period);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (!_wallpapersEnumerator.MoveNext())
            {
                _wallpapersEnumerator.Reset();
                _wallpapersEnumerator.MoveNext();
            }
            var imageUrl = (Wallpaper)_wallpapersEnumerator.Current;
            ShellHelper.Bash("gsettings set org.gnome.desktop.background picture-uri file://" + imageUrl.Url);
        }
    }
}