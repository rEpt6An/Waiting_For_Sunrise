using Assets.C_.player.player;

namespace Assets.C_.item
{
    public interface IEquipment : IPlayerStateChanger
    {
        int Id { get; }

        void ExecuteRemove(IPlayerState playState);
    }
}