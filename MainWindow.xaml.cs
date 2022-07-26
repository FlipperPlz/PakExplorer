using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using PakExplorer.Pak;
using PakExplorer.Tree;
using PakExplorer.Tree.Items;

namespace PakExplorer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private readonly ObservableCollection<ITreeItem> PakList = new();
        
        private PakTreeItem? CurrentPak { get; set; }
        private EntryTreeItem? SelectedEntry { get; set; } 
        
        public MainWindow() {
            InitializeComponent();
            PakView.ItemsSource = PakList;
        }

        private void OpenFile(object sender, ExecutedRoutedEventArgs e) {
            var dlg = new OpenFileDialog();
            dlg.Title = "Load PAK archive";
            dlg.DefaultExt = ".pak";
            dlg.Filter = "PAK File|*.pak|Preview BI Files|*.pak";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == true) {
                LoadPAK(dlg.FileName);
                
            }
        }

        public void LoadPAK(string path) {
            var pak = new PakTreeItem(new Pak.Pak(path));
            PakList.Add(pak);
        }

        private void ShowPakEntry(object sender, RoutedPropertyChangedEventArgs<object> e) {
            ResetView();
            Cursor = Cursors.Wait;
            switch (e.NewValue) {
                case EntryTreeItem entry:
                    SelectedEntry = entry;
                    Show(entry);
                    break;
                case DirectoryTreeItem directory:
                    break;
            }
            Cursor = Cursors.Arrow;
        }
        
        private void ResetView() {
            AboutBox.Visibility = Visibility.Hidden;
            TextPreview.Visibility = Visibility.Hidden;
            TextPreview.Text = string.Empty;
            SelectedEntry = null;
            CurrentPak = null;
        }

        private void Show(IFileItem entry) {
            TextPreview.Text = Encoding.UTF8.GetString(entry.EntryData);
            TextPreview.Visibility = Visibility.Visible;
        }
    }
}