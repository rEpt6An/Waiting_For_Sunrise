using System;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace Assets.C_.common
{
    public class Item: JsonReadable<Item>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Rarity Rarity { get; set; }
        public string Description { get; set; }
        public Image Icon { get; set; }

        public Item Read(FileResource fileResource)
        {
            string jsonFileContent = fileResource.FileContent;
            if (jsonFileContent == null)
            {
                throw new ArgumentException("读取Json资源内容为空：" + fileResource.Path);
            }
            Item item = JsonConvert.DeserializeObject<Item>(jsonFileContent);
            return item;
        }
    }
}

