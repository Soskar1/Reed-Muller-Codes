using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using CodingTheory.Channels;
using CodingTheory.ReedMuller;
using Vector = CodingTheory.Math.Vector;
using System.Windows.Controls;

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

    /// <summary>
    /// Handles the event for selecting an input image file.
    /// </summary>
    /// <param name="sender">The control that triggered the event.</param>
    /// <param name="e">The event data associated with the action.</param>
    /// <remarks>
    /// Opens a standard file selection dialog, allowing the user to choose a
    /// <c>.bmp</c> image file.  
    /// Once selected, the image is loaded and displayed in the input preview area.
    /// </remarks>
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

    /// <summary>
    /// Handles the click event that initiates the channel simulation process.
    /// </summary>
    /// <param name="sender">The source control that triggered the event.</param>
    /// <param name="e">The associated event data.</param>
    /// <remarks>
    /// This method reads the selected BMP file, validates its format, and
    /// simulates transmission both with and without Reed–Muller encoding.
    /// The resulting images are displayed side-by-side for comparison.
    /// <para>
    /// The BMP file headers and pixel data are parsed manually to ensure compatibility
    /// with 24-bit uncompressed BMP images.
    /// </para>
    /// </remarks>
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

            // BMP format validation
            ushort type = BitConverter.ToUInt16(GetTypeBytes(), 0);  
            if (type != m_bmpFileType)
            {
                errorDisplayLabel.Content = "Not a BMP file";
                return;
            }

            // 24-bit BMP validation
            ushort bitCount = BitConverter.ToUInt16(GetBitsPerPixelBytes(), 0);
            if (bitCount != m_bmpBitsPerPixel)
            {
                errorDisplayLabel.Content = "Not a 24-bit BMP";
                return;
            }

            // Reading pixel data
            uint offset = BitConverter.ToUInt32(GetOffsetBytes(), 0);
            byte[] additionalHeaderData = reader.ReadBytes((int)offset - m_fileHeaderSize - m_infoHeaderSize);

            fs.Seek(offset, SeekOrigin.Begin);
            byte[] pixels = reader.ReadBytes((int)(fs.Length - offset));

            PassThroughChannelWithoutAlgorithm(fileHeader, infoHeader, additionalHeaderData, pixels);
            PassThroughChannelUsingReedMullerCodes(fileHeader, infoHeader, additionalHeaderData, pixels);
        }
    }

    /// <summary>
    /// Simulates image transmission through the noisy channel without any error correction.
    /// </summary>
    /// <param name="fileHeader">The BMP file header (14 bytes).</param>
    /// <param name="infoHeader">The BMP info header (40 bytes).</param>
    /// <param name="additionalHeaderData">Any additional header bytes between the info header and pixel data.</param>
    /// <param name="pixels">The raw pixel byte array of the image.</param>
    /// <remarks>
    /// The pixel data is passed directly through the <see cref="Channel"/>,
    /// which introduces random bit flips based on the specified probability.
    /// The distorted image is reconstructed and displayed.
    /// </remarks>
    private void PassThroughChannelWithoutAlgorithm(byte[] fileHeader, byte[] infoHeader, byte[] additionalHeaderData, byte[] pixels)
    {
        pixels = m_channel.PassThrough(pixels);
        Display(fileHeader, infoHeader, additionalHeaderData, pixels, withoutAlgorithmImage);
    }

    /// <summary>
    /// Simulates image transmission through the noisy channel using Reed–Muller encoding and decoding.
    /// </summary>
    /// <param name="fileHeader">The BMP file header (14 bytes).</param>
    /// <param name="infoHeader">The BMP info header (40 bytes).</param>
    /// <param name="additionalHeaderData">Any additional header bytes between the info header and pixel data.</param>
    /// <param name="pixels">The raw pixel byte array of the image.</param>
    /// <remarks>
    /// The pixel data is first encoded using the <see cref="ReedMullerEncoder"/>,
    /// transmitted through the <see cref="Channel"/> where random bit flips occur,
    /// and then decoded using <see cref="ReedMullerDecoder"/>.  
    /// The reconstructed pixel data is displayed, allowing comparison with
    /// the unencoded version to observe error correction performance.
    /// </remarks>
    private void PassThroughChannelUsingReedMullerCodes(byte[] fileHeader, byte[] infoHeader, byte[] additionalHeaderData, byte[] pixels)
    {
        ReedMullerEncoder encoder = new ReedMullerEncoder(byte.Parse(mInput.Text));
        List<Vector> encodedMsg = encoder.Encode(pixels);

        for (int i = 2; i < encodedMsg.Count; ++i)
            encodedMsg[i] = m_channel.PassThrough(encodedMsg[i]);

        ReedMullerDecoder decoder = new ReedMullerDecoder();
        byte[] decodedPixels = decoder.Decode(encodedMsg.ToArray());

        Display(fileHeader, infoHeader, additionalHeaderData, decodedPixels, withAlgorithmImage);
    }

    /// <summary>
    /// Displays a BMP image from header and pixel data in the specified <see cref="Image"/> control.
    /// </summary>
    /// <param name="fileHeader">The BMP file header (14 bytes).</param>
    /// <param name="infoHeader">The BMP info header (40 bytes).</param>
    /// <param name="additionalHeaderData">Additional BMP metadata bytes.</param>
    /// <param name="pixels">The raw pixel array of the image to display.</param>
    /// <param name="image">The target WPF <see cref="Image"/> control.</param>
    /// <remarks>
    /// Combines the BMP file components (headers and pixel data) into a single in-memory stream,
    /// loads it into a <see cref="BitmapImage"/>, and displays the resulting bitmap
    /// in the specified image container.
    /// </remarks>
    private void Display(byte[] fileHeader, byte[] infoHeader, byte[] additionalHeaderData, byte[] pixels, Image image)
    {
        byte[] bmpFileData = fileHeader.Concat(infoHeader).Concat(additionalHeaderData).Concat(pixels).ToArray();

        MemoryStream memoryStream = new MemoryStream(bmpFileData);
        BitmapImage bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.StreamSource = memoryStream;
        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        bitmap.EndInit();
        image.Source = bitmap;
    }
}