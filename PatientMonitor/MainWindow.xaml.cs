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
using Microsoft.Win32;


namespace PatientMonitor
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double lastFrequency, lastHighAlarmFrequency, lastLowAlarmFrequency;
        private double ampValue;

        private DateTime dateTime;

        private int index;
        private int lastPatientAge;
        private int lastRoom;
        private int lastHarmonics, lastlastHarmonics;
        private const int sampleSize = 512; // FFT dimension

        private bool lastPatient = false;

        private ObservableCollection<KeyValuePair<int, double>> dataPointsTime;
        private ObservableCollection<KeyValuePair<int, double>> dataPointsFrequency;
        private DispatcherTimer timer;
        private Patient patient;

        private MonitorConstants.Parameter parameter;

        private MonitorConstants.Parameter[] allParameter = {
            MonitorConstants.Parameter.ECG,
            MonitorConstants.Parameter.EMG,
            MonitorConstants.Parameter.EEG,
            MonitorConstants.Parameter.Resp
        };

        private MonitorConstants.Parameter[] savedParameters = {
            MonitorConstants.Parameter.ECG,
            MonitorConstants.Parameter.EMG,
            MonitorConstants.Parameter.EEG,
            MonitorConstants.Parameter.Resp
        };

        Stationary stationary;
        Database database;

        private string lastPatientName = "";


        public MainWindow()
        {
            InitializeComponent();
            database = new Database();
            
            dataPointsTime = new ObservableCollection<KeyValuePair<int, double>>();
            dataPointsFrequency = new ObservableCollection<KeyValuePair<int, double>>();
            lineSeriesECG.ItemsSource = dataPointsTime; // Bind the series to the data points
            patientDataGrid.Visibility = switchParameterDatabase.IsChecked == false ? Visibility.Hidden : Visibility.Visible;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1); // Set timer to tick every second
            timer.Tick += Timer_Tick;

            // Bind the DataGrid einmalig an die ObservableCollection
            patientDataGrid.ItemsSource = database.Data;

            comboBoxSort.SelectedIndex = 5;
 
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
                 displayTime();       
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
            bool isNameValid = !string.IsNullOrWhiteSpace(textBoxPatientName.Text);
            bool isDateSelected = datePickerDate.SelectedDate.HasValue;
            bool isRoomValid = int.TryParse(textBoxPatientRoom.Text, out int patientRoom);
            

            if (!isRoomValid) patientRoom = 0; // Standardwert für Raum, falls ungültig

            if (!isNameValid) MessageBox.Show("Name is not valid!");
            if (!isAgeValid) MessageBox.Show("Age is not valid!");
            if (!isDateSelected) MessageBox.Show("Date is not valid!");

            if (isNameValid && isAgeValid && isDateSelected)
            {
                MonitorConstants.clinic selectedClinic = (MonitorConstants.clinic)comboBoxClinic.SelectedIndex;
                DateTime dateTime = datePickerDate.SelectedDate.Value;

                // Sicherstellen, dass Frequenz und Amplitude korrekt erfasst werden
                double.TryParse(textBoxFrequencyValue.Text, out double frequency);
                double amplitude = sliderAmplitudeValue.Value;

                Debug.WriteLine($"Creating Patient: Amplitude={amplitude}, Frequency={frequency}");

                if (switchAmbulatoryStationary.IsChecked == true) // Stationary
                {
                    if (patientRoom > 0)
                    {
                        stationary = new Stationary(
                            textBoxPatientName.Text,
                            patientAge,
                            dateTime,
                            ampValue, // Übergebe Amplitude
                            lastFrequency, // Übergebe Frequenz
                            lastHarmonics,
                            selectedClinic,
                            patientRoom
                        )
                        {
                            Type = "Stationary"
                        };
                        patient = stationary;
                    }
                    else
                    {
                        MessageBox.Show("Room number is required for Stationary patients.");
                        return;
                    }
                }
                else // Ambulatory
                {
                    patient = new Patient(
                        textBoxPatientName.Text,
                        patientAge,
                        dateTime,
                        ampValue, // Übergebe Amplitude
                        lastFrequency, // Übergebe Frequenz
                        lastHarmonics,
                        selectedClinic
                    )
                    {
                        Type = "Ambulatory",
                        Room = "No Room" // Standardwert für Ambulatory
                    };
                }

                // Füge den Patienten der Datenbank hinzu
                database.AddPatient(patient);

                // Aktualisiere das DataGrid
                patientDataGrid.ItemsSource = null; // Setze die Quelle zurück
                patientDataGrid.ItemsSource = database.GetPatients(); // Aktualisiere mit neuer Liste

                // Aktiviere weitere Buttons
                
                buttonParameter.IsEnabled = true;
                buttonSafeDatabase.IsEnabled = true;

                MessageBox.Show("Patient was created!");
            }
        }



        private void buttonQuit_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            sliderAmplitudeValue.IsEnabled = false;
            textBoxFrequencyValue.IsEnabled = false;
            comboBoxHarmonics.IsEnabled = false;
            comboBoxParameter.IsEnabled = false;

            //Image Buttons
            buttonPrev.IsEnabled = false;
            buttonNext.IsEnabled = false;
            buttonLoadImage.IsEnabled = false;

            //Alarm
            textBoxHightAlarm.IsEnabled = false;
            textBoxLowAlarm.IsEnabled = false;
            buttonFFT.IsEnabled = false;
            
        }

        private void buttonParameter_Click(object sender, RoutedEventArgs e)
        {
           
            timer.Start();
            sliderAmplitudeValue.IsEnabled = true;
            textBoxFrequencyValue.IsEnabled = true;
            comboBoxHarmonics.IsEnabled = true;
            comboBoxParameter.IsEnabled = true;
            lastPatient = true;

            //Image Buttons
            buttonPrev.IsEnabled = true;
            buttonNext.IsEnabled = true;
            buttonLoadImage.IsEnabled = true;

            //Alarm
            textBoxHightAlarm.IsEnabled = true;
            textBoxLowAlarm.IsEnabled = true;
            buttonFFT.IsEnabled = true;
            


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

        private void buttonLoadDatabase_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Patient Database Files|*.bin",
                Title = "Load Patient Database"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                try
                {
                    // Datenbank laden
                    database.OpenData(path);
                    MessageBox.Show("Database loaded successfully!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Aktualisiere die DataGrid ItemsSource
                    patientDataGrid.ItemsSource = null; // Temporär auf null setzen
                    patientDataGrid.ItemsSource = database.Data; // Wieder binden

                    // Setze den aktiven Patienten auf den letzten in der Liste
                    if (database.Data.Count > 0)
                    {
                        patient = database.Data[database.Data.Count - 1]; // Letzter Patient in der Liste wird "aktiv"
                        DisplayPatientInInputSection(patient);
                    }
                }
                catch (InvalidOperationException ex)
                {
                    // Behandlung spezifischer Ausnahmen, z.B. maximale Kapazität überschritten
                    MessageBox.Show($"Failed to load database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    // Allgemeine Fehlerbehandlung
                    MessageBox.Show($"An error occurred while loading the database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }




        private void ComboBoxHarmonics_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lastHarmonics = comboBoxHarmonics.SelectedIndex;
            if (lastPatient) { 
                
                patient.ECGHarmonics = lastHarmonics; 
                UpdateHarmonics();    
            }
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
                                         textBoxHightAlarm.Text = patient.ECGHighAlarm.ToString();
                                         textBoxLowAlarm.Text = patient.ECGLowAlarm.ToString();
                                         textBlockAlarm.Text = patient.ECGHighAlarmString;
                                         textBlockAlarm.Text = patient.ECGLowAlarmString;

                    }
                    break;
                case MonitorConstants.Parameter.EMG:
                    {
                        if (lastPatient) textBoxFrequencyValue.Text = patient.EMGFrequency.ToString();
                                         sliderAmplitudeValue.Value = patient.EMGAmplitude;
                                         comboBoxHarmonics_deaktivation(false);
                                         textBoxHightAlarm.Text = patient.EMGHighAlarm.ToString();
                                         textBoxLowAlarm.Text = patient.EMGLowAlarm.ToString();
                                         textBlockAlarm.Text = patient.EMGHighAlarmString;
                                         textBlockAlarm.Text = patient.EMGLowAlarmString;



                    }
                    break;
                case MonitorConstants.Parameter.EEG:
                    {
                        if (lastPatient) textBoxFrequencyValue.Text = patient.EEGFrequency.ToString();
                                         sliderAmplitudeValue.Value = patient.EEGAmplitude;
                                         comboBoxHarmonics_deaktivation(false);
                                         textBoxHightAlarm.Text = patient.EEGHighAlarm.ToString();
                                         textBoxLowAlarm.Text = patient.EEGLowAlarm.ToString();
                                         textBlockAlarm.Text = patient.EEGHighAlarmString;
                                         textBlockAlarm.Text = patient.EEGLowAlarmString;

                    }
                    break;
                case MonitorConstants.Parameter.Resp:
                    {
                        if (lastPatient) textBoxFrequencyValue.Text = patient.RespFrequency.ToString();
                                         sliderAmplitudeValue.Value = patient.RespAmplitude;
                                         comboBoxHarmonics_deaktivation(false);
                                         textBoxHightAlarm.Text = patient.RespHighAlarm.ToString();
                                         textBoxLowAlarm.Text = patient.RespLowAlarm.ToString();
                                         textBlockAlarm.Text = patient.RespiHighAlarmString;
                                         textBlockAlarm.Text = patient.RespiLowAlarmString;


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
            textBlockPatientAge.Text = "Enter age here";
            textBlockPatientName.Text = "Enter name here";
            //datePickerDate.SelectedDate = null;

            MyImage.Source = null;
            timer.Stop();

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

        private void buttonLoadImage_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Valid name-format for image file: " + "\n BASE**.ext \n BASE is an arbitary string \n ** are 2 digits \n .ext is the image format");
            if (patient == null || patient.MRImages == null)
            {
                MessageBox.Show("No patient or image database initialized.");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.bmp)|*.bmp|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Lade die Bilder basierend auf dem ausgewählten Bild
                    patient.MRImages.LoadImages(openFileDialog.FileName);

                    // Zeige das erste Bild (Base01.bmp)
                    MyImage.Source = patient.MRImages.AnImage;
                    textBoxImageIndex.Text = patient.MRImages.GetImageIndexText();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load images: {ex.Message}");
                }
            }
        }

        private void textBoxLowAlarm_TextChanged(object sender, TextChangedEventArgs e)
        {
            double.TryParse(textBoxLowAlarm.Text, out double parsedFrequency);
            lastLowAlarmFrequency = parsedFrequency;

            switch (parameter)
            {
                case (MonitorConstants.Parameter.ECG):
                    if (lastPatient) { 
                        patient.ECGLowAlarm = lastLowAlarmFrequency;
                        textBlockAlarm.Text = patient.ECGLowAlarmString;
                    }
                    break;
                case (MonitorConstants.Parameter.EMG):
                    if (lastPatient) { 
                        patient.EMGLowAlarm = lastLowAlarmFrequency;
                        textBlockAlarm.Text = patient.EMGLowAlarmString;

                    }
                    break;
                case (MonitorConstants.Parameter.EEG):
                    if (lastPatient) { 
                        patient.EEGLowAlarm = lastLowAlarmFrequency;
                        textBlockAlarm.Text = patient.EEGLowAlarmString;
                    }
                    break;
                case (MonitorConstants.Parameter.Resp):
                    if (lastPatient) { 
                        patient.RespLowAlarm = lastLowAlarmFrequency;
                        textBlockAlarm.Text = patient.RespiLowAlarmString;
                    }
                    break;
            }
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (patient == null || patient.MRImages == null)
            {
                MessageBox.Show("No patient or image database initialized.");
                return;
            }

            try
            {
                // Nächstes Bild anzeigen
                patient.MRImages.ForwardImage();
                MyImage.Source = patient.MRImages.AnImage;
                textBoxImageIndex.Text = patient.MRImages.GetImageIndexText();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to navigate to the next image: {ex.Message}");
            }
        }

        private void textBoxHighAlarm_TextChanged(object sender, TextChangedEventArgs e)
        {
            double.TryParse(textBoxHightAlarm.Text, out double parsedFrequency);
            lastHighAlarmFrequency = parsedFrequency;

            switch (parameter)
            {
                case (MonitorConstants.Parameter.ECG):
                    if (lastPatient)
                    {
                        patient.ECGHighAlarm = lastHighAlarmFrequency;
                        textBlockAlarm.Text = patient.ECGHighAlarmString;
                        
                    }
                    break;
                case (MonitorConstants.Parameter.EMG):
                    if (lastPatient)
                    {
                        patient.EMGHighAlarm = lastHighAlarmFrequency;
                        textBlockAlarm.Text = patient.EMGHighAlarmString;
                        

                    }
                    break;
                case (MonitorConstants.Parameter.EEG):
                    if (lastPatient)
                    {
                        patient.EEGHighAlarm = lastHighAlarmFrequency;
                        textBlockAlarm.Text = patient.EEGHighAlarmString;
                        
                    }
                    break;
                case (MonitorConstants.Parameter.Resp):
                    if (lastPatient)
                    {
                        patient.RespHighAlarm = lastHighAlarmFrequency;
                        textBlockAlarm.Text = patient.RespiHighAlarmString;
                    }
                    break;
            }
        }

        private void textBoxHightAlarm_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }
        private void textBoxLowAlarm_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }
        private void textBoxHighAlarm_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Clear();
                textBox.Foreground = Brushes.Black;
            }
        }
        private void textBoxLowAlarm_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Clear();
                textBox.Foreground = Brushes.Black;
            }
        }
        private void textBoxHighAlarm_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Enter High Alarm frequency here";
                textBox.Foreground = Brushes.Red;
            }
        }
        private void textBoxLowAlarm_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Enter Low Alarm frequency here";
                textBox.Foreground = Brushes.Red;
            }
        }

        private void buttonFFT_Click(object sender, RoutedEventArgs e)
        {
         
            if (patient.SampleList.Count > 512)
            {
                displayFrequency();
            }
            else
            {
                MessageBox.Show("Not enough samples available for FFT calculation.");
            }
        }

        private void buttonPrev_Click(object sender, RoutedEventArgs e)
        {
            if (patient == null || patient.MRImages == null)
            {
                MessageBox.Show("No patient or image database initialized.");
                return;
            }

            try
            {
                // Vorheriges Bild anzeigen
                patient.MRImages.BackwardImage();
                MyImage.Source = patient.MRImages.AnImage;
                textBoxImageIndex.Text = patient.MRImages.GetImageIndexText();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to navigate to the previous image: {ex.Message}");
            }
        }
        private void textBoxMaxImages_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(textBoxImageIndex.Text, out int newMaxImages) && newMaxImages > 0)
            {
                try
                {
                    // Begrenze die Anzahl der Bilder und aktualisiere die Anzeige
                    patient.MRImages.SetMaxImages(newMaxImages);

                    // Aktualisiere das aktuelle Bild und den Index
                    MyImage.Source = patient.MRImages.AnImage;
                    textBoxImageIndex.Text = patient.MRImages.GetImageIndexText();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
            else if (!string.IsNullOrWhiteSpace(textBoxImageIndex.Text))
            {
                // Für ungültige Eingaben: Zeige einen Platzhalterwert
                textBoxImageIndex.Text = patient.MRImages.MaxImages.ToString();
            }
        }
        private void textBoxMaxImages_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (int.TryParse(textBoxImageIndex.Text, out int newMaxImages) && newMaxImages > 0)
                {
                    try
                    {
                        // Begrenze die Anzahl der Bilder und aktualisiere die Anzeige
                        patient.MRImages.SetMaxImages(newMaxImages);

                        // Aktualisiere das aktuelle Bild und den Index
                        MyImage.Source = patient.MRImages.AnImage;
                        textBoxImageIndex.Text = patient.MRImages.GetImageIndexText();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid input. Please enter a positive integer.");
                    textBoxImageIndex.Text = patient.MRImages.MaxImages.ToString(); // Setze den alten Wert zurück
                }
            }
        }

        private void textBoxMaxImages_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _); // Blockiere nicht-numerische Eingaben
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void textBoxPatientRoom_TextChanged(object sender, TextChangedEventArgs e)
        {
                int.TryParse(textBoxPatientRoom.Text, out int parsedage);
                lastRoom = parsedage;

        }

        private void textBoxPatientRoom_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "Enter Room number";
                textBox.Foreground = Brushes.Red;
            }
        }

        private void textBoxPatientRoom_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Clear();
                textBox.Foreground = Brushes.Black;
            }
        }

        private void textBoxPatientRoom_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        private void switchAmbulatoryStationary_Checked(object sender, RoutedEventArgs e)
        {
            textBoxPatientRoom.IsEnabled = true;
        }

        private void switchAmbulatoryStationary_Unchecked(object sender, RoutedEventArgs e)
        {
            textBoxPatientRoom.Text = "No Room";
            textBoxPatientRoom.IsEnabled = false;
        }

        private void switchParameterDatabase_Checked(object sender, RoutedEventArgs e)
        {
            if (switchParameterDatabase.IsChecked == false)
            {
                // `switchParameterDatabase` ist aktiviert -> Verstecke das DataGrid
                patientDataGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                // `switchParameterDatabase` ist deaktiviert -> Zeige das DataGrid
                patientDataGrid.Visibility = Visibility.Visible;
            }
        }

        private void comboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Stelle sicher, dass ein tatsächlicher Sortier-Index ausgewählt ist
            if (comboBoxSort.SelectedIndex <= 0)
                return;

            PatientComparer pc = new PatientComparer();
            pc.CA = (MonitorConstants.compareAfter)comboBoxSort.SelectedIndex;

            // Hole die Patienten als Liste
            var list = database.Data.ToList();

            // Sortiere die Liste mit deinem Comparer
            list.Sort(pc);

            // Leere die bestehende ObservableCollection und füge die sortierten Patienten ein
            database.Data.Clear();
            foreach (var p in list)
            {
                database.Data.Add(p);
            }

            // Das DataGrid wird automatisch aktualisiert
        }

        private void displayDatabase()
        {
            
            //patientDataGrid.ItemsSource = null;
            //patientDataGrid.ItemsSource = database.Data;
        }

        private void buttonSafeDatabase_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Patient Database Files|*.bin",
                Title = "Save Patient Database"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                // Datenbank speichern
                database.SaveData(path);
                MessageBox.Show("Database saved successfully!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void patientDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (patientDataGrid.SelectedItem is Patient selectedPatient)
            {
                // Aktiven Patienten updaten
                patient = selectedPatient;

                // Patienten-Infos im Input-Bereich aktualisieren
                DisplayPatientInInputSection(patient);

            }
        }
        private void DisplayPatientInInputSection(Patient p)
        {
            if (p == null)
            {
                System.Diagnostics.Debug.WriteLine("DisplayPatientInInputSection: Patient is null.");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Displaying patient: {p.PatientName}, Age: {p.Age}");

            textBoxPatientName.Text = p.PatientName;
            textBoxPatientAge.Text = p.Age.ToString();
            datePickerDate.SelectedDate = p.DateOfStudy;
            comboBoxClinic.SelectedIndex = (int)p.Clinic;
            textBoxPatientRoom.Text = p is Stationary s ? s.RoomNumber.ToString() : "No Room";

            // Ambulatory/Stationary setzen
            switchAmbulatoryStationary.IsChecked = p is Stationary;

        }

        private void patientDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (patientDataGrid.SelectedItem is Patient selectedPatient)
            {
                patient = selectedPatient;
                DisplayPatientInInputSection(patient);
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
        private void displayTime()
        {
             
            // Generate a new data point          
            if (patient != null) dataPointsTime.Add(new KeyValuePair<int, double>(index++, patient.NextSample(index, parameter)));

            // Optional: Remove old points to keep the chart clean
            if (dataPointsTime.Count > 200) // Maximum number of points
            {
                dataPointsTime.RemoveAt(0); // Remove the oldest point
            }
        }
        private void displayFrequency()
        {
            
            const double samplingRate = 6000.0; // Example sampling rate in Hz
            Spektrum spektrum = new Spektrum(sampleSize);



            // Letzte 512 Samples aus SampleList extrahieren
            double[] timeDomainData = patient.SampleList
                .Skip(patient.SampleList.Count - sampleSize) // Überspringe ältere Werte
                .Take(sampleSize) // Nimm die letzten 512 Samples
                .ToArray();

            // Perform FFT and retrieve frequency spectrum
            double[] frequencySpectrum = spektrum.FFT(timeDomainData, sampleSize);

            // Clear old data points and update with frequency spectrum
            dataPointsFrequency.Clear();

            // Fülle die Datenpunkte für die Darstellung
            for (int i = 0; i < frequencySpectrum.Length / 2; i++) // Nur positive Frequenzen
            {
                double frequency = i * (samplingRate / sampleSize); // Frequenz berechnen
                double amplitude = frequencySpectrum[i]; // Amplitude abrufen
                dataPointsFrequency.Add(new KeyValuePair<int, double>((int)frequency, amplitude));
            }

            // Aktualisiere die Anzeige
            lineSeriesFFT.ItemsSource = null; // Reset ItemsSource
            lineSeriesFFT.ItemsSource = dataPointsFrequency;

        }
    }
}
