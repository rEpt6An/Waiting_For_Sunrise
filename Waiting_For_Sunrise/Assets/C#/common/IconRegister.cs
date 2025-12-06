
using System;
using System.Collections.Generic;

namespace Assets.C_.common
{
    public class IconRegister: AbstractRegister<FileResources, Icon>
    {
        private static readonly List<string> ICON_FILE_RESOURCE_PATHS = new();
        private static readonly String ICON_FOLDER_PATH = "Assets/Resource/item/";

        static IconRegister()
        {
            // todo 向 ICON_FILE_RESOURCE_PATHS 添加图标位置
            for (int i = 0; i < 58; i++)
            {
                ICON_FILE_RESOURCE_PATHS.Add(ICON_FOLDER_PATH + i + ".png");
            }
        }

        protected override FileResources GetFileResource()
        {
            List<FileResource> fileResources = new();
            foreach (string path in ICON_FILE_RESOURCE_PATHS)
            {
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