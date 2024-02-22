using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
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

namespace ECM_system_for_PNG.Pages
{
    /// <summary>
    /// Логика взаимодействия для Folder.xaml
    /// </summary>
    public partial class Folder : Page
    {
        // A property to store the list of folders in the current path
        public List<string> PathItems { get; set; }

        public Folder()
        {
            InitializeComponent();
            // Initialize the PathItems with the root folder
            PathItems = new List<string>() { "/" };
            // Set the data context of the window to this class
            this.DataContext = this;
            // Load the files from the root folder
            LoadFiles();
        }

        private void LoadFiles()
        {
            // SFTP server credentials
            string host = "80.91.18.120";
            int port = 535;
            string username = "sysadmin";
            string password = "v8nlbm!*_pv*";

            // Remote folder to view files
            // Get the remote folder from the PathItems list
            string remoteFolder = string.Join("/", PathItems);

            // Create a sftp client and connect to the server
            using (SftpClient sftp = new SftpClient(host, port, username, password))
            {
                try
                {
                    sftp.Connect();

                    // Get the list of files in the remote folder
                    var files = sftp.ListDirectory(remoteFolder);

                    // Clear the listbox
                    listBox.Items.Clear();

                    // Add the file names to the listbox
                    foreach (var file in files)
                    {
                        listBox.Items.Add(file.Name);
                    }

                    sftp.Disconnect();
                }
                catch (Exception ex)
                {
                    // Show the error message
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // Enable or disable the back button depending on the current folder
            // If the PathItems list has more than one element, it means we are not in the root folder
            // and we can go back to the parent folder
            if (PathItems.Count > 1)
            {
                buttonBack.IsEnabled = true;
            }
            else
            {
                buttonBack.IsEnabled = false;
            }
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            // Check if the PathItems list has more than one element
            if (PathItems.Count > 1)
            {
                // Remove the last element from the PathItems list
                PathItems.RemoveAt(PathItems.Count - 1);

                // Load the files from the parent folder
                LoadFiles();
            }
        }

        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // SFTP server credentials
            string host = "80.91.18.120";
            int port = 535;
            string username = "sysadmin";
            string password = "v8nlbm!*_pv*";

            // Get the selected item from the listbox
            string selectedItem = listBox.SelectedItem as string;

            // Create a sftp client and connect to the server
            using (SftpClient sftp = new SftpClient(host, port, username, password))
            {
                try
                {
                    sftp.Connect();

                    // Get the current remote folder from the PathItems list
                    string remoteFolder = string.Join("/", PathItems);

                    // Get the full path of the selected item on the server
                    string itemPath = remoteFolder + "/" + selectedItem;

                    // Get the SftpFile object for the selected item
                    SftpFile item = (SftpFile)sftp.Get(itemPath);

                    // Check if the item is a directory
                    if (item.IsDirectory)
                    {
                        // Add the item name to the PathItems list
                        PathItems.Add(selectedItem);

                        // Load the files from the new folder
                        LoadFiles();
                    }

                    sftp.Disconnect();
                }
                catch (Exception ex)
                {
                    // Show the error message
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
