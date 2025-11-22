
namespace Assets.C_.player
{
    public class LifeStealEvent
    {
        public int LifeStealValue {  get; set; }
        public string target { get; set; }

        public LifeStealEvent(int lifeStealValue, string target)
        {
            LifeStealValue = lifeStealValue;
            this.target = target;
        }
    }
}