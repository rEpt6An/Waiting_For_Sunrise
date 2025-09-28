
using System;
using System.Collections.Generic;

namespace Assets.C_.common
{
    public class IconRegister: IRegister
    {
        private static readonly List<string> ICON_FILE_RESOURCE_PATHS = new();

        static IconRegister()
        {
            // todo 向 ICON_FILE_RESOURCE_PATHS 添加图标位置
        }

        public void Register()
        {
            List<FileResource> fileResources = new();
            foreach (string path in ICON_FILE_RESOURCE_PATHS)
            {
                FileResource fileResource = FileResourceFactory.CreateFileResource(path, "content");
                fileResources.Add(fileResource);
            }
            IconManager iconManager = IconManager.GetInstance();
            iconManager.Load(fileResources);
        }
    }
}