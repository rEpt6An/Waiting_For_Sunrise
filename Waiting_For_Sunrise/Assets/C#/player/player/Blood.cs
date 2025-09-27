namespace Assets.C_.player.player
{
	public class Blood:IBlood{
        //当前血量
		public int blood { get;private  set; }
        //最大生命值
        public int HP { get;private  set; }
        //更改当前生命值
		public void ChangeBlood(int changeBlood)
        {
            //判断当前生命值更改后是否小于零
            if (blood + changeBlood <= 0)
            {
                blood = 0;
                return;
            }
            //判断当前生命值更改后是否大于最大生命值s
            if (blood + changeBlood >= HP)
            {
                blood = HP;
                return;
            }
			blood = blood + changeBlood;
        }
        //初始化最大生命值
		public void InitBlood()
        {
            HP = 100;
                      
        }
		public  Blood()
        {
            blood = HP;
        }
        //判断当前生命值是否为小于等于0
		public bool isEmpty()
        {
            if (blood <= 0)
            {
                return true;
            }
            return false;
        }
        //更改最大生命值
        public void ChangeHP(int changeHP)
        {
            HP = HP + changeHP;
        }
	}
}