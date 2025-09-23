using Assets.C_.common.common;

namespace Assets.C_.player.bag
{
    public interface IPlayerToolbar
    {
        PileOfItem GetPileOfItem(int index);

        // �õ����Ǹ���
        PileOfItem[] GetAll();

        PileOfItem Pop(int index, int num);

        void Delete(PileOfItem item);

        void Add(int index, PileOfItem item);

        int Count(int itemId);
    }
}