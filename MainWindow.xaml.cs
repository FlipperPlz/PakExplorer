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
using PakExplorer.Es.Models;
using PakExplorer.Pak;
using PakExplorer.Tree;
using PakExplorer.Tree.Files;
using PakExplorer.Tree.Items;
using PakExplorer.Tree.Items.Es;
using PakExplorer.Tree.Items.Es.Child;

namespace PakExplorer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private bool ParseScripts = false;
        
        private readonly ObservableCollection<ITreeItem> PakList = new();
        private readonly ObservableCollection<ITreeItem> ScriptList = new();
        
        private PakTreeItem? CurrentPak { get; set; }
        private FileBase? SelectedEntry { get; set; } 
        
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
            if(ParseScripts) ScriptList.Add(new ScriptPakTreeItem(pak));
        }

        private void ShowPakEntry(object sender, RoutedPropertyChangedEventArgs<object> e) {
            ResetView();
            Cursor = Cursors.Wait;
            switch (e.NewValue) {
                case FileBase entry:
                    SelectedEntry = entry;
                    Show(entry);
                    break;
                case PakDirectoryTreeItem directory:
                    break;
            }
            Cursor = Cursors.Arrow;
        }

        private void ResetView() {
            TextPreview.Visibility = Visibility.Hidden;
            TextPreview.Text = string.Empty;
            SelectedEntry = null;
            CurrentPak = null;
        }

        private void Show(FileBase entry) {
            ResetView();
            TextPreview.Text = Encoding.UTF8.GetString(entry.EntryData);
            TextPreview.Visibility = Visibility.Visible;
        }

        private void ShowPakScript(object sender, RoutedPropertyChangedEventArgs<object> e) {
            switch (e.NewValue) {
                case EnforceScriptTreeItem esItem:
                    ResetView();
                    TextPreview.Text = esItem.Scope.ToString();
                    TextPreview.Visibility = Visibility.Visible;
                    break;
                case EnforceClassTreeItem esClassItem:
                    ResetView();
                    TextPreview.Text = esClassItem.EsClazz.ToString();
                    TextPreview.Visibility = Visibility.Visible;
                    break;
                case EnforceFunctionTreeItem esFunctionItem:
                    ResetView();
                    TextPreview.Text = esFunctionItem.EsFunction.ToString();
                    TextPreview.Visibility = Visibility.Visible;
                    break;
                case EnforceVariableTreeItem esVariableItem:
                    ResetView();
                    TextPreview.Text = esVariableItem.EsVariable.ToString() + ';';
                    TextPreview.Visibility = Visibility.Visible;
                    break;
            }
        }
        
        private void EnableScriptParsing_Click(object sender, RoutedEventArgs e) {
            ParseScripts = true;
            ScriptsTab.Visibility = Visibility.Visible;
            ScriptView.ItemsSource = ScriptList;
            foreach (var pak in PakList) ScriptList.Add(new ScriptPakTreeItem((PakTreeItem) pak));
        }

        private void DisableScriptParsing_Click(object sender, RoutedEventArgs e) {
            ParseScripts = false;
            ScriptView.ItemsSource = null;
            ScriptsTab.Visibility = Visibility.Hidden;
            ScriptList.Clear();
        }
    }
}