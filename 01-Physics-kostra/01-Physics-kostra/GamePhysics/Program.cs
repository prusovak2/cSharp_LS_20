using System;

namespace GamePhysics {

	class Program {
		static void Main(string[] args) {
			var distance = 2.Meters();
			var time = 3.Seconds();
			var speed = distance / time;
			Console.WriteLine($"Speed: {speed}");
		}
	}
    public static class ExtensionsInt
    {
        public static Meters Meters(this int what)
        {
            return new Meters(what);
        }
        public static Seconds Seconds(this int what)
        {
            return new Seconds(what);
        }
        public static MeterPerSeconds MeterPerSeconds(this int what)
        {
            return new MeterPerSeconds(what);
        }
    }


    public struct Meters
    {
        public int Value;
        public Meters(int i)
        {
            this.Value = i;
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }
        public static MeterPerSeconds operator /(Meters meters, Seconds seconds)
        {
            return new MeterPerSeconds(meters.Value / seconds.Value);
        }
    }
    public struct Seconds
    {
        public double Value;
        public Seconds(double i)
        {
            this.Value = i;
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
    public struct MeterPerSeconds
    {
        public double Value;
        public MeterPerSeconds(double i)
        {
            this.Value = i;
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }

    }

}
