using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace WallpaperAutoChanger
{
    class Program
    {
        static void Main(string[] args)
        {
            ShellHelper.Bash("gsettings set org.gnome.desktop.background picture-uri file:///home/nail/Projects/WallpaperAutoChanger/img/castle.jpg");
        }
    }
}