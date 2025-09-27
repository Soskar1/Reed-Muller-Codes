using CodingTheory.Channels;
using CodingTheory.ReedMuller;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Vector = CodingTheory.Math.Vector;

namespace CodingTheory.Presentation;

public partial class MainWindow : Window
{
    private Vector m_currentEncodedVector;
    private Vector m_currentDistortedVector;
    private int m_currentM;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Encode_Click(object sender, RoutedEventArgs e)
    {
        if (!Int32.TryParse(mParameter.Text, out m_currentM)) {
            inputVectorErrorLabel.Content = "m parameter must be int";
            e.Handled = true;
            return;
        }
        
        ReedMullerEncoder encoder = new ReedMullerEncoder((byte)m_currentM);
        int inputVectorLength = inputVector.Text.Length;

        if (inputVectorLength != encoder.RequiredMessageLength)
        {
            inputVectorErrorLabel.Content = "Vector length must be: m + 1";
            e.Handled = true;
            return;
        }

        if (!Vector.TryParse(inputVector.Text, out Vector vectorToEncode))
        {
            inputVectorErrorLabel.Content = "Invalid vector";
            e.Handled = true;
            return;
        }

        m_currentEncodedVector = encoder.Encode(vectorToEncode);

        encodingResult.Text = m_currentEncodedVector.ToString();
        inputVectorErrorLabel.Content = "";
        channelButton.IsEnabled = true;
    }

    private void channelButton_Click(object sender, RoutedEventArgs e)
    {
        if (!float.TryParse(probabilityParameter.Text, out float probability))
        {
            // TODO: error message
            e.Handled = true;
            return;
        }

        Channel channel = new Channel(probability);
        m_currentDistortedVector = channel.PassThrough(m_currentEncodedVector);

        distortedVectorTextBox.Text = m_currentDistortedVector.ToString();
        distortedVectorTextBox.IsReadOnly = false;

        decodeButton.IsEnabled = true;
    }

    private void DistortedVectorTextChanged(object sender, TextChangedEventArgs e)
    {
        if (!Vector.TryParse(distortedVectorTextBox.Text, out Vector v))
        {
            // TODO: error message
            return;
        }

        m_currentDistortedVector = v;

        List<int> errors = new List<int>();
        for (int i = 0; i < m_currentEncodedVector.Length; ++i)
            if (m_currentDistortedVector[i] != m_currentEncodedVector[i])
                errors.Add(i);

        TextBlock tb = new TextBlock();
        tb.Inlines.Add(new Run($"{errors.Count} errors: "));

        for (int i = 0; i < m_currentDistortedVector.Length; ++i)
        {
            if (errors.Contains(i))
                tb.Inlines.Add(new Run(m_currentDistortedVector[i].ToString()) { Foreground = Brushes.Red });
            else
                tb.Inlines.Add(new Run(m_currentDistortedVector[i].ToString()));
        }

        nErrorsLabel.Content = tb;
    }

    private void decodeButton_Click(object sender, RoutedEventArgs e)
    {
        ReedMullerDecoder decoder = new ReedMullerDecoder(m_currentM);
        Vector v = decoder.Decode(m_currentDistortedVector);
        resultVectorTextBox.Text = v.ToString();
    }
}