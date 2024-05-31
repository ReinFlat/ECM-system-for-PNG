using Microsoft.Win32;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class Folder : Page
    {
        public List<string> PathItems { get; set; }
        public string PathDisplay => string.Join("/", PathItems);
        public Folder()
        {
            InitializeComponent();
            PathItems = new List<string>() { "/", "media", "sysadmin", "work" };
            this.DataContext = this;
            LoadFiles();
        }

        private void searchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string searchTerm = searchBox.Text.ToLower();
                var filteredFiles = (listBox.ItemsSource as List<FileItem>).Where(fileItem => fileItem.Name.ToLower().Contains(searchTerm)).ToList();

                listBox.ItemsSource = filteredFiles;
            }
        }

        public class FileItem
        {
            public string Name { get; set; }
            public bool IsDirectory { get; set; }
        }

        private void LoadFiles()
        {
            string host = "80.91.18.120";
            int port = 535;
            string username = "sysadmin";
            string password = "v8nlbm!*_pv*";

            string remoteFolder = string.Join("/", PathItems);

            using (SftpClient sftp = new SftpClient(host, port, username, password))
            {
                try
                {
                    sftp.Connect();

                    var files = sftp.ListDirectory(remoteFolder);

                    var fileItems = files.Where(file => file.Name != "." && file.Name != "..")
                        .Select(file => new FileItem
                        {
                            Name = file.Name,
                            IsDirectory = file.IsDirectory
                        }).ToList();

                    // Удалена строка listBox.Items.Clear();
                    listBox.ItemsSource = fileItems;

                    sftp.Disconnect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (PathItems.Count > 1)
            {
                buttonBack.IsEnabled = true;
            }
            else
            {
                buttonBack.IsEnabled = false;
            }

            textBoxPath.Text = PathDisplay;
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            if (PathItems.Count > 1)
            {
                PathItems.RemoveAt(PathItems.Count - 1);
                LoadFiles();
            }
        }

        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Проверяем, что выбранный элемент является FileItem и это директория
            if (listBox.SelectedItem is FileItem selectedItem && selectedItem.IsDirectory)
            {
                // Добавляем выбранный элемент к пути
                PathItems.Add(selectedItem.Name);
                // Обновляем содержимое ListBox с новым путем
                LoadFiles();
            }
        }

        private void DownloadFile_Click(object sender, RoutedEventArgs e)
        {
            string host = "80.91.18.120";
            int port = 535;
            string username = "sysadmin";
            string password = "v8nlbm!*_pv*";
            var button = sender as Button;
            if (button != null)
            {
                var fileItem = button.DataContext as FileItem;
                if (fileItem != null && !fileItem.IsDirectory)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.FileName = fileItem.Name;
                    saveFileDialog.DefaultExt = System.IO.Path.GetExtension(fileItem.Name);
                    saveFileDialog.Filter = "All files (*.*)|*.*";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string localPath = saveFileDialog.FileName;
                        string remotePath = string.Join("/", PathItems) + "/" + fileItem.Name; // Добавлен полный путь к файлу
                        using (SftpClient sftp = new SftpClient(host, port, username, password))
                        {
                            try
                            {
                                sftp.Connect();
                                using (var fileStream = File.Create(localPath))
                                {
                                    sftp.DownloadFile(remotePath, fileStream); // Используем полный путь
                                }
                                sftp.Disconnect();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Ошибка скачивания", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }
    }
}