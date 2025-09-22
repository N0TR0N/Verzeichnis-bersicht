using IronPdf; //um pdf zu lesen 
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace VerzeichnisÜbersicht
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Storyboard spinnerStoryboard;
        int order = 0;
        List<string> newFiles = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
           
            listFiles(false);
            var t = new DispatcherTimer();
            t.Interval = TimeSpan.FromSeconds(10);
            t.Tick += (s, e) =>
            {

                listFiles(false);

            };
            t.Start();

        }

        public Boolean CheckNewFiles(string file)
        {
            string text = File.ReadAllText("log.txt");
            if (!text.Contains(file))
            {
                newFiles.Add(file);
                return true;
            }
            return false;
        }



        //  File.AppendAllText("C:\\Users\\schottmeier\\source\\repos\\VerzeichnisÜbersicht\\VerzeichnisÜbersicht\\log.txt", "Vom: " + file.CreationTime + "  Pfad: " + file);

        public void listFiles(Boolean close)
        {
            {
                
                FilesListBox.Items.Clear();
                FilesListBox.ItemsSource = null;
                //Arrray Path
                string[] files = Directory.GetFiles(@"\\mvz-nas-mst\Fülleranzeigen", "*", SearchOption.AllDirectories);
                //Array Time

                var filesInfo = files.Select(value => new FileInfo(value)).ToArray();
                if (order == 1)
                {
                    var sortedFilesInfo = filesInfo.OrderBy(f => f.CreationTime).ToArray();
                    foreach (var file in sortedFilesInfo)
                    {
                        if (CheckNewFiles("Vom: " + file.CreationTime + "  Pfad: " + file) == true)
                        {
                            var item = new ListBoxItem();
                            item.Content = "Vom: " + file.CreationTime + "  Pfad: " + file;


                            item.Foreground = Brushes.Red; // Hier die Farbe setzen

                            FilesListBox.Items.Add(item);

                       //     File.AppendAllText("C:\\Users\\schottmeier\\source\\repos\\VerzeichnisÜbersicht\\VerzeichnisÜbersicht\\log.txt", "Vom: " + file.CreationTime + "  Pfad: " + file);
                        }
                        else 
                        {
                            var item = new ListBoxItem();
                            item.Content = "Vom: " + file.CreationTime + "  Pfad: " + file;
                            FilesListBox.Items.Add(item);
                        }
                        //    File.AppendAllText("C:\\Users\\schottmeier\\source\\repos\\VerzeichnisÜbersicht\\VerzeichnisÜbersicht\\log.txt", "Vom: " + file.CreationTime + "  Pfad: " + file);
                        //FilesListBox.Items.Add("Vom: " + file.CreationTime + "  Pfad: " + file);
                    }
                }
                else if (order == 0)
                {

                    var sortedFilesInfo = filesInfo.OrderByDescending(f => f.CreationTime).ToArray();
                    foreach (var file in sortedFilesInfo)
                        if (CheckNewFiles("Vom: " + file.CreationTime + "  Pfad: " + file) == true)
                        {
                            var item = new ListBoxItem();
                            item.Content = "Vom: " + file.CreationTime + "  Pfad: " + file;


                            item.Foreground = Brushes.Red; // Hier die Farbe setzen

                            FilesListBox.Items.Add(item);

                            //     File.AppendAllText("C:\\Users\\schottmeier\\source\\repos\\VerzeichnisÜbersicht\\VerzeichnisÜbersicht\\log.txt", "Vom: " + file.CreationTime + "  Pfad: " + file);
                        }
                        else
                        {
                            var item = new ListBoxItem();
                            item.Content = "Vom: " + file.CreationTime + "  Pfad: " + file;
                            FilesListBox.Items.Add(item);
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
                listFiles(false);
            }
            else if (SortierungComboBox.Text == "Absteigend")
            {
                order = 1;
                listFiles(false);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilesListBox.SelectedItem != null)
            {
                var fileList = FilesListBox.SelectedItem as ListBoxItem;
                
                string file = fileList.ToString();
               
                string path = file.Remove(0, 69);

                MessageBoxResult result = MessageBox.Show(
                $"Willst du die Datei: {path} dauerhaft löschen?", // Text
                "Datei Löschen",                  // Titel des Fensters
                MessageBoxButton.YesNo,         // Welche Buttons angezeigt werden
                MessageBoxImage.Question        // Icon: Fragezeichen
           );
                if (result == MessageBoxResult.Yes)
                {
                    File.Delete(path);
                    listFiles(false);
                }
            }
            else if (FilesListBox.SelectedItem == null)
            {
                MessageBox.Show("Bitte eine Datei Auswählen!!!");
            }
        }
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            foreach (string t in newFiles){
                File.AppendAllText("log.txt", t);

            }
        }

        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {

            if (FilesListBox.SelectedItem != null)
            {
                var fileList = FilesListBox.SelectedItem as ListBoxItem;

                string file = fileList.ToString();

                string path = file.Remove(0, 69);

                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });

            }
            else if (FilesListBox.SelectedItem == null)
            {
                MessageBox.Show("Bitte eine Datei Auswählen!!!");
            }

           
        }
    }
}
