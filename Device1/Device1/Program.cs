using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.IO;

namespace Device1
{
    // struct Measurment {
    //    public int digital;
    //    public double analog;
    //}

    public enum Type { Analog, Digital };



    public abstract class Device
    {
        public int deviceID;
        public abstract string WriteMeasurment();
        public Type t;
        //int timeStamp;
        //Measurment m = new Measurment();
        public abstract void MakeMeasurment(int ts);
    }

    public class Sphygmomanometer : Device
    {
        public int sphID;
        public int[] systolicBP = new int[11];
        public int[] diastolicBP = new int[11];
        public int[] heartRate = new int[11];
        public byte[] diastolic = new byte[44];
        public byte[] systolic = new byte[44];
        public byte[] pulse = new byte[44];
        public static int i = 0;
        public Type ti;
        public override string WriteMeasurment()
        {
            string s = "Systolic blood pressure is " + systolicBP[i] + " mmHg, while diastolic blood pressure is " + diastolicBP[i] + " mmHG. Heart rate of the patient is " + heartRate[i] + "\n";
            return s;
        }
        public Sphygmomanometer(int sphID = 0, int systolicBP = 120, int diastolicBP = 80, int heartRate = 70)
        {

            this.sphID = sphID;
            this.systolicBP[i] = systolicBP;
            this.diastolicBP[i] = diastolicBP;
            this.heartRate[i] = heartRate;
            ti = Type.Digital;
            string str = WriteMeasurment();
            Console.WriteLine(str);
            i++;
            if (i == 11)
                i = 0;
        }
        public override void MakeMeasurment(int ts)
        {
            Random sbp = new Random();
            Random dbp = new Random();
            Random hr = new Random();

            systolicBP[i] = sbp.Next(100, 200);
            diastolicBP[i] = dbp.Next(60, 90);
            heartRate[i] = hr.Next(60, 120);
            string str = WriteMeasurment();
            Console.WriteLine(str);
            i++;
            if (i == 11)
                i = 0;


        }
    }

    public class Glucometer : Device
    {
        public int glucID;
        public double[] blood_sugar = new double[11];
        public byte[] bsg = new byte[44];
        public static int i = 0;
        public Type ti;

        public override string WriteMeasurment()
        {
            string str = "Blood sugar level of the patient is " + blood_sugar[i] + " mg/dL\n";
            return str;
        }

        public override void MakeMeasurment(int ts)
        {
            Random bs = new Random(180);

            
                blood_sugar[i] = 20 * bs.NextDouble();
                
                string str = WriteMeasurment();
                Console.WriteLine(str);
            i++;
            if (i == 11)
                i = 0;
            
        }

        public Glucometer(int glucID = 1, double blood_sugar = 20)
        {
            this.glucID = glucID;
            this.blood_sugar[i] = blood_sugar;
            this.ti = Type.Analog;
            string str = WriteMeasurment();
            Console.WriteLine(str);
            i++;
            if (i == 11)
                i = 0;
        }
    }

    public class Cholesterol_meter : Device
    {
        public int chID;
        public double[] total_cholesterol = new double[11];
        public Type t1;
        public static int i = 0;
        public byte[] cholesterol = new byte[88];

        public Cholesterol_meter(int chID = 2, double total_cholesterol = 4.15, Type t1 = Type.Analog)
        {
            this.chID = chID;
            this.total_cholesterol[i] = total_cholesterol;
            string s = WriteMeasurment();
            Console.WriteLine(s);
            i++;
            if (i == 11)
                i = 0;
            this.t1 = t1;
        }

        public override string WriteMeasurment()
        {
            string s = "Total cholesterol levels (LDL and HDL) of the patient are " + total_cholesterol[i] + " mmol/L\n";
            return s;
        }

        public override void MakeMeasurment(int ts)
        {
            Random r = new Random();
            
                total_cholesterol[i] = 5 * r.NextDouble();
                
                string str = WriteMeasurment();
                Console.WriteLine(str);
            i++;
            if (i == 11)
                i = 0;
            
        }
    }

