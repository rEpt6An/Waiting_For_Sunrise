using Assets.C_.common.common;
using System.Collections.Generic;

namespace Assets.C_.player.bag
{
    public class PlayerToolbar: IPlayerToolbar
    {
        public PileOfItem[] Toolbar { get; private set; } = new PileOfItem[9];
        private readonly object _lock = new object();

        public void Add(int index, PileOfItem item)
        {
            lock (_lock)
            {
                if (Toolbar[index] == null) 
                {
                    Toolbar[index] = item;
                } else
                {
                    if (Toolbar[index].ItemId == item.ItemId)
                    {
                        Toolbar[index].Count += item.Count;
                    } else
                    {
                        throw new BagException("�ÿ�����Ѿ���ռ��");
                    }
                }
            }
        }

        public PileOfItem[] GetAll()
        {
            PileOfItem[] copy = new PileOfItem[9];
            for (int i = 0; i < Toolbar.Length; i++)
            {
                copy[i] = Toolbar[i];
            }
            return copy;
        }

        public PileOfItem GetPileOfItem(int index)
        {
            PileOfItem pileOfItem = Toolbar[index];
            return pileOfItem;
        }

        public PileOfItem Pop(int index, int num)
        {
            lock (_lock) 
            {
                int count = Toolbar[index].Count - num;
                if (count < 0)
                {
                    throw new BagException("��������");
                }
                Toolbar[index].Count = count;
                int itemId = Toolbar[index].ItemId;
                if (count == 0)
                {
                    Toolbar[index] = null;
                }
                return new PileOfItem(itemId, num);
            }
        }

        public int Count(int itemId)
        {
            lock (_lock)
            {
                int count = 0;
                for (int i = 0; i <= Toolbar.Length; i++)
                {
                    if (Toolbar[i].ItemId == itemId)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public void Delete(PileOfItem item)
        {
            lock (_lock)
            {
                if (Count(item.ItemId) < item.Count)
                {
                    throw new BagException("��Ʒ��������");
                }
                int nowNeedToDeleteCount = item.Count;
                for (int i = 0; i < Toolbar.Length; i++)
                {
                    if (item.ItemId == Toolbar[i].ItemId)
                    {
                        int num = Toolbar[i].Count;
                        if (num >= nowNeedToDeleteCount)
                        {
                            Toolbar[i].Count -= nowNeedToDeleteCount;
                            nowNeedToDeleteCount = 0;
                        }
                        else
                        {
                            Toolbar[i].Count = 0;
                            nowNeedToDeleteCount -= num;
                        }

                        if (Toolbar[i].Count == 0 && nowNeedToDeleteCount == 0)
                        {
                            Toolbar[i] = null;
                        }
                    }
                    if (nowNeedToDeleteCount == 0)
                    {
                        break;
                    }
                }
            }
        }
    }
}