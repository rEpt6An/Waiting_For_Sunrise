namespace Assets.C_.player
{
    public class PlayerGetDamageContext
    {
        public string Source { get; set; }
        public int value {  get; set; }
        public int defensivePower { get; set; }
        public double dodge { get; set; }

        public PlayerGetDamageContext(string source, int value, int defensivePower, double dodge)
        {
            Source = source;
            this.value = value;
            this.defensivePower = defensivePower;
            this.dodge = dodge;
        }
    }
}