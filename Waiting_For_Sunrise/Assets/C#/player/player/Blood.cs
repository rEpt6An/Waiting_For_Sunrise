namespace Assets.C_.player.player
{
	public class Blood:IBlood{
        //��ǰѪ��
		public int blood { get;private  set; }
        //�������ֵ
        public int HP { get;private  set; }
        //���ĵ�ǰ����ֵ
		public void ChangeBlood(int changeBlood)
        {
            //�жϵ�ǰ����ֵ���ĺ��Ƿ�С����
            if (blood + changeBlood <= 0)
            {
                blood = 0;
                return;
            }
            //�жϵ�ǰ����ֵ���ĺ��Ƿ�����������ֵs
            if (blood + changeBlood >= HP)
            {
                blood = HP;
                return;
            }
			blood = blood + changeBlood;
        }
        //��ʼ���������ֵ
		public void InitBlood()
        {
            HP = 100;
                      
        }
		public  Blood()
        {
            blood = HP;
        }
        //�жϵ�ǰ����ֵ�Ƿ�ΪС�ڵ���0
		public bool isEmpty()
        {
            if (blood <= 0)
            {
                return true;
            }
            return false;
        }
        //�����������ֵ
        public void ChangeHP(int changeHP)
        {
            HP = HP + changeHP;
        }
	}
}