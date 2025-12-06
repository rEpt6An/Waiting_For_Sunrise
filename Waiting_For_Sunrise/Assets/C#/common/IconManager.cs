using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.C_.common
{
    public class IconManager : IResourceManager<FileResources, Icon>
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
            Icon icon = ICONS[iconId];
            if (icon == null)
            {
                throw new ResourceNotFoundException("无法通过iconId查找到该图标，iconId：" + iconId);
            }
            return icon;
        }

        public void Load(FileResources fileResources) 
        {
            lock (_lock)
            {
                foreach (FileResource resource in fileResources.Resources)
                {
                    byte[] bytes = resource.Bytes;

                    // 调用转换
                    Sprite sprite = SpriteConverter.ConvertBytesToSprite(bytes);

                    // 安全检查：只有在 sprite 创建成功时才添加到列表
                    if (sprite != null)
                    {
                        Icon icon = new Icon(GetIconId(resource), sprite);
                        ICONS.Add(icon);
                    }
                    else
                    {
                        throw new Exception($"图片资源加载失败: ID {GetIconId(resource)}, FilePath {resource.Path}");
                    }
                }
            }
        }

        private static int GetIconId(FileResource fileResource)
        {
            Match match = Regex.Match(fileResource.Path, @"\d+");
            if (match.Success)
            {
                int number = int.Parse(match.Value);
                return number;
            }
            throw new ResourceNotFoundException("无法从路径中提取id");
        }

        public List<Icon> GetAll()
        {
            List<Icon> copy = new List<Icon>();
            copy.AddRange(ICONS);
            return copy;
        }
    }
}