using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System;
using System.Windows.Media.Media3D;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace VerzeichnisÜbersicht
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Storyboard spinnerStoryboard;
        int order = 1;
        public MainWindow()
        {
            InitializeComponent();
            
            listFiles();
            var t = new DispatcherTimer();
            t.Interval = TimeSpan.FromSeconds(10);
            t.Tick += (s, e) =>
            {
                
                listFiles();

            };
            t.Start();

        }

     

        


        public void listFiles()
        {
            {
                FilesListBox.Items.Clear();
                //Arrray Path
                string[] files = Directory.GetFiles(@"yourPath", "*", SearchOption.AllDirectories);
                //Array Time

                var filesInfo = files.Select(value => new FileInfo(value)).ToArray();
                if (order == 1)
                {
                    var sortedFilesInfo = filesInfo.OrderBy(f => f.CreationTime).ToArray();
                    foreach (var file in sortedFilesInfo)
                    {
                        FilesListBox.Items.Add("Vom: " + file.CreationTime + "  Pfad: " + file);
                    }
                }
                else if (order == 0)
                {

                    var sortedFilesInfo = filesInfo.OrderByDescending(f => f.CreationTime).ToArray();
                    foreach (var file in sortedFilesInfo)
                    {
                        FilesListBox.Items.Add("Vom: " + file.CreationTime + "  Pfad: " + file);
                    }
                }




            }

        }

        private void DataGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SortierungComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortierungComboBox.Text == "Aufsteigend")
            {
                order = 0;
                listFiles();
            }
            else if (SortierungComboBox.Text == "Absteigend")
            {
                order = 1;
                listFiles();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilesListBox.SelectedItem != null)
            {
                string file = FilesListBox.SelectedItem as string;
                int diff = file.Length - 32;
                string path = file.Substring(32, diff);

                MessageBoxResult result = MessageBox.Show(
                $"Willst du die Datei: {path} dauerhaft löschen?", // Text
                "Datei Löschen",                  // Titel des Fensters
                MessageBoxButton.YesNo,         // Welche Buttons angezeigt werden
                MessageBoxImage.Question        // Icon: Fragezeichen
           );
                if (result == MessageBoxResult.Yes)
                {
                    File.Delete(path);
                    listFiles();
                }
            }
            else if (FilesListBox.SelectedItem == null)
            {
                MessageBox.Show("Bitte eine Datei Auswählen!!!");
            }
        }
    }
}
