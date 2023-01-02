using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.IO;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class Patient : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string v)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(v));
        }
        public string name;
        public int[] systolicBP = new int[12];
        public int[] diastolicBP = new int[12];
        public int[] heartRate = new int[12];
        public int[] oxygen_saturation = new int[12];
        public double[] total_cholesterol = new double[12];
        public double[] blood_sugar = new double[12];
        public static int i = 0;
        public static int k = 0;
        public static int r = 0;
        public static int t = 0;
        public static int g = 0;
        public static int q = 0;
        public static int e = 0;
        public Patient(string name = "", int systolicBP = 0, int diastolicBP = 0, int heartRate = 0, int oxygen_saturation = 0, double total_cholesterol = 0, double blood_sugar = 0)
        {
            this.name = name;
            this.systolicBP[q] = systolicBP;
            this.diastolicBP[q] = diastolicBP;
            this.heartRate[q] = heartRate;
            this.oxygen_saturation[q] = oxygen_saturation;
            this.total_cholesterol[q] = total_cholesterol;
            this.blood_sugar[q] = blood_sugar;
            q++;
        }
        public int SystolicBP
        {
            get { return systolicBP[i]; }
            set { systolicBP[i] = value;
                i++;
                if (i == 12)
                    i = 0;
                this.NotifyPropertyChanged("SystolicBP");
            }
        }
        public int DiastolicBP
        {
            get { return diastolicBP[k]; }
            set
            {
                diastolicBP[k] = value;
                k++;
                if (k == 12)
                    k = 0;
                this.NotifyPropertyChanged("DiastolicBP");
            }
        }
        public int HeartRate
        {
            get
            {
                return heartRate[r];
            }
            set
            {
                heartRate[r] = value;
                r++;
                if (r == 12)
                    r = 0;
                this.NotifyPropertyChanged("HeartRate");
            }
        }
        public int Oxygen_saturation
        {
            get
            {
                return oxygen_saturation[t];
            }
            set
            {
                oxygen_saturation[t] = value;
                t++;
                if (t == 12)
                    t = 0;
                this.NotifyPropertyChanged("Oxygen_saturation");
            }
        }
        public double Total_cholesterol
        {
            get { return total_cholesterol[g]; }
            set
            {
                total_cholesterol[g] = value;
                g++;
                if (g == 12)
                    g = 0;
                this.NotifyPropertyChanged("Total_cholesterol");
            }
        }
        public double Blood_sugar
        {
            get { return blood_sugar[e]; }
            set
            {
                blood_sugar[e] = value;
                e++;
                if (e == 12)
                    e = 0;
                this.NotifyPropertyChanged("Blood_sugar");
            }
        }
    }

    public class SystolicBloodPressure : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string v)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(v));
        }
        public int bp;
        public int BP { get { return bp; } set { bp = value; this.NotifyPropertyChanged("BP"); } }
    }

    public class DiastolicBloodPressure : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string v)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(v));
        }
        public int bp;
        public int BP { get { return bp; } set { bp = value; this.NotifyPropertyChanged("BP"); } }
    }

    public class HeartRate : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string v)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(v));
        }
        public int hr;
        public int HR { get { return hr; } set { hr = value; this.NotifyPropertyChanged("HR"); } }

    }

    public class Oxygen_saturation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string v)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(v));
        }
        public int os;
        public int OS { get { return os; } set { os = value; this.NotifyPropertyChanged("OS"); } }
    }

    public class Total_cholesterol : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string v)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(v));
        }
        public double tc;
        public double TC { get { return tc; } set { tc = value; this.NotifyPropertyChanged("TC"); } }
    }

    public class Blood_sugar : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string v)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(v));
        }
        public double bs;
        public double BS { get { return bs; } set { bs = value; this.NotifyPropertyChanged("BS"); } }
    }

    

    

    public partial class MainWindow : Window
    {
        Patient p = new Patient();
        public ObservableCollection<Patient> patients = new ObservableCollection<Patient>();
        public MainWindow()
        {
            InitializeComponent();
            patients.Add(p);
            data_grid.ItemsSource = patients;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            IPAddress ipa = new IPAddress(new byte[] { 127, 0, 0, 1 });
            IPEndPoint local = new IPEndPoint(ipa, 10230);
            IPEndPoint iPEndPoint = new IPEndPoint(ipa, 12030);
            //Patient p = new Patient();

            UdpClient client = new UdpClient(local);
            client.Connect("127.0.0.1", 12030);
            string str = name_of_the_patient.Text.ToString();
            byte[] b = Encoding.ASCII.GetBytes(str);
            client.Send(b, b.Length);
            byte[] bytes = client.Receive(ref iPEndPoint);
            int i = 0;
            while(i < 12)
            {
                p.SystolicBP = BitConverter.ToInt32(bytes, 4 * i);
                i++;
            }
            while(i < 24)
            {
                p.DiastolicBP = BitConverter.ToInt32(bytes, 4 * i);
                i++;
            }
            while(i < 36)
            {
                p.HeartRate = BitConverter.ToInt32(bytes, 4 * i);
                i++;
            }
            while(i < 48)
            {
                p.Oxygen_saturation = BitConverter.ToInt32(bytes, 4 * i);
                i++;
            }
            while(i < 60)
            {
                p.Total_cholesterol = BitConverter.ToDouble(bytes, 8 * i - 192);
                i++;
            }
            while(i < 72)
            {
                p.Blood_sugar = BitConverter.ToDouble(bytes, 8 * i - 192);
                i++;
            }
        }

        private void All_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Last_Click(object sender, RoutedEventArgs e)
        {

        }

        private void All_of_sphygmomanometer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void All_of_cholesterol_meter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void All_of_glucometer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void All_of_pulse_oximeter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Last_of_sphygmomanometer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Last_of_cholesterol_meter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Last_of_glucometer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Last_of_pulse_oximeter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void All_analog_Click(object sender, RoutedEventArgs e)
        {

        }

        private void All_digital_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
