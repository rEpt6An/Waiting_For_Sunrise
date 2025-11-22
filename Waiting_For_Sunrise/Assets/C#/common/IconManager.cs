using System.Collections.Generic;
using UnityEngine;

namespace Assets.C_.common
{
    public class IconManager : IResourceManager<List<FileResource>, Icon>
    {
        // 下标必须和id一样！！！
        private readonly List<Icon> ICONS = new();
        private static readonly object _lock = new();

        public static IconManager Instance { get; private set; }

        private IconManager()
        {

        }

        public static IconManager GetInstance()
        {
            if (Instance == null)
            {
                Instance = new IconManager();
            }
            return Instance;
        }

        public Icon Get(int iconId)
        {
            //现在无图标
            //Icon icon = ICONS[iconId];
            //if (icon == null)
            //{
            //    throw new ResourceNotFoundException("无法通过iconId查找到该图标，iconId：" + iconId);
            //}
            //return icon;
            return new Icon(iconId, null);
        }

        public void Load(List<FileResource> fileResources) 
        {
            lock (_lock)
            {
                foreach (FileResource resource in fileResources)
                {
                    byte[] bytes = resource.Bytes;
                    Sprite sprite = SpriteConverter.ConvertBytesToSprite(bytes);
                    Icon icon = new Icon(GetIconId(resource), sprite);
                    ICONS.Add(icon);
                }
            }
        }

        private static int GetIconId(FileResource fileResource)
        {
            // todo
            return 1;
        }

        public List<Icon> GetAll()
        {
            List<Icon> copy = new List<Icon>();
            copy.AddRange(ICONS);
            return copy;
        }
    }
}