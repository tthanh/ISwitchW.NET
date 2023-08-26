using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ISwitchW.NET
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ListItem> Items = new ObservableCollection<ListItem>();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            // Add items to the collection
            Items.Add(new ListItem { Text = "Item 1" });
            Items.Add(new ListItem { Text = "Item 2" });
            Items.Add(new ListItem { Text = "Item 3" });
            Items.Add(new ListItem { Text = "Item 4" });
            Items.Add(new ListItem { Text = "Item 5" });

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

            //this.KeyDown +
            this.Deactivated += (sender, args) => 
            {
                Items.Clear();
                // Hide Clear Window 
            };

            this.Activated += async (sender, args) =>
            {
                await foreach (var p in GetProcesses())
                {
                    Items.Add(new ListItem { Text = p });
                }
            };

            autocompleteTextBox.TextChanged += AutocompleteTextBox_TextChanged;
            autocompleteResult.ItemsSource = Items;
            var canm = autocompleteResult.Items.MoveCurrentToFirst();
        }

        private async IAsyncEnumerable<string> GetProcesses()
        {
            Process[] processes = Process.GetProcesses();
            foreach (Process p in processes)
            {
                if (!string.IsNullOrEmpty(p.MainWindowTitle))
                {
                    yield return p.MainWindowTitle;
                }
            }
        }
        
        private void AutocompleteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Items.Clear();

            //var rand = Random.Shared.Next(0, 10);
            //for (int i = 0; i < rand; i++)
            //{
            //    Items.Add(new ListItem { Text = "Item " + i });
            //}


            this.UpdateLayout();
            if (Items.Count > 0)
            {
                autocompleteResult.SelectedIndex = 0;
                var aaa = (ListBoxItem)autocompleteResult.ItemContainerGenerator.ContainerFromIndex(0);
                aaa.Focus();
            }
            
            


            //string searchText = autocompleteTextBox.Text.ToLower();
            //List<string> filteredSuggestions = new List<string>();

            //foreach (string suggestion in suggestionList)
            //{
            //    if (suggestion.ToLower().Contains(searchText))
            //    {
            //        filteredSuggestions.Add(suggestion);
            //    }
            //}

            // Clear previous suggestions and show new suggestions
            //autocompleteTextBox.ItemsSource = filteredSuggestions;
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {


            switch (e.Key)
            {
                case Key.Escape:
                    WindowState = WindowState.Minimized;
                    //Close();
                    // Hide windows
                    // Clear things
                    break;
                case Key.Up:
                    if (autocompleteResult.SelectedIndex > 0)
                    {
                        var selected = (ListBoxItem)autocompleteResult.ItemContainerGenerator.ContainerFromIndex(autocompleteResult.SelectedIndex - 1);
                        selected.Focus();
                    }
                    e.Handled = true;

                    break;
                case Key.Down:
                    if (autocompleteResult.SelectedIndex < autocompleteResult.Items.Count - 1)
                    {
                        var selected = (ListBoxItem)autocompleteResult.ItemContainerGenerator.ContainerFromIndex(autocompleteResult.SelectedIndex + 1);
                        selected.Focus();
                    }
                    e.Handled = true;

                    break;
                //case Key.PageUp:
                //    if (autocompleteResult.Items.Count > 0)
                //    {
                //        autocompleteResult.SelectedIndex = autocompleteResult.Items.Count - 1;
                //        var selected = (ListBoxItem)autocompleteResult.ItemContainerGenerator.ContainerFromIndex(autocompleteResult.SelectedIndex - 1);
                //        selected.Focus();
                //        e.Handled = true;
                //    }
                //    break;
                //case Key.PageDown:
                //    if (autocompleteResult.Items.Count > 0)
                //    {
                //        autocompleteResult.SelectedIndex = autocompleteResult.Items.Count - 1;
                //        var selected = (ListBoxItem)autocompleteResult.ItemContainerGenerator.ContainerFromIndex(autocompleteResult.SelectedIndex - 1);
                //        selected.Focus();
                //        e.Handled = true;
                //    }
                //    break;
                case Key.Return:
                    // switch to windows
                    WindowState = WindowState.Minimized;
                    autocompleteTextBox.Clear();
                    Items.Clear();
                    // Clear data
                    break;
                default:
                    autocompleteTextBox.Focus();
                    break;
            }

        }
    }

    public class ListItem
    {
        public string Text { get; set; }
    }

}
