using CodingTheory.Channels;
using CodingTheory.ReedMuller;
using System.Globalization;
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

    /// <summary>
    /// Handles the click event for the <c>Encode</c> button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments associated with the button click.</param>
    /// <remarks>
    /// This method:
    /// <list type="number">
    ///   <item>Validates the input parameter <c>m</c> and the message vector.</item>
    ///   <item>Creates a <see cref="ReedMullerEncoder"/> for <c>RM(1, m)</c>.</item>
    ///   <item>Encodes the user-provided input vector.</item>
    ///   <item>Displays the encoded result and enables the channel simulation button.</item>
    /// </list>
    /// </remarks>
    private void Encode_Click(object sender, RoutedEventArgs e)
    {
        // m validation
        if (!Int32.TryParse(mParameter.Text, out m_currentM)) {
            inputVectorErrorLabel.Content = "m parameter must be int";
            e.Handled = true;
            return;
        }

        if (m_currentM < 2)
        {
            inputVectorErrorLabel.Content = "Error: M < 2";
            e.Handled = true;
            return;
        }

        ReedMullerEncoder encoder = new ReedMullerEncoder((byte)m_currentM);
        int inputVectorLength = inputVector.Text.Length;

        // input vector length validation
        if (inputVectorLength != encoder.RequiredMessageLength)
        {
            inputVectorErrorLabel.Content = "Vector length must be: m + 1";
            e.Handled = true;
            return;
        }

        // input vector parsing
        if (!Vector.TryParse(inputVector.Text, out Vector vectorToEncode))
        {
            inputVectorErrorLabel.Content = "Invalid vector";
            e.Handled = true;
            return;
        }

        // encoding
        m_currentEncodedVector = encoder.Encode(vectorToEncode);

        // display result
        encodingResult.Text = m_currentEncodedVector.ToString();
        inputVectorErrorLabel.Content = "";
        channelButton.IsEnabled = true;
    }

    /// <summary>
    /// Handles the click event for the <c>Channel</c> button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments associated with the button click.</param>
    /// <remarks>
    /// This method simulates the transmission of the encoded vector through a binary channel
    /// with a user-specified bit-flip probability.  
    /// It uses the <see cref="Channel"/> class to introduce random bit distortions,
    /// displays the distorted vector, and enables the decoding step.
    /// </remarks>
    private void channelButton_Click(object sender, RoutedEventArgs e)
    {
        // probability validation
        if (!TryParseFloat(probabilityParameter.Text, out float probability) || probability < 0 || probability > 1)
        {
            MessageBox.Show("Please enter a valid probability between 0 and 1.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        // Distortion through channel
        Channel channel = new Channel(probability);
        m_currentDistortedVector = channel.PassThrough(m_currentEncodedVector);

        // Displaying distorted vector
        distortedVectorTextBox.Text = m_currentDistortedVector.ToString();
        distortedVectorTextBox.IsReadOnly = false;

        // Enabling decoding step
        decodeButton.IsEnabled = true;
    }

    /// <summary>
    /// Handles text change events in the distorted vector input field.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The text change event data.</param>
    /// <remarks>
    /// This method dynamically compares the user-modified distorted vector against the
    /// originally encoded vector and visually highlights bit errors.  
    /// <para>
    /// Incorrect bits are displayed in red within the <c>nErrorsLabel</c> field,
    /// and the total number of detected bit errors is shown.
    /// </para>
    /// </remarks>
    private void DistortedVectorTextChanged(object sender, TextChangedEventArgs e)
    {
        // Parsing distorted vector
        if (!Vector.TryParse(distortedVectorTextBox.Text, out Vector v))
        {
            return;
        }

        if (v.Length != m_currentEncodedVector.Length)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Foreground = Brushes.Red;
            textBlock.Text = "Distorted vector length mismatch";
            nErrorsLabel.Content = textBlock;
            return;
        }

        m_currentDistortedVector = v;

        // Comparing with encoded vector and highlighting errors
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

    /// <summary>
    /// Handles the click event for the <c>Decode</c> button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event arguments associated with the button click.</param>
    /// <remarks>
    /// This method performs decoding of the (possibly distorted) vector
    /// using the <see cref="ReedMullerDecoder"/>.  
    /// The decoded vector is displayed in the output field for user comparison.
    /// </remarks>
    private void decodeButton_Click(object sender, RoutedEventArgs e)
    {
        ReedMullerDecoder decoder = new ReedMullerDecoder(m_currentM);
        Vector v = decoder.Decode(m_currentDistortedVector);
        resultVectorTextBox.Text = v.ToString();
    }

    private bool TryParseFloat(string input, out float result)
    {
        if (float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            return true;

        if (float.TryParse(input, NumberStyles.Float, new CultureInfo("fr-FR"), out result))
            return true;

        return false;
    }
}