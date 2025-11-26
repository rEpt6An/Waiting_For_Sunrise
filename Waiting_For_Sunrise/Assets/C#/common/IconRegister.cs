
using System;
using System.Collections.Generic;

namespace Assets.C_.common
{
    public class IconRegister: AbstractRegister
    {
        private static readonly List<string> ICON_FILE_RESOURCE_PATHS = new();
        private static readonly String ICON_FOLDER_PATH = "Assets/Resource/item/";

        static IconRegister()
        {
            // todo 向 ICON_FILE_RESOURCE_PATHS 添加图标位置
            for (int i = 1; i < 58; i++)
            {
                ICON_FILE_RESOURCE_PATHS.Add(ICON_FOLDER_PATH + i + ".png");
            }
        }

        protected override object GetFileResource()
        {
            List<FileResource> fileResources = new();
            foreach (string path in ICON_FILE_RESOURCE_PATHS)
            {
                FileResource fileResource = FileResourceFactory.CreateFileResource(path, "content");
                fileResources.Add(fileResource);
            }
            return fileResources;
        }

        protected override IResourceManager<object, object> GetResourceManager()
        {
            return (IResourceManager<object, object>) IconManager.GetInstance();
        }
    }
}