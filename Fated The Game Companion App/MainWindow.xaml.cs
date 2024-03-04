using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Holds all the variables that describe a character
    /// </summary>
    public class CharacterSheet
    {
        public string Name;
        public string Species;
        public string Profession;
        public string Level;
    }
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

        string charactersPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Fated\\Characters";

        CharacterSheet curSelectedCharacter = new CharacterSheet();
        string curCharacterPath;





        TreeViewItem lastParentItem = new TreeViewItem(); // Used later in a recursive method
        public MainWindow()
        {
            InitializeComponent();
            LoadTree(@"documents\Ruleset", true); //Loads the treeviewer using file folders
            LoadDocumentViewer(@"documents\Ruleset Welcome.xps"); //Loads the Welcome Document into the Document Viewer

            Directory.CreateDirectory(charactersPath);
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
            playerCharacterList.Children.Clear();
            
            LoadPlayerCharacterList();

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
                parentItem.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./fonts/#Ami R");
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

        private void LoadPlayerCharacterList() // Method used for loading the list of characters found on the tapestries page
        {
            string[] characters = Directory.GetFiles(charactersPath); // Gets a list of files from the directory all character save files are stored
            
            foreach (string character in characters) // Iterates over list of files
            {
                CharacterSheet cs = Deserialize(character);

                Border charInfoBorder = new Border();
                charInfoBorder.BorderThickness = new Thickness(2, 2, 2, 2);
                charInfoBorder.BorderBrush = Brushes.White;
                charInfoBorder.CornerRadius = new CornerRadius(6);
                charInfoBorder.Margin = new Thickness(15,10,1,1);

                Grid charInfoHorPanel = new Grid();
                ColumnDefinition c1 = new ColumnDefinition();
                ColumnDefinition c2 = new ColumnDefinition();
                charInfoHorPanel.ColumnDefinitions.Add(c1);
                charInfoHorPanel.ColumnDefinitions.Add(c2);

                StackPanel charInfoPanel = new StackPanel();
                StackPanel charInfoVerBTNPanel = new StackPanel();
                charInfoVerBTNPanel.VerticalAlignment = VerticalAlignment.Center;

                Grid.SetColumn(charInfoPanel, 0);
                Grid.SetColumn(charInfoVerBTNPanel, 1);
                charInfoHorPanel.Children.Add(charInfoPanel);
                charInfoHorPanel.Children.Add(charInfoVerBTNPanel);

                Label charName = new Label(); // Creates a new label
                charName.Content = cs.Name; // Sets the content of the label to be the name of the file without extension
                charName.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./fonts/#Ami R");
                charName.FontSize = 32;
                charName.FontWeight = FontWeights.Bold;
                charName.Foreground = Brushes.White;
                charName.Padding = new Thickness(6,0,0,0);
                charName.VerticalAlignment = VerticalAlignment.Center;
                charInfoPanel.Children.Add(charName);

                Label charLevel = new Label();
                charLevel.Content = "Level " + cs.Level;
                charLevel.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./fonts/#Ami R");
                charLevel.FontSize = 18;
                charLevel.FontWeight = FontWeights.Bold;
                charLevel.Foreground = Brushes.White;
                charLevel.VerticalAlignment = VerticalAlignment.Center;
                charLevel.Padding = new Thickness(6, 0, 0, 0);
                charInfoPanel.Children.Add(charLevel);

                Label charSAndP = new Label();
                charSAndP.Content = cs.Species+" " + cs.Profession;
                charSAndP.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./fonts/#Ami R");
                charSAndP.FontSize = 18;
                charSAndP.FontWeight = FontWeights.Bold;
                charSAndP.Foreground = Brushes.White;
                charSAndP.VerticalAlignment = VerticalAlignment.Center;
                charSAndP.Padding = new Thickness(6, 0, 0, 6);
                charInfoPanel.Children.Add(charSAndP);

                Button loadBTN = new Button();
                loadBTN.Name = character.Replace(charactersPath, "").Replace(".fcs", "").Replace("\\","");
                loadBTN.Content = "Load";
                loadBTN.HorizontalAlignment = HorizontalAlignment.Right;
                loadBTN.Width = 40;
                loadBTN.Margin = new Thickness(0, 1, 3, 1);
                loadBTN.Style = (Style)Resources["charInfoBTNS"];
                loadBTN.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./fonts/#Ami R");
                loadBTN.Foreground = Brushes.White;
                loadBTN.FontSize = 16;
                charInfoVerBTNPanel.Children.Add(loadBTN);

                Button editBTN = new Button();
                editBTN.Name = character.Replace(charactersPath, "").Replace("\\", "").Replace(".fcs","");
                editBTN.Content = "Edit";
                editBTN.HorizontalAlignment = HorizontalAlignment.Right;
                editBTN.Width = 40;
                editBTN.Margin = new Thickness(0, 1, 3, 1);
                editBTN.Style = (Style)Resources["charInfoBTNS"];
                editBTN.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./fonts/#Ami R");
                editBTN.Foreground = Brushes.White;
                editBTN.FontSize = 16;
                editBTN.Click += EditBTN_Click;
                charInfoVerBTNPanel.Children.Add(editBTN);

                Button copyBTN = new Button();
                copyBTN.Name = character.Replace(charactersPath, "").Replace(".fcs", "").Replace("\\", "") + "Copy";
                copyBTN.Content = "Copy";
                copyBTN.HorizontalAlignment = HorizontalAlignment.Right;
                copyBTN.Width = 40;
                copyBTN.Margin = new Thickness(0, 1, 3, 1);
                copyBTN.Style = (Style)Resources["charInfoBTNS"];
                copyBTN.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./fonts/#Ami R");
                copyBTN.Foreground = Brushes.White;
                copyBTN.FontSize = 16;
                charInfoVerBTNPanel.Children.Add(copyBTN);

                Button exportBTN = new Button();
                exportBTN.Name = character.Replace(charactersPath, "").Replace(".fcs", "").Replace("\\", "") + "Export";
                exportBTN.Content = "Export";
                exportBTN.HorizontalAlignment = HorizontalAlignment.Right;
                exportBTN.Width = 40;
                exportBTN.Margin = new Thickness(0, 1, 3, 1);
                exportBTN.Style = (Style)Resources["charInfoBTNS"];
                exportBTN.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./fonts/#Ami R");
                exportBTN.Foreground = Brushes.White;
                exportBTN.FontSize = 16;
                charInfoVerBTNPanel.Children.Add(exportBTN);

                charInfoBorder.Child = charInfoHorPanel;
                playerCharacterList.Children.Add(charInfoBorder); // Adds label to the stackpanel of characters
            }
        }

        private void EditBTN_Click(object sender, RoutedEventArgs e)
        {
            string saveFile = (sender as Button).Name.ToString();
            string saveFilePath = charactersPath + "\\" + saveFile + ".fcs";

            curSelectedCharacter = Deserialize(saveFilePath);

            mainContent.SelectedIndex = 5;
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

        private void newCharBTN_Click(object sender, RoutedEventArgs e)
        {
            mainContent.SelectedIndex = 5;

            string filePath = GetNewFilePath();

            curSelectedCharacter.Name = "Unnamed Character";
            curSelectedCharacter.Level = "0";
            curSelectedCharacter.Species = "Undecided Species";
            curSelectedCharacter.Profession = "Undecided Profession";

            Serialize(filePath, curSelectedCharacter);
            curCharacterPath = filePath;


        }

        private void Serialize(string path, CharacterSheet characterSheet)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(CharacterSheet));
                x.Serialize(writer, characterSheet);
            }
        }

        private CharacterSheet Deserialize(string path)
        {
            CharacterSheet cs;

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(CharacterSheet));


                cs = (CharacterSheet)x.Deserialize(fs);

                curCharacterPath = path;
            }

            return cs;

        }

        private string GetNewFilePath()
        {
            List<int> fileNumbers = new List<int>();
            fileNumbers.Add(0);

            string[] characters = Directory.GetFiles(charactersPath);

            foreach (string character in characters)
            {
                fileNumbers.Add(Convert.ToInt32(character.Replace(".fcs", "").Replace("Save","").Replace(charactersPath,"").Replace("\\","")));
            }

            int max = fileNumbers.Max();

            string newFileName = "Save" + Convert.ToString(max+1) + ".fcs";

            string newFilePath = charactersPath + "\\" + newFileName;

            return newFilePath;
        }

        private void nameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            curSelectedCharacter.Name = nameInput.Text;
            Serialize(curCharacterPath, curSelectedCharacter);
        }
    }
}