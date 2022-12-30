using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.IO;

namespace Proxy_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipa = new IPAddress(new byte[] { 127, 0, 0, 1 });
            IPEndPoint local = new IPEndPoint(ipa, 12030);
            UdpClient udp = new UdpClient(local);
            IPEndPoint iPEnd = new IPEndPoint(ipa, 10230);
            IPEndPoint endPoint = new IPEndPoint(ipa, 12010);
            string str = "";
            int[] systolicBP = new int[12];
            int[] diastolicBP = new int[12];
            int[] heartRate = new int[12];
            int[] oxygen_saturation = new int[12];
            double[] total_cholesterol = new double[12];
            double[] blood_sugar = new double[12];
            byte[] systolic = new byte[48];
            byte[] diastolic = new byte[48];
            byte[] pulse = new byte[48];
            byte[] oxygen = new byte[48];
            byte[] cholesterol = new byte[96];
            byte[] bsg = new byte[96];
            while (true)
            {
                byte[] bytes = udp.Receive(ref iPEnd);
                str = Encoding.ASCII.GetString(bytes);
                if (str.Equals("ali"))
                {
                    udp.Connect("127.0.0.1", 12010);
                    byte[] b = Encoding.ASCII.GetBytes(str);
                    udp.Send(b, b.Length);
                    byte[] message = udp.Receive(ref endPoint);
                    int i = 0;
                    Console.WriteLine("Systolic blood pressure: ");
                    while(i < 12)
                    {
                         systolicBP[i] = BitConverter.ToInt32(message, 4 * i);
                        systolic = systolicBP.SelectMany(BitConverter.GetBytes).ToArray();
                        Console.Write(systolicBP[i] + "  ");
                        i++;
                    }
                    Console.WriteLine();
                    Console.WriteLine("--------------------------------------------------------------");

                    Console.WriteLine("Diastolic blood pressure: ");
                    while(i < 24)
                    {
                        diastolicBP[i-12] = BitConverter.ToInt32(message, 4 * i);
                        diastolic = diastolicBP.SelectMany(BitConverter.GetBytes).ToArray();
                        Console.Write(diastolicBP[i] + "  ");
                        i++;
                    }
                    Console.WriteLine();
                    Console.WriteLine("--------------------------------------------------------------");

                    Console.WriteLine("Heart rate: ");
                    while (i < 36)
                    {
                        heartRate[i - 24] = BitConverter.ToInt32(message, 4 * i);
                        pulse = heartRate.SelectMany(BitConverter.GetBytes).ToArray();
                        Console.Write(heartRate[i] + "  ");
                        i++;
                    }
                    Console.WriteLine();
                    Console.WriteLine("--------------------------------------------------------------");

                    Console.WriteLine("Oxygen saturation levels: ");
                    while (i < 48)
                    {
                        oxygen_saturation[i - 36] = BitConverter.ToInt32(message, 4 * i);
                        oxygen = oxygen_saturation.SelectMany(BitConverter.GetBytes).ToArray();
                        Console.Write(oxygen_saturation[i] + "  ");
                        i++;
                    }
                    Console.WriteLine();
                    Console.WriteLine("--------------------------------------------------------------");

                    Console.WriteLine("Total cholesterol (LDL + HDL) levels: ");
                    while (i < 60)
                    {
                        total_cholesterol[i - 48] = BitConverter.ToDouble(message, 8 * i - 192);
                        cholesterol = total_cholesterol.SelectMany(BitConverter.GetBytes).ToArray();
                        Console.Write(total_cholesterol[i] + "  ");
                        i++;
                    }
                    Console.WriteLine();
                    Console.WriteLine("--------------------------------------------------------------");

                    Console.WriteLine("Blood sugar levels: ");
                    while (i < 72)
                    {
                        blood_sugar[i - 60] = BitConverter.ToDouble(message, 8 * i - 192);
                        bsg = blood_sugar.SelectMany(BitConverter.GetBytes).ToArray();
                        Console.Write(blood_sugar[i] + "  ");
                        i++;
                    }
                    Console.WriteLine();
                    Console.WriteLine("--------------------------------------------------------------");

                    byte[] numbers = new byte[384];
                    for(int k = 0; k < systolic.Length; k++)
                    {
                        numbers[k] = systolic[k];
                    }
                    for(int k = 0; k < diastolic.Length; k++)
                    {
                        numbers[k + 48] = diastolic[k];
                    }
                    for (int k = 0; k < pulse.Length; k++)
                    {
                        numbers[k + 96] = pulse[k];
                    }
                    for (int k = 0; k < oxygen.Length; k++)
                    {
                        numbers[k + 144] = oxygen[k];
                    }
                    for (int k = 0; k < cholesterol.Length; k++)
                    {
                        numbers[k + 192] = cholesterol[k];
                    }
                    for (int k = 0; k < bsg.Length; k++)
                    {
                        numbers[k + 288] = bsg[k];
                    }
                    udp.Connect("127.0.0.1", 10230);
                    udp.Send(numbers, numbers.Length);
                    continue;
                }
            }
        }
    }
}
