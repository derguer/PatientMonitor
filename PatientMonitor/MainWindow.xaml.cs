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
        int lastHarmonics;
        double ampValue;
        bool lastPatient = false;

        MonitorConstants.Parameter parameter; 

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
                    //if (lastPatient) patient.EEGFrequency = lastFrequency; noch nicht die klasse implimentiert 
                    break;
            }
            
        }

        private void sliderAmplitudeValue_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            ampValue = sliderAmplitudeValue.Value;
            switch (parameter)
            {
                case (MonitorConstants.Parameter.ECG):
                    if (lastPatient) patient.ECGAmplitude = ampValue;
                    break;
                case (MonitorConstants.Parameter.EMG):
                    if (lastPatient) patient.EMGAmplitude = ampValue;
                    break;
                case (MonitorConstants.Parameter.EEG):
                    //if (lastPatient) patient.EEGAmplitude = ampValue; noch nicht die klasse implimentiert 
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
                MessageBox.Show("Fill all boxes!");
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

        }

        private void ComboBoxHarmonics_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lastHarmonics = comboBoxHarmonics.SelectedIndex;
            if (lastPatient) patient.ECGHarmonics = lastHarmonics;

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
            //if (comboBox.IsEnabled) { comboBox.SelectionChanged += comboBoxParameter_SelectionChanged; }
            //else { comboBox.SelectionChanged -= comboBoxParameter_SelectionChanged; }

            switch (comboBoxParameter.SelectedIndex) {

                case 0: { parameter = MonitorConstants.Parameter.ECG;
                        MessageBox.Show("ECG Erstellt");
                    } 
                    break;
                case 1:
                    {
                        parameter = MonitorConstants.Parameter.EMG;
                        MessageBox.Show("EMG Erstellt");
                    }
                    break;
                case 2: { parameter = MonitorConstants.Parameter.EEG; } 
                    break;
                case 3: { parameter = MonitorConstants.Parameter.Resp; } 
                    break;
                //default: parameter = MonitorConstants.Parameter.ECG;
                    //MessageBox.Show("ECG Erstellt");
                    //break;
            }

            
        }

        private void comboBoxParameter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            parameter = (MonitorConstants.Parameter)comboBoxParameter.SelectedIndex;
        }


    }
}
