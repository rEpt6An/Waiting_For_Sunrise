using UnityEngine.Playables;

namespace Asset.C_.player.executor
{
    public interface IPlayerEffect
    {
        void Execute(PlayState playState);
    }
}