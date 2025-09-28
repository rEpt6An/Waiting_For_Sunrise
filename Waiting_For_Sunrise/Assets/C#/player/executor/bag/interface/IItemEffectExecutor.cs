namespace Asset.C_.player.executor
{
    public interface IItemEffectExecutor
    {
        ItemEffect OnObtaining(int id, int num);

        ItemEffect OnLosing(int id, int num);
    }
}