    public class Pulse_oximeter : Device
    {
        public int pulseID;
        public int[] oxygen_saturation = new int[11];
        public Type t2;
        public static int i = 0;
        public byte[] oxygen = new byte[88];

        public Pulse_oximeter(int pulseID = 3, int oxygen_saturation1 = 99, Type t2 = Type.Digital)
        {
            this.pulseID = pulseID;
            this.oxygen_saturation[i] = oxygen_saturation1;
            string s = WriteMeasurment();
            Console.WriteLine(s);
            i++;
            if (i == 11)
                i = 0;
            this.t2 = t2;
        }
        public override string WriteMeasurment()
        {
            return "Patient has oxygen saturation level of " + oxygen_saturation[i] + " %\n";
        }

        public override void MakeMeasurment(int ts)
        {
            Random r = new Random();
            

                oxygen_saturation[i] = r.Next(0, 100);
                
                string str = WriteMeasurment();
                Console.WriteLine(str);
            i++;
            if (i == 11)
                i = 0;
            
        }
    }

    public class Program
    {

        static void Main(string[] args)
        {
            UdpClient udp = new UdpClient(10000);
            udp.Connect("127.0.0.1", 12010);
            Sphygmomanometer s = new Sphygmomanometer();
            Glucometer gm = new Glucometer();
            Cholesterol_meter cm = new Cholesterol_meter();
            Pulse_oximeter po = new Pulse_oximeter();
            int i = 0;
            byte[] bytes = new byte[352];

            while (true)
            {
                s.MakeMeasurment(1);


                gm.MakeMeasurment(1);


                cm.MakeMeasurment(1);


                po.MakeMeasurment(1);

                Thread.Sleep(1000);


                i++;
                if (i == 11)
                {
                    s.systolic = s.systolicBP.SelectMany(BitConverter.GetBytes).ToArray();
                    s.diastolic = s.diastolicBP.SelectMany(BitConverter.GetBytes).ToArray();
                    s.pulse = s.heartRate.SelectMany(BitConverter.GetBytes).ToArray();
                    po.oxygen = po.oxygen_saturation.SelectMany(BitConverter.GetBytes).ToArray();
                    cm.cholesterol = cm.total_cholesterol.SelectMany(BitConverter.GetBytes).ToArray();
                    gm.bsg = gm.blood_sugar.SelectMany(BitConverter.GetBytes).ToArray();

                    for (int k = 0;k < s.systolic.Length; k++)
                    {
                        bytes[k] = s.systolic[k];
                    }

                    for(int k = 0;k < s.diastolic.Length; k++)
                    {
                        bytes[k + 44] = s.diastolic[k];
                    }

                    for(int k = 0;k < s.pulse.Length; k++)
                    {
                        bytes[k + 88] = s.pulse[k];
                    }

                    for(int k = 0; k < po.oxygen.Length; k++)
                    {
                        bytes[k + 132] = po.oxygen[k];
                    }

                    for(int k = 0; k < cm.cholesterol.Length; k++)
                    {
                        bytes[k + 176] = cm.cholesterol[k];
                    }

                    for(int k = 0; k < gm.bsg.Length; k++)
                    {
                        bytes[k + 264] = gm.bsg[k];
                    }

                    udp.Send(bytes, bytes.Length);

                    for(int k = 0;k < bytes.Length; k++)
                    {
                        bytes[k] = 0;
                    }

                    for (int k = 0; k < s.systolic.Length; k++)
                    {
                        s.systolic[k] = 0;
                        s.diastolic[k] = 0;
                        s.pulse[k] = 0;
                    }

                    for(int k = 0;k < cm.cholesterol.Length; k++)
                    {
                        cm.cholesterol[k] = 0;
                        gm.bsg[k] = 0;
                    }

                    i = 0;
                }


            }

            udp.Close();
            Console.ReadLine();
        }
    }
}
