using CodingTheory.ReedMuller;
using System.Globalization;
using System.Text;
using System.Windows;
using Channel = CodingTheory.Channels.Channel;
using Vector = CodingTheory.Math.Vector;

namespace CodingTheory.Presentation;

public partial class MainWindow : Window
{
    private Channel m_channel;

    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Handles the click event for the <c>Show Result</c> button.
    /// </summary>
    /// <param name="sender">The control that triggered the event.</param>
    /// <param name="e">The event arguments associated with the button click.</param>
    /// <remarks>
    /// When the user clicks the button:
    /// <list type="number">
    ///   <item>A <see cref="Channel"/> is initialized with the specified bit-flip probability.</item>
    ///   <item>The input text is transmitted directly through the noisy channel.</item>
    ///   <item>The same text is encoded using a <see cref="ReedMullerEncoder"/>,
    ///         passed through the same channel, decoded via <see cref="ReedMullerDecoder"/>,
    ///         and displayed for comparison.</item>
    /// </list>
    /// This side-by-side comparison illustrates the error resilience provided by
    /// Reed–Muller codes.
    /// </remarks>
    private void showResultButton_Click(object sender, RoutedEventArgs e)
    {
        if (!TryParseFloat(probabilityInput.Text, out float probability) || probability < 0 || probability > 1)
        {
            MessageBox.Show("Please enter a valid probability between 0 and 1.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        m_channel = new Channel(probability);
        PassRawTextThroughChannel(textInput.Text);
        EncodeText(textInput.Text);
    }

    private bool TryParseFloat(string input, out float result)
    {
        if (float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            return true;

        if (float.TryParse(input, NumberStyles.Float, new CultureInfo("fr-FR"), out result))
            return true;

        return false;
    }

    /// <summary>
    /// Simulates transmission of the input text through a noisy channel
    /// without any error-correction encoding.
    /// </summary>
    /// <param name="text">The input text message to transmit.</param>
    /// <remarks>
    /// The method converts the input string to bytes using UTF-8 encoding,
    /// sends it through the <see cref="Channel"/>, and reconstructs the resulting
    /// (possibly corrupted) text for display.  
    /// <para>
    /// This demonstrates the direct effects of random bit errors on raw text data.
    /// </para>
    /// </remarks>
    private void PassRawTextThroughChannel(string text)
    {
        byte[] distortedRawText = m_channel.PassThrough(Encoding.UTF8.GetBytes(text));
        withoutAlgorithmTextBox.Text = Encoding.UTF8.GetString(distortedRawText);
    }

    /// <summary>
    /// Encodes, transmits, and decodes the input text using Reed–Muller coding.
    /// </summary>
    /// <param name="text">The input text message to process.</param>
    /// <remarks>
    /// This method demonstrates the complete Reed–Muller encoding and decoding process:
    /// <list type="number">
    ///   <item>Converts the input text to bytes.</item>
    ///   <item>Encodes the bytes using <see cref="ReedMullerEncoder"/> with the specified parameter <c>m</c>.</item>
    ///   <item>Passes each encoded vector through the noisy <see cref="Channel"/>.</item>
    ///   <item>Decodes the distorted codewords using <see cref="ReedMullerDecoder"/>.</item>
    ///   <item>Displays the reconstructed message, showing the error-corrected output.</item>
    /// </list>
    /// <para>
    /// This simulation highlights the difference between unprotected and
    /// Reed–Muller-protected transmission under identical noise conditions.
    /// </para>
    /// </remarks>
    private void EncodeText(string text)
    {
        if (byte.TryParse(mInput.Text, out byte mValue))
        {
            if (mValue < 2)
            {
                MessageBox.Show("Parameter m must be at least 2 for Reed–Muller coding.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        else
        {
            MessageBox.Show("Please enter a valid integer for parameter m.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        ReedMullerEncoder encoder = new ReedMullerEncoder(byte.Parse(mInput.Text));
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        List<Vector> encodedMessage = encoder.Encode(bytes);

        for (int i = 2; i < encodedMessage.Count; ++i)
            encodedMessage[i] = m_channel.PassThrough(encodedMessage[i]);

        ReedMullerDecoder decoder = new ReedMullerDecoder();
        bytes = decoder.Decode(encodedMessage.ToArray());

        withAlgorithmTextBox.Text = Encoding.UTF8.GetString(bytes);
    }
}