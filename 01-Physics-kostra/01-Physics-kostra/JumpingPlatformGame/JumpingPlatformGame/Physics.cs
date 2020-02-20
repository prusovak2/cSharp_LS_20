namespace JumpingPlatformGame 
{
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

    public static class ExtensionDouble
    {
        public static MeterPerSeconds MeterPerSeconds(this double what)
        {
            return new MeterPerSeconds(what);
        }
        public static Seconds Seconds(this double what)
        {
            return new Seconds(what);
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