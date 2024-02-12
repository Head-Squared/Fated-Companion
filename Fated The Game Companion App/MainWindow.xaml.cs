using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;

namespace Fated_The_Game_Companion_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double animationDuration = 0.15;

        double homeBTNWidth;
        double homeBTNXPos;

        double rulesetBTNWidth;
        double rulesetBTNXPos;

        double tapestriesBTNWidth;
        double tapestriesBTNXPos;

        double toolsBTNWidth;
        double toolsBTNXPos;

        double settingsBTNWidth;
        double settingsBTNXPos;

        string selectedTab = "home";


        TreeViewItem lastParentItem = new TreeViewItem(); // Used later in a recursive method
        public MainWindow()
        {
            InitializeComponent();
            LoadTree(@"documents\Ruleset", true); //Loads the treeviewer using file folders
            LoadDocumentViewer(@"documents\Ruleset Welcome.xps"); //Loads the Welcome Document into the Document Viewer
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            homeBTNWidth = homeBTNContent.ActualWidth;
            rulesetBTNWidth = rulesetBTNContent.ActualWidth;
            tapestriesBTNWidth = tapestriesBTNContent.ActualWidth;
            toolsBTNWidth = toolsBTNContent.ActualWidth;
            settingsBTNWidth = settingsBTNContent.ActualWidth;

            homeBTNXPos = homeBTNContent.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)).X;
            rulesetBTNXPos = rulesetBTNContent.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)).X;
            tapestriesBTNXPos = tapestriesBTNContent.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)).X;
            toolsBTNXPos = toolsBTNContent.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)).X;
            settingsBTNXPos = settingsBTNContent.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)).X;

            indicator.Width = homeBTNWidth;
            indicator.Margin = new Thickness(homeBTNXPos, 0, 0, 0);
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            mechanicsViewer.FitToWidth(); // Ensures the document is always formatted correctly

            homeBTNWidth = homeBTNContent.ActualWidth;
            rulesetBTNWidth = rulesetBTNContent.ActualWidth;
            tapestriesBTNWidth = tapestriesBTNContent.ActualWidth;
            toolsBTNWidth = toolsBTNContent.ActualWidth;
            settingsBTNWidth = settingsBTNContent.ActualWidth;

            homeBTNXPos = homeBTNContent.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)).X;
            rulesetBTNXPos = rulesetBTNContent.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)).X;
            tapestriesBTNXPos = tapestriesBTNContent.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)).X;
            toolsBTNXPos = toolsBTNContent.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)).X;
            settingsBTNXPos = settingsBTNContent.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0)).X;

            if (selectedTab == "home")
            {
                moveIndicator(homeBTNXPos, homeBTNWidth, 0);
            } 
            else if (selectedTab == "ruleset")
            {
                moveIndicator(rulesetBTNXPos, rulesetBTNWidth, 0);
            } 
            else if (selectedTab == "tapestries")
            {
                moveIndicator(tapestriesBTNXPos, tapestriesBTNWidth, 0);
            } 
            else if (selectedTab == "tools")
            {
                moveIndicator(toolsBTNXPos, toolsBTNWidth, 0);
            } 
            else if (selectedTab == "settings")
            {
                moveIndicator(settingsBTNXPos, settingsBTNWidth, 0);
            }

            if (this.WindowState == WindowState.Maximized)
            {
                windowBTNS.Margin = new Thickness(0, 6, 6, 0);
                maximizeBTN.Style = (Style)Resources["maxBTN"];
            }
        }

        private void rulesetBTN_Click(object sender, RoutedEventArgs e)
        {
            mainContent.SelectedIndex = 1;
            selectedTab = "ruleset";
            moveIndicator(rulesetBTNXPos,rulesetBTNWidth, animationDuration);
        }

        private void tapestriesBTN_Click(object sender, RoutedEventArgs e)
        {
            mainContent.SelectedIndex = 2;
            selectedTab = "tapestries";
            moveIndicator(tapestriesBTNXPos, tapestriesBTNWidth, animationDuration);
        }

        private void toolsBTN_Click(object sender, RoutedEventArgs e)
        {
            mainContent.SelectedIndex = 3;
            selectedTab = "tools";
            moveIndicator(toolsBTNXPos, toolsBTNWidth, animationDuration);
        }

        private void settingsBTN_Click(object sender, RoutedEventArgs e)
        {
            mainContent.SelectedIndex = 4;
            selectedTab = "settings";
            moveIndicator(settingsBTNXPos, settingsBTNWidth, animationDuration);
        }

        private void homeBTN_Click(object sender, RoutedEventArgs e)
        {
            mainContent.SelectedIndex = 0;
            selectedTab = "home";
            moveIndicator(homeBTNXPos, homeBTNWidth, animationDuration);
        }

        private void moveIndicator(double position,double width, double duration)
        {
            var sb = new Storyboard();
            var ta = new ThicknessAnimation();
            var db = new DoubleAnimation();

            Storyboard.SetTargetName(ta, "indicator");
            Storyboard.SetTargetProperty(ta, new PropertyPath(MarginProperty));
            Storyboard.SetTargetName(db, "indicator");
            Storyboard.SetTargetProperty(db, new PropertyPath(WidthProperty));

            ta.To = new Thickness(position, 0, 0, 0);
            ta.Duration = new Duration(TimeSpan.FromSeconds(duration));

            db.To = width;
            db.Duration = new Duration(TimeSpan.FromSeconds(duration));

            sb.Children.Add(ta);
            sb.Children.Add(db);
            sb.Begin(this);
        }

        private void rulesetTree_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            mechanicsViewer.FitToWidth(); // Ensures the document is always formatted correctly
        }

        private void rulesetTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem? item = rulesetTree.SelectedItem as TreeViewItem;
            bool run = true;
            string path = item.Header.ToString() + ".xps";
            while (run)
            {
                TreeViewItem parent = item.Parent as TreeViewItem;
                try
                {
                    path = parent.Header.ToString() + "\\" + path;
                }
                catch
                {
                    run = false;
                }
                item = parent as TreeViewItem;
            }
            path = "documents\\Ruleset\\" + path;

            try
            {
                LoadDocumentViewer(path);
            }
            catch
            {

            }
        }
        private void LoadTree(string DirPath, bool firstRun) // Loads items into our tree viewer using the document directory
        {
            DirPath = System.IO.Path.GetFullPath(DirPath).Replace(@"bin\Debug\net8.0-windows\", ""); // Changes partial path into useable full path
            string[] folders = Directory.GetDirectories(DirPath); // Creates an array of all the folders in parent folder
            string[] files = Directory.GetFiles(DirPath); // Creates an array of all the files in parent folder

            foreach (string folder in folders) // Iterates over array of folders
            {
                string folderName = System.IO.Path.GetFileName(folder); // Variable that stores the name of the file, excluding path

                TreeViewItem parentItem = new TreeViewItem(); // Creates a new treeview item
                parentItem.Style = (Style)Resources["mechtreeitem"];
                parentItem.Header = folderName; // Sets the new treeview item's header to the name of the file
                parentItem.Foreground = Brushes.White;
                parentItem.FontFamily = new FontFamily("Ami R");
                parentItem.FontSize = 20;
                if (firstRun) // This runs only on the first iteration of method
                {
                    rulesetTree.Items.Add(parentItem); // Adds treeview item to the main treeview
                    rulesetTree.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending)); // Sorts items alphabetically
                }
                else // runs every other iteration of method
                {
                    lastParentItem.Items.Add(parentItem); // Adds treeview item to the previous treeview item, which is created at the end of the first iteration of method
                    lastParentItem.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending)); // Sorts items alphabetically
                }

                string[] subFiles = Directory.GetFiles(folder); // Array of files in subfolder

                foreach (string subFile in subFiles) // Iterates over files in subfolder
                {
                    string fileName = System.IO.Path.GetFileName(subFile).Replace(".xps", ""); // Changes partial path into useable full path

                    TreeViewItem childItem2 = new TreeViewItem(); // Creates new treeview item
                    childItem2.Header = fileName; // Sets the new treeview item's header to the name of the file
                    childItem2.Foreground = Brushes.LightGray;
                    childItem2.Style = (Style)Resources["mechtreeitem"];
                    parentItem.Items.Add(childItem2); // Adds treeview items as a child of folder treeview item
                    parentItem.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending)); // Sorts items alphabetically
                }
                if (firstRun) { lastParentItem = parentItem; } // Sets lastparent to this iterations parent item, used in the next iteration
                LoadTree(folder, false); // iterates the method to run on more subfolders

            }
            if (firstRun) // Only runs for the first iteration of method. (Only runs after all iterations finish)
            {
                foreach (string file in files) // Iterates over all documents in main folder
                {
                    string fileName = System.IO.Path.GetFileName(file).Replace(".xps", ""); // Changes partial path into useable full path

                    TreeViewItem childItem1 = new TreeViewItem(); // Creates new treeview item
                    childItem1.Header = fileName; // Sets new treeview items header to document name
                    childItem1.Foreground = Brushes.LightGray;
                    childItem1.Style = (Style)Resources["mechtreeitem"];
                    rulesetTree.Items.Add(childItem1); // Adds treeview item to main treeview list
                    rulesetTree.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending)); // Sorts list alphabetically
                }
            }


        }
        private void LoadDocumentViewer(string partialDir) // Method that loads the docviewer with the correct xps file
        {
            string fullDir = System.IO.Path.GetFullPath(partialDir).Replace(@"bin\Debug\net8.0-windows\", ""); // Changes the partial directory we passed in Mechanics() to a usable full path

            XpsDocument _xpsPackage = new XpsDocument(fullDir, FileAccess.Read, System.IO.Packaging.CompressionOption.NotCompressed); // Creates a new xpsdocument using the fullpath

            FixedDocumentSequence fixedDocumentSequence = _xpsPackage.GetFixedDocumentSequence(); // IDK what this does but program doesnt run without it

            mechanicsViewer.Document = fixedDocumentSequence as IDocumentPaginatorSource; // Sets the viewer (created in xaml) to view the document we just created
            mechanicsViewer.FitToWidth(); // Resizes the document in our viewer
            mechanicsViewer.ShowPageBorders = false; // Removes ugly dropshadow
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void closeBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void maximizeBTN_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                windowBTNS.Margin = new Thickness(0, 6, 6, 0);
                maximizeBTN.Style = (Style)Resources["maxBTN"];
            } 
            else if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                windowBTNS.Margin = new Thickness(0, 0, 0, 0);
                maximizeBTN.Style = (Style)Resources["normBTN"];
            }

           

        }

        private void minimizeBTN_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}