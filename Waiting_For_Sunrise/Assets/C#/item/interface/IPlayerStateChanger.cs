
using Assets.C_.player.player;

namespace Assets.C_.item
{
    public interface IPlayerStateChanger
    {
        void Execute(IPlayerState playState);
    }
}