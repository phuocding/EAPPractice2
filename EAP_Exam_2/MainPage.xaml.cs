using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Streams;
using Microsoft.WindowsAzure.Storage.Blob;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.WindowsAzure.Storage;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EAPPractice2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private static readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=eapuwppractice2;AccountKey=UOJR42pNwp1ZNMG6wONrgcsXQmCXdlJvBcbiqIO3VTf8hM4SHYbUDP/82UZfbMV57bolVerN3+Tf/EjIFveDig==;EndpointSuffix=core.windows.net";


        private async void Choose_fileAsync(object sender, RoutedEventArgs e)
        {
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.FileTypeFilter.Clear();
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".png");
            StorageFile file = await filePicker.PickSingleFileAsync();
            if (file != null)
            {
                UploadImg(file);
            }
        }

        private async void UploadImg(StorageFile file)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            CloudStorageAccount storageAccount;
            storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("imgcontainer");
            await container.CreateIfNotExistsAsync();
            CloudBlockBlob blob = container.GetBlockBlobReference(file.Name);
            await blob.UploadFromFileAsync(file);
            MessageDialog alert = new MessageDialog("Sucessfully uploaded!");
            await alert.ShowAsync();
            var filestream = await file.OpenAsync(FileAccessMode.Read);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(filestream);
            UploadedImg.Source = bitmapImage;
        }
    }
}
