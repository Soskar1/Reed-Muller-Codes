using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using CodingTheory.Channels;
using System.Diagnostics;

namespace CodingTheory.Presentation;

public partial class MainWindow : Window
{
    private Channel m_channel;
    private string m_selectedFile;
    private const ushort m_bmpFileType = 0x4D42;
    private const ushort m_bmpBitsPerPixel = 24;
    private const int m_fileHeaderSize = 14;
    private const int m_infoHeaderSize = 40;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void SelectImageClick(object sender, RoutedEventArgs e)
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        fileDialog.Filter = "bmp24 Files | *.bmp";
        
        bool? success = fileDialog.ShowDialog();
        if (success == true)
        {
            fileNameLabel.Content = fileDialog.SafeFileName;
            m_selectedFile = fileDialog.FileName;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fileDialog.FileName);
            bitmap.EndInit();

            imageInput.Source = bitmap;
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        m_channel = new Channel(float.Parse(probabilityInput.Text));

        using (FileStream fs = new FileStream(m_selectedFile, FileMode.Open, FileAccess.Read))
        using (BinaryReader reader = new BinaryReader(fs))
        {
            byte[] fileHeader = reader.ReadBytes(m_fileHeaderSize);
            byte[] infoHeader = reader.ReadBytes(m_infoHeaderSize);

            byte[] GetTypeBytes() => [fileHeader[0], fileHeader[1]];
            byte[] GetBitsPerPixelBytes() => [infoHeader[14], infoHeader[15]];
            byte[] GetOffsetBytes() => [fileHeader[10], fileHeader[11], fileHeader[12], fileHeader[13]];

            ushort type = BitConverter.ToUInt16(GetTypeBytes(), 0);  
            if (type != m_bmpFileType)
            {
                errorDisplayLabel.Content = "Not a BMP file";
                return;
            }

            ushort bitCount = BitConverter.ToUInt16(GetBitsPerPixelBytes(), 0);
            if (bitCount != m_bmpBitsPerPixel)
            {
                errorDisplayLabel.Content = "Not a 24-bit BMP";
                return;
            }

            uint offset = BitConverter.ToUInt32(GetOffsetBytes(), 0);
            byte[] additionalHeaderData = reader.ReadBytes((int)offset - m_fileHeaderSize - m_infoHeaderSize);

            fs.Seek(offset, SeekOrigin.Begin);
            byte[] pixels = reader.ReadBytes((int)(fs.Length - offset));

            pixels = m_channel.PassThrough(pixels);
            byte[] bmpFileData = fileHeader.Concat(infoHeader).Concat(additionalHeaderData).Concat(pixels).ToArray();

            MemoryStream memoryStream = new MemoryStream(bmpFileData);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = memoryStream;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            withoutAlgorithmImage.Source = bitmap;
        }
    }
}