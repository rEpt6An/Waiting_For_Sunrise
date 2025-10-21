
using System;
using System.Collections.Generic;

namespace Assets.C_.common
{
    public class IconRegister: AbstractRegister
    {
        private static readonly List<string> ICON_FILE_RESOURCE_PATHS = new();

        static IconRegister()
        {
            // todo �� ICON_FILE_RESOURCE_PATHS ���ͼ��λ��
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
            return (IResourceManager<object, object>)IconManager.GetInstance();
        }
    }
}