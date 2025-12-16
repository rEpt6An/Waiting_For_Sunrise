using Assets.C_.bus;
using Assets.C_.player.player;
using Assets.C_.item;
using System.Collections.Generic; // 引入 List
using Assets.C_.player.bag;      // 假设 WeaponData 在这个命名空间

namespace Assets.C_.player
{
    public class Player : AbstractAttackable
    {
        private static readonly Player Instance = new();

        public static Player GetInstance() { return Instance; }

        public IPlayerState PlayerState { get; private set; }

        public IPlayerAsset PlayerAsset { get; private set; }

        // ⭐️ 核心修正：用于跨场景持久化保存玩家的武器列表
        public List<WeaponData> GlobalWeaponArsenal { get; set; } = new List<WeaponData>();


        public Player()
        {
            PlayerState = new PlayerState();
            PlayerAsset = new PlayerAsset();
        }

        private static void PublishPlayerGetDamageEvent(Damage actualDamage, Damage orignDamage)
        {
            EventBus.Publish(new PlayerGetDamageEvent(actualDamage, orignDamage));
        }

        protected override int GetDefensivePower()
        {
            // throw new System.NotImplementedException(); // 保持现有逻辑
            return 0;
        }

        protected override double GetDodge()
        {
            // throw new System.NotImplementedException(); // 保持现有逻辑
            return 0.0;
        }

        protected override void ChangeBlood(int value)
        {
            // throw new System.NotImplementedException(); // 保持现有逻辑
        }
    }
}