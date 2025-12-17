using System;
using System.Collections.Generic;

namespace Assets.C_.common
{
    public class IconRegister : AbstractRegister<FileResources, Icon>
    {
        private static readonly List<string> ICON_FILE_RESOURCE_PATHS = new();

        // ✅ 修正：Resources.Load 不需要 "Assets/Resources/" 前缀
        // 确保你的实际路径是 Assets/Resources/item/
        private static readonly string ICON_FOLDER_PATH = "item/";

        static IconRegister()
        {
            // 清空列表以防万一（虽然 static 只执行一次）
            ICON_FILE_RESOURCE_PATHS.Clear();

            // 生成逻辑路径
            for (int i = 0; i < 58; i++)
            {
                // ✅ 修正：Resources.Load 不能带文件后缀名 (.png)
                // 结果应该是 "item/0", "item/1" ...
                ICON_FILE_RESOURCE_PATHS.Add(ICON_FOLDER_PATH + i);
            }
        }

        protected override FileResources GetFileResource()
        {
            List<FileResource> fileResources = new();
            foreach (string path in ICON_FILE_RESOURCE_PATHS)
            {
                // 调用工厂，此时传入的是逻辑路径 "item/0"
                FileResource fileResource = FileResourceFactory.CreateFileResource(path, "byte");
                fileResources.Add(fileResource);
            }
            FileResources fileResources1 = new FileResources();
            fileResources1.Resources = fileResources;
            return fileResources1;
        }

        protected override IResourceManager<FileResources, Icon> GetResourceManager()
        {
            return IconManager.GetInstance();
        }
    }
}