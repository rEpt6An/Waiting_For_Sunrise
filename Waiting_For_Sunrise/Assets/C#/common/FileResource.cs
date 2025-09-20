
namespace Assets.C_.common
{
    public class FileResource
    {
        public string Path {  get; set; }
        public string FileContent { get; set; } = null;

        public FileResource(string path)
        {
            ValidChecker.CheckIsValidWindowsPath(path);
            this.Path = path;
        }

        public void Init()
        {
            ReadFileContent();
        }

        public string ReadFileContent()
        {
            if (this.FileContent == null)
            {
                this.FileContent = SystemFileReader.Read(Path);
            }
            return this.FileContent;
        }
    }
}