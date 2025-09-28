using System.Windows;
using Vector = CodingTheory.Math.Vector;
using Channel = CodingTheory.Channels.Channel;
using System.Text;
using CodingTheory.ReedMuller;

namespace CodingTheory.Presentation;

public partial class MainWindow : Window
{
    private Channel m_channel;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void showResultButton_Click(object sender, RoutedEventArgs e)
    {
        m_channel = new Channel(float.Parse(probabilityInput.Text));
        PassRawTextThroughChannel(textInput.Text);
        EncodeText(textInput.Text);
    }

    private void PassRawTextThroughChannel(string text)
    {
        List<byte> distortedRawText = new List<byte>();
        foreach (char c in text)
        {
            Vector v = Vector.ByteToVector((byte)c);
            v = m_channel.PassThrough(v);
            distortedRawText.Add((byte)v);
        }
        withoutAlgorithmTextBox.Text = Encoding.UTF8.GetString(distortedRawText.ToArray());
    }

    private void EncodeText(string text)
    {
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