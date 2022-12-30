using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.IO;

namespace Server
{

    public class Patient
    {
        public string name;
        public int[] systolicBP = new int[12];
        public int[] diastolicBP = new int[12];
        public int[] heartRate = new int[12];
        public int[] oxygen_saturation = new int[12];
        public double[] cholesterol_levels = new double[12];
        public double[] blood_sugar = new double[12];
        public static int i = 0;
        public Patient(string name = "ali",int systolicBP=120, int diastolicBP=80, int heartRate=70, int oxygen_saturation=98, double cholesterol_levels = 4.95, double blood_sugar = 15.13)
        {
            this.name = name;
            this.systolicBP[i] = systolicBP;
            this.diastolicBP[i] = diastolicBP;
            this.heartRate[i] = heartRate;
            this.oxygen_saturation[i] = oxygen_saturation;
            this.cholesterol_levels[i] = cholesterol_levels;
            this.blood_sugar[i] = blood_sugar;
            i++;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int[] systolicBP = new int[11];
            int[] diastolicBP = new int[11];
            int[] heartRate = new int[11];
            int[] oxygen_saturation = new int[11];
            double[] total_cholesterol = new double[11];
            double[] blood_sugar = new double[11];
            IPAddress ipa = new IPAddress(new byte[] { 127, 0, 0, 1 });
            IPEndPoint local = new IPEndPoint(ipa, 12010);
            UdpClient udp = new UdpClient(local);
            IPEndPoint iPEnd = new IPEndPoint(ipa, 10000);
            IPEndPoint endPoint = new IPEndPoint(ipa, 12030);
            Patient p = new Patient();
            Console.WriteLine("Server is receiving data from device...");
            int i = 0;

            while (true)
            {
                
                byte[] bytes = udp.Receive(ref iPEnd);
                byte[] byts = udp.Receive(ref endPoint);

                string str = Encoding.ASCII.GetString(byts);
                if (str.Equals("ali"))
                {
                    byte[] message = new byte[384];
                    byte[] systolic = new byte[48];
                    systolic = p.systolicBP.SelectMany(BitConverter.GetBytes).ToArray();
                    for(int k = 0; k < systolic.Length; k++)
                    {
                        message[k] = systolic[k];
                    }
                    byte[] diastolic = new byte[48];
                    diastolic = p.diastolicBP.SelectMany(BitConverter.GetBytes).ToArray();
                    for(int k = 0; k < diastolic.Length; k++)
                    {
                        message[k + 48] = diastolic[k];
                    }
                    byte[] pulse = new byte[48];
                    pulse = p.heartRate.SelectMany(BitConverter.GetBytes).ToArray();
                    for(int k = 0;k < pulse.Length; k++)
                    {
                        message[k + 96] = pulse[k];
                    }
                    byte[] oxygen = new byte[48];
                    oxygen = p.oxygen_saturation.SelectMany(BitConverter.GetBytes).ToArray();
                    for(int k = 0; k < oxygen.Length; k++)
                    {
                        message[k + 144] = oxygen[k];
                    }
                    byte[] cholesterol = new byte[96];
                    cholesterol = p.cholesterol_levels.SelectMany(BitConverter.GetBytes).ToArray();
                    for(int k = 0; k < cholesterol.Length; k++)
                    {
                        message[k + 192] = cholesterol[k];
                    }
                    byte[] bsg = new byte[96];
                    bsg = p.blood_sugar.SelectMany(BitConverter.GetBytes).ToArray();
                    for(int k = 0;k < bsg.Length; k++)
                    {
                        message[k + 288] = bsg[k];
                    }
                    udp.Connect("127.0.0.1", 12030);
                    udp.Send(message, message.Length);
                }

                Console.WriteLine("Systolic blood pressure: ");
                while (i < 11)
                {
                    systolicBP[i] = BitConverter.ToInt32(bytes, 4 * i);
                    p.systolicBP[i + 1] = systolicBP[i];
                    Console.Write(systolicBP[i] + "  ");
                    i++;
                }
                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------------------");

                Console.WriteLine("Diastolic blood pressure: ");
                while (i < 22)
                {
                    diastolicBP[i - 11] = BitConverter.ToInt32(bytes, 4 * i);
                    p.diastolicBP[i - 10] = diastolicBP[i - 11];
                    Console.Write(diastolicBP[i-11] + "  ");
                    i++;
                }
                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------------------");

                Console.WriteLine("Heart rate: ");
                while (i < 33)
                {
                    heartRate[i - 22] = BitConverter.ToInt32(bytes, 4 * i);
                    p.heartRate[i - 21] = heartRate[i - 22];
                    Console.Write(heartRate[i-22] + "  ");
                    i++;
                }
                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------------------");

                Console.WriteLine("Oxygen saturation levels: ");
                while (i < 44)
                {
                    oxygen_saturation[i - 33] = BitConverter.ToInt32(bytes, 4 * i);
                    p.oxygen_saturation[i - 32] = oxygen_saturation[i - 33];
                    Console.Write(oxygen_saturation[i-33] + "  ");
                    i++;
                }
                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------------------");

                Console.WriteLine("Total cholesterol (LDL and HDL) levels: ");
                while (i < 55)
                {
                    total_cholesterol[i - 44] = BitConverter.ToDouble(bytes, 8 * i - 176);
                    p.cholesterol_levels[i - 43] = total_cholesterol[i - 44];
                    Console.Write(total_cholesterol[i-44] + "  ");
                    i++;
                }
                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------------------");

                Console.WriteLine("Blood sugar levels: ");
                while (i < 66)
                {
                    blood_sugar[i - 55] = BitConverter.ToDouble(bytes, 8 * i - 176);
                    p.blood_sugar[i - 54] = blood_sugar[i - 55];
                    Console.Write(blood_sugar[i-55] + "  ");
                    i++;
                }
                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------------------");

                i = 0;
            }

            udp.Close();
            Console.ReadLine();
        }
    }
}
