using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace ScannerApp1
{
    public class MainViewModel : DependencyObject
    {
        public ObservableCollection<string> UserDirectories { get; set; }

        public MainViewModel()
        {
            UserDirectories = new ObservableCollection<string>();
            LoadUserDirectories();
        }

        private void LoadUserDirectories()
        {
            string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (Directory.Exists(userDirectory))
            {
                foreach (var dir in Directory.GetDirectories(userDirectory))
                {
                    UserDirectories.Add(Path.GetFileName(dir));
                }
            }
        }
    }
}
