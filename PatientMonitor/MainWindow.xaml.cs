using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Controls.DataVisualization.Charting.Compatible;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace PatientMonitor
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<KeyValuePair<int, double>> dataPoints;
        private DispatcherTimer timer;
        private int index = 0;
        Patient patient;

        string lastPatientName = "";
        int lastPatientAge;
        DateTime dateTime;
        double lastFrequency;
        int lastHarmonics, lastlastHarmonics;
        double ampValue;
        bool lastPatient = false;

        MonitorConstants.Parameter parameter;

        MonitorConstants.Parameter[] allParameter = { 
            MonitorConstants.Parameter.ECG, 
            MonitorConstants.Parameter.EMG, 
            MonitorConstants.Parameter.EEG, 
            MonitorConstants.Parameter.Resp 
        };

        MonitorConstants.Parameter[] savedParameters = {
            MonitorConstants.Parameter.ECG,
            MonitorConstants.Parameter.EMG,
            MonitorConstants.Parameter.EEG,
            MonitorConstants.Parameter.Resp
        };


        public MainWindow()
        {
            InitializeComponent();
            dataPoints = new ObservableCollection<KeyValuePair<int, double>>();
            lineSeriesECG.ItemsSource = dataPoints; // Bind the series to the data points
            
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1); // Set timer to tick every second
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Generate a new data point
            
            if (patient != null) dataPoints.Add(new KeyValuePair<int, double>(index++, patient.NextSample(index, parameter)));
            
            // Optional: Remove old points to keep the chart clean
            if (dataPoints.Count > 200) // Maximum number of points
            {
                dataPoints.RemoveAt(0); // Remove the oldest point
            }
        }

        private void textBoxPatientName_TextChanged(object sender, TextChangedEventArgs e)
        {
            lastPatientName = textBlockPatientName.Text;
        }

        private void textBoxPatientAge_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(textBoxPatientAge.Text, out int parsedage);
            lastPatientAge = parsedage;

        }

        private void textBoxFrequencyValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            double.TryParse(textBoxFrequencyValue.Text, out double parsedFrequency);
            lastFrequency = parsedFrequency;
            switch (parameter)
            {
                case (MonitorConstants.Parameter.ECG):
                    if (lastPatient) { patient.ECGFrequency = lastFrequency; }
                    break;
                case (MonitorConstants.Parameter.EMG):
                    if (lastPatient) patient.EMGFrequency = lastFrequency;
                    break;
                case (MonitorConstants.Parameter.EEG):
                    if (lastPatient) patient.EEGFrequency = lastFrequency;
                    break;
                case (MonitorConstants.Parameter.Resp):
                    if (lastPatient) patient.RespFrequency = lastFrequency;
                    break;
            }
            
        }

        private void sliderAmplitudeValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            ampValue = sliderAmplitudeValue.Value;
            switch (parameter)
            {
                case (MonitorConstants.Parameter.ECG):
                    if (lastPatient) { patient.ECGAmplitude = sliderAmplitudeValue.Value;
                    }
                    break;
                case (MonitorConstants.Parameter.EMG):
                   if (lastPatient) patient.EMGAmplitude = sliderAmplitudeValue.Value;
                    break;
                case (MonitorConstants.Parameter.EEG):
                    if (lastPatient) patient.EEGAmplitude = sliderAmplitudeValue.Value;
                    break;
                case (MonitorConstants.Parameter.Resp):
                    if (lastPatient) patient.RespAmplitude = sliderAmplitudeValue.Value;
                    break;
            }

        }

        private void datePickerDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dateTime = datePickerDate.SelectedDate.Value;
            
        }

        private void buttonCreatePatient_Click(object sender, RoutedEventArgs e)
        {

            bool isAgeValid = int.TryParse(textBoxPatientAge.Text, out int patientAge);

            bool isNameValid = textBoxPatientName.Text != "Enter name here" &&
            !string.IsNullOrWhiteSpace(textBoxPatientName.Text);

            bool isDateSelected = datePickerDate.SelectedDate.HasValue;

            if (!isNameValid) MessageBox.Show("Name is not Valid!");
            if (!isAgeValid) MessageBox.Show("Age is not Valid!");
            if (!isDateSelected) MessageBox.Show("Date is not Valid!");

            if (isNameValid && isAgeValid && isDateSelected)
            {
                //lastPatientName = textBlockPatientName.Text;
                patient = new Patient(lastPatientName, lastPatientAge, dateTime, ampValue, lastFrequency, lastHarmonics);
                
                lastPatient = true;
                buttonUpdatePatient.IsEnabled = true;
                
                MessageBox.Show("Patient was created!");
                //buttonStartSimulation.IsEnabled = true;
            }
            else
            {
               // MessageBox.Show("Fill all boxes!");
            }

        }

        private void buttonQuit_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void buttonParameter_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            sliderAmplitudeValue.IsEnabled = true;
            textBoxFrequencyValue.IsEnabled = true;
            comboBoxHarmonics.IsEnabled = true;
            comboBoxParameter.IsEnabled = true;
        }

        private void textBoxPatientAge_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void textBoxFrequencyValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void textBoxPatientName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Enter name here";
                textBox.Foreground = Brushes.Red;
            }

        }

        private void textBoxPatientName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Clear();
                textBox.Foreground = Brushes.Black;
            }
        }

        private void textBoxFrequencyValue_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Clear();
                textBox.Foreground = Brushes.Black;
            }
        }

        private void textBoxFrequencyValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Enter frequency here";
                textBox.Foreground = Brushes.Red;
            }
        }

        private void buttonUpdatePatient_Click(object sender, RoutedEventArgs e)
        {
            resetParameters();
        }

        private void ComboBoxHarmonics_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lastHarmonics = comboBoxHarmonics.SelectedIndex;
            if (lastPatient) { patient.ECGHarmonics = lastHarmonics; UpdateHarmonics(); }
            

        }

        private void textBoxPatientAge_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Enter age here";
                textBox.Foreground = Brushes.Red;
            }
        }
        private void comboBoxParameter_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.IsEnabled) { comboBox.SelectionChanged += comboBoxParameter_SelectionChanged; }

            else { comboBox.SelectionChanged -= comboBoxParameter_SelectionChanged; }

        }

        private void comboBoxParameter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            parameter = (MonitorConstants.Parameter)comboBoxParameter.SelectedIndex;

            switch (parameter)
            {

                case MonitorConstants.Parameter.ECG:
                    {
                        if (lastPatient) textBoxFrequencyValue.Text = patient.ECGFrequency.ToString();
                                         sliderAmplitudeValue.Value = patient.ECGAmplitude;
                                         comboBoxHarmonics_deaktivation(true);

                    }
                    break;
                case MonitorConstants.Parameter.EMG:
                    {
                        if (lastPatient) textBoxFrequencyValue.Text = patient.EMGFrequency.ToString();
                                         sliderAmplitudeValue.Value = patient.EMGAmplitude;
                                         comboBoxHarmonics_deaktivation(false);

                    }
                    break;
                case MonitorConstants.Parameter.EEG:
                    {
                        if (lastPatient) textBoxFrequencyValue.Text = patient.EEGFrequency.ToString();
                                         sliderAmplitudeValue.Value = patient.EEGAmplitude;
                                         comboBoxHarmonics_deaktivation(false);

                    }
                    break;
                case MonitorConstants.Parameter.Resp:
                    {
                        if (lastPatient) textBoxFrequencyValue.Text = patient.RespFrequency.ToString();
                                         sliderAmplitudeValue.Value = patient.RespAmplitude;
                                         comboBoxHarmonics_deaktivation(false);


                    }
                    break;
            }

            allParameter = savedParameters;

        }

        private void resetParameters()
        {
            // Setze die Frequenz, Amplitude und Harmonischen auf Standardwerte zurück
            lastFrequency = 0.0;
            ampValue = 0.0;
            lastHarmonics = 0;

            // Aktualisiere die Anzeigeelemente
            textBoxFrequencyValue.Text = "0";
            sliderAmplitudeValue.Value = 0;
            comboBoxHarmonics.SelectedIndex = 0;

        }

        private void comboBoxHarmonics_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.IsEnabled) { comboBox.SelectionChanged += comboBoxParameter_SelectionChanged; }

            else { comboBox.SelectionChanged -= comboBoxParameter_SelectionChanged; }

        }
        private void UpdateHarmonics()
        {
            if (parameter == MonitorConstants.Parameter.ECG)
            {
                lastlastHarmonics = patient.ECGHarmonics;
            }
        }

        private void comboBoxHarmonics_deaktivation(bool ein_oderAus)
        {
            if (ein_oderAus)
            {
                comboBoxHarmonics.SelectedIndex = lastlastHarmonics;
                comboBoxHarmonics.Visibility = Visibility.Visible;
                comboBoxHarmonics.IsEnabled = true;
                textBlockHarmonics.Text = "Harmonics : ";
            }
            else 
            {
                comboBoxHarmonics.IsEnabled = false;
                comboBoxHarmonics.Visibility = Visibility.Collapsed; textBlockHarmonics.Text = "";
            }
        }
    }
}
