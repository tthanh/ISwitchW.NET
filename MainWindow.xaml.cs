using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Management;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Interop;
using System.IO;
using System.Xml.Linq;

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
                //Items.Clear();
                // Hide Clear Window 
            };

            this.Activated += async (sender, args) =>
            {
                await foreach (var li in GetProcesses())
                {
                    Items.Add(li);
                }
            };

            autocompleteTextBox.TextChanged += AutocompleteTextBox_TextChanged;
            autocompleteResult.ItemsSource = Items;
            var canm = autocompleteResult.Items.MoveCurrentToFirst();
        }

        private async IAsyncEnumerable<ListItem> GetProcesses()
        {
            //Process[] processes = Process.GetProcesses();
            //foreach (Process p in processes)
            //{
            //    if (!string.IsNullOrEmpty(p.MainWindowTitle))
            //    {
            //        yield return p.MainWindowTitle;
            //    }
            //}

            var aa = AAA.FindWindows();
            int i = 0;
            foreach (var item in aa)
            {
                if (i == 100)
                {
                    break;
                }
                i++;
                yield return new ListItem()
                {
                    PID = (uint)item.ToInt64(),
                    Text = (string)AAA.GetWindowText(item),
                    Icon = IconHelper.GetWindowIcon(item),
                };

                //var path = (string)enumerator.Current["ExecutablePath"];
                //if (System.IO.File.Exists(path))
                //{
                //    var icon = System.Drawing.Icon.ExtractAssociatedIcon(path);
                //    li.Icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions());
                //    yield return li;
                //}
            }

            //var query = "SELECT ProcessId, Name, ExecutablePath FROM Win32_Process";
            //using (var searcher = new ManagementObjectSearcher(query))
            //using (var results = searcher.Get())
            //{
            //    var enumerator = results.GetEnumerator();
            //    while (enumerator.MoveNext())
            //    {
            //        ListItem li = new ListItem()
            //        {
            //            PID = (uint)enumerator.Current["ProcessId"],
            //            Text = (string)enumerator.Current["Name"]
            //        };

            //        var path = (string)enumerator.Current["ExecutablePath"];
            //        if (System.IO.File.Exists(path))
            //        {
            //            var icon = System.Drawing.Icon.ExtractAssociatedIcon(path);
            //            li.Icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon.Handle, new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions());
            //            yield return li;
            //        }

            //    }
            //}
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
                aaa?.Focus();
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
                        selected?.Focus();
                    }
                    e.Handled = true;

                    break;
                case Key.Down:
                    if (autocompleteResult.SelectedIndex < autocompleteResult.Items.Count - 1)
                    {
                        var selected = (ListBoxItem)autocompleteResult.ItemContainerGenerator.ContainerFromIndex(autocompleteResult.SelectedIndex + 1);
                        selected?.Focus();
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

                    var tt = new IntPtr(Items[autocompleteResult.SelectedIndex].PID);
                    AAA.SetForegroundWindow(tt);

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
        public uint PID { get; set; }
        public string Text { get; set; }

        public ImageSource Icon { get; set; }
    }

    public static class AAA
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
        
        [DllImport("user32.dll")]
        private static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumWindowsProc enumProc, IntPtr lParam);

        // Delegate to filter which windows to include 
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        /// <summary> Get the text for the window pointed to by hWnd </summary>
        public static string GetWindowText(IntPtr hWnd)
        {
            int size = GetWindowTextLength(hWnd);
            if (size > 0)
            {
                var builder = new StringBuilder(size + 1);
                GetWindowText(hWnd, builder, builder.Capacity);
                return builder.ToString();
            }

            return String.Empty;
        }

        /// <summary> Find all windows that match the given filter </summary>
        /// <param name="filter"> A delegate that returns true for windows
        ///    that should be returned and false for windows that should
        ///    not be returned </param>
        public static IEnumerable<IntPtr> FindWindows(EnumWindowsProc filter)
        {
            IntPtr found = IntPtr.Zero;
            List<IntPtr> windows = new List<IntPtr>();

            EnumWindows(delegate (IntPtr wnd, IntPtr param)
            {
                if (filter(wnd, param))
                {
                    // only add the windows that pass the filter
                    windows.Add(wnd);
                }

                // but return true here so that we iterate all windows
                return true;
            }, IntPtr.Zero);

            return windows;
        }

        public static IEnumerable<IntPtr> FindWindows()
        {
            IntPtr found = IntPtr.Zero;
            List<IntPtr> windows = new List<IntPtr>();

            EnumDesktopWindows(IntPtr.Zero, delegate(IntPtr wnd, IntPtr param)
            {
                if (!string.IsNullOrEmpty(GetWindowText(wnd)) && IsWindowVisible(wnd))
                {
                    windows.Add(wnd);
                }

                // but return true here so that we iterate all windows
                return true;
            }, IntPtr.Zero);

            return windows;
        }

        /// <summary> Find all windows that contain the given title text </summary>
        /// <param name="titleText"> The text that the window title must contain. </param>
        public static IEnumerable<IntPtr> FindWindowsWithText(string titleText)
        {
            return FindWindows(delegate (IntPtr wnd, IntPtr param)
            {
                return GetWindowText(wnd).Contains(titleText);
            });
        }

        //public static IEnumerable<IntPtr> FindWindows()
        //{
        //    return FindWindows(delegate (IntPtr wnd, IntPtr param)
        //    {
        //        return true;
        //    });
        //}
        [DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr wnd);

        [DllImport("User32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);
    }

    public static class IconHelper
    {
        public static BitmapSource GetForegroundWindowIcon()
        {
            var hwnd = GetForegroundWindow();
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            Process proc = Process.GetProcessById((int)pid);
            // modern apps run under ApplicationFrameHost host process in windows 10
            // don't forget to check if that is true for windows 8 - maybe they use another host there
            if (proc.MainModule.ModuleName == "ApplicationFrameHost.exe")
            {
                // this should be modern app
                return GetModernAppLogo(hwnd);
            }
            return GetWindowIcon(hwnd);
        }

        public static BitmapSource GetModernAppLogo(IntPtr hwnd)
        {
            // get folder where actual app resides
            var exePath = GetModernAppProcessPath(hwnd);
            var dir = System.IO.Path.GetDirectoryName(exePath);
            var manifestPath = System.IO.Path.Combine(dir, "AppxManifest.xml");
            if (File.Exists(manifestPath))
            {
                // this is manifest file
                string pathToLogo;
                using (var fs = File.OpenRead(manifestPath))
                {
                    var manifest = XDocument.Load(fs);
                    const string ns = "http://schemas.microsoft.com/appx/manifest/foundation/windows10";
                    // rude parsing - take more care here
                    pathToLogo = manifest.Root.Element(XName.Get("Properties", ns)).Element(XName.Get("Logo", ns)).Value;
                }
                // now here it is tricky again - there are several files that match logo, for example
                // black, white, contrast white. Here we choose first, but you might do differently
                string finalLogo = null;
                // serach for all files that match file name in Logo element but with any suffix (like "Logo.black.png, Logo.white.png etc)
                foreach (var logoFile in Directory.GetFiles(System.IO.Path.Combine(dir, System.IO.Path.GetDirectoryName(pathToLogo)),
                    System.IO.Path.GetFileNameWithoutExtension(pathToLogo) + "*" + System.IO.Path.GetExtension(pathToLogo)))
                {
                    finalLogo = logoFile;
                    break;
                }

                if (System.IO.File.Exists(finalLogo))
                {
                    using (var fs = File.OpenRead(finalLogo))
                    {
                        var img = new BitmapImage()
                        {
                        };
                        img.BeginInit();
                        img.StreamSource = fs;
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.EndInit();
                        return img;
                    }
                }
            }
            return null;
        }

        private static string GetModernAppProcessPath(IntPtr hwnd)
        {
            uint pid = 0;
            GetWindowThreadProcessId(hwnd, out pid);
            // now this is a bit tricky. Modern apps are hosted inside ApplicationFrameHost process, so we need to find
            // child window which does NOT belong to this process. This should be the process we need
            var children = GetChildWindows(hwnd);
            foreach (var childHwnd in children)
            {
                uint childPid = 0;
                GetWindowThreadProcessId(childHwnd, out childPid);
                if (childPid != pid)
                {
                    // here we are
                    Process childProc = Process.GetProcessById((int)childPid);
                    return childProc.MainModule.FileName;
                }
            }

            throw new Exception("Cannot find a path to Modern App executable file");
        }

        public static BitmapSource GetWindowIcon(IntPtr windowHandle)
        {
            var hIcon = default(IntPtr);
            hIcon = SendMessage(windowHandle, WM_GETICON, (IntPtr)ICON_BIG, IntPtr.Zero);

            if (hIcon == IntPtr.Zero)
                hIcon = GetClassLongPtr(windowHandle, GCL_HICON);

            if (hIcon == IntPtr.Zero)
            {
                hIcon = LoadIcon(IntPtr.Zero, (IntPtr)0x7F00 /*IDI_APPLICATION*/);
            }

            if (hIcon != IntPtr.Zero)
            {
                return Imaging.CreateBitmapSourceFromHIcon(hIcon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            else
            {
                throw new InvalidOperationException("Could not load window icon.");
            }
        }

        #region Helper methods
        const UInt32 WM_GETICON = 0x007F;
        const int ICON_BIG = 1;
        const int GCL_HICON = -14;

        private static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }
            list.Add(handle);
            //  You can modify this to check to see if you want to cancel the operation, then return a null here
            return true;
        }

        public delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);
        [DllImport("user32.Dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr parentHandle, EnumWindowProc callback, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out uint processId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        private static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size > 4)
                return GetClassLongPtr64(hWnd, nIndex);
            else
                return new IntPtr(GetClassLongPtr32(hWnd, nIndex));
        }

        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        public static extern uint GetClassLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        public static extern IntPtr GetClassLongPtr64(IntPtr hWnd, int nIndex);
        #endregion

    }
}
