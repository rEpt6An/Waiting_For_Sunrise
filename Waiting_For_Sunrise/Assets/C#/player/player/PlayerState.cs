using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.C_.player.player
{
    class PlayerState
    {
        //当前生命值
        public int bloodPoint { get;private set; }
        //经验值
        public int experiencePoint { get;private set; }
        //伤害倍率
        public int damageMultiplerPoint { get;private  set; }
        //最大生命值
        public int HP { get; private set; }
        //防御力
        public int defensivePower { get; private set; }
        //闪避
        public int dodge { get; private set; }
        //命中率
        public int hitRate { get; private set; }
        //范围
        public int scope { get; private set; }
        //速度
        public int speed { get; private set; }
        //暴击率
        public int criticalChance { get; private set; }
        //暴击倍率（暴击伤害）
        public int criticalDamage { get; private set; }
        //吸血
        public int leech { get; private set; }
        //自愈
        public int selfHealing { get; private set; }
        //幸运
        public int lucky { get; private set; }
        //收获
        public int harvest { get; private set; }

        Blood blood = new Blood();
        Experience experience = new Experience();
        DamageMultipler damageMultipler = new DamageMultipler();
        public PlayerState()
        {
            this.bloodPoint = blood.blood;
            this.experiencePoint = experience.CurrentExperiencePoints;
            this.damageMultiplerPoint = damageMultipler.damageMultipler;
            this.HP = blood.HP;
        }

        public void changeDefensivePower(int changePoint)
        {
            defensivePower += changePoint;
        }
        public void changeDodge(int changePoint)
        {
            dodge += changePoint;
            if (dodge >= 90)
            {
                dodge = 90;
            }
        }
        public void changeHitRate(int changePoint)
        {
            hitRate += changePoint;
        }

        public void changeScope(int changePoint)
        {
            scope += changePoint;
        }
        public void changeSpeed(int changePoint)
        {
            speed += changePoint;
        }
        public void changeCriticalChance(int changePoint)
        {
            criticalChance += changePoint;
        }
        public void changeCriticalDamage(int changePoint)
        {
            criticalDamage += changePoint;
        }
        public void changeLeech(int changePoint)
        {
            leech += changePoint;
        }
        public void changeSelfHealing(int changePoint)
        {
            selfHealing += changePoint;
        }
        public void changeLucky(int changePoint)
        {
            lucky += changePoint;
        }
        public void changeHarvest(int changePoint)
        {
            harvest += changePoint;
        }
    }
}
