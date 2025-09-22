namespace Assets.C_.common
{
    public class FileResourceFactory
    {
        public static FileResource CreateFileResource(string path, string type)
        {
            FileResource fileResource = new(path, type);
            fileResource.Init();
            return fileResource;
        }
    }
}