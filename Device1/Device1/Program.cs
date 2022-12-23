using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        public int systolicBP;
        public int diastolicBP;
        public int heartRate;
        //public int timeStamp;
        public Type ti;
        public override string WriteMeasurment()
        {
            string s = "Systolic blood pressure is " + systolicBP + ", while diastolic blood pressure is " + diastolicBP + ". Heart rate of the patient is " + heartRate + "\n";
            return s;
        }
        public Sphygmomanometer(int sphID = 0, int systolicBP = 120, int diastolicBP = 80, int heartRate = 70)
        {
            this.sphID = sphID;
            this.systolicBP = systolicBP;
            this.diastolicBP = diastolicBP;
            this.heartRate = heartRate;
            ti = Type.Digital;
        }
        public override void MakeMeasurment(int ts)
        {
            Random sbp = new Random();
            Random dbp = new Random();
            Random hr = new Random();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Systolic blood pressure is " + sbp.Next(100, 200) + ", while diastolic blood pressure is " + dbp.Next(60, 90) + ". Heart rate of the patient is " + hr.Next(60, 120));
                Thread.Sleep(ts * 1000);

            }
        }
    }

    public class Glucometer : Device
    {
        public int glucID;
        public double blood_sugar;
        //public int timeStamp;
        public Type ti;
        public override string WriteMeasurment()
        {
            string str = "Blood sugar level of the patient is " + blood_sugar + "mg/dL\n";
            return str;
        }
        public override void MakeMeasurment(int ts)
        {
            Random bs = new Random(180);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Blood sugar of the patient is " + 20*bs.NextDouble() + "mg/dL");
                Thread.Sleep(ts * 1000);
            }
            Thread.Sleep(ts * 1000);

        }
        public Glucometer(int glucID = 1, double blood_sugar = 20)
        {
            this.glucID = glucID;
            this.blood_sugar = blood_sugar;
            this.ti = Type.Analog;
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Sphygmomanometer s = new Sphygmomanometer();
            string str = s.WriteMeasurment();
            Console.WriteLine(str);
            s.MakeMeasurment(1);
            Glucometer gm = new Glucometer();
            string str1 = gm.WriteMeasurment();
            Console.WriteLine(str1);
            gm.MakeMeasurment(1);
            Console.ReadLine();
        }
    }
}
