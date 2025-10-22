
using System;
using System.IO;

namespace Assets.C_.common
{
    public class FileResource
    {
        public string Path {  get; private set; }
        public string FileContent { get; private set; } = null;
        public byte[] Bytes { get; private set; } = null;

        // content or byte
        public string Type { get; private set; }

        public FileResource(string path, string type)
        {
            ValidChecker.CheckIsValidWindowsPath(path);
            this.Path = path;
            this.Type = type;
        }

        public void Init()
        {
            if (Type == "byte")
            {
                LoadBytes();
            } 
            else if (Type == "content")
            {
                LoadContent();
            }
        }

        private string LoadContent()
        {
            if (this.FileContent == null)
            {
                this.FileContent = SystemFileReader.ReadContext(Path);
                if (this.FileContent == null)
                {
                    throw new InvalidDataException($"文件内容为空：{Path}");
                }
            }
            return this.FileContent;
        }

        private byte[] LoadBytes()
        {
            this.Bytes = SystemFileReader.ReadBytes(Path);
            if (this.Bytes == null)
            {
                throw new InvalidDataException($"文件内容为空：{Path}");
            }
            return this.Bytes;
        }
    }
}