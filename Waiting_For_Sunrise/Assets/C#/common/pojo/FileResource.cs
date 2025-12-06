using System;
using System.IO;

namespace Assets.C_.common
{
    public class FileResource
    {
        public string Path { get; private set; }
        public string FileContent { get; private set; } = null;
        public byte[] Bytes { get; private set; } = null;

        // content or byte
        public string Type { get; private set; }

        // zy: Constructor for injecting content directly (e.g., from Unity)
        public FileResource(string content)
        {
            this.FileContent = content;
            this.Type = "content";
            this.Path = "Injected_From_Unity";
        }

        // Constructor for loading from disk path
        public FileResource(string path, string type)
        {
            // 假设 ValidChecker.CheckIsValidWindowsPath(path) 是一个静态校验方法
            // 如果你没有这个类，需要替换成自己的路径校验逻辑
            // ValidChecker.CheckIsValidWindowsPath(path); 

            this.Path = path;
            this.Type = type.ToLower(); // 统一转为小写，确保判断正确
        }

        // 统一初始化方法，根据 Type 加载内容或字节
        public void Init()
        {
            if (Path == "Injected_From_Unity") return; // 注入的内容不需要初始化

            if (Type == "byte")
            {
                LoadBytes();
            }
            else if (Type == "content")
            {
                LoadContent();
            }
            else
            {
                throw new NotSupportedException($"不支持的文件资源类型: {Type}");
            }
        }

        // ----------------------------------------------------
        // Private Loader Methods
        // ----------------------------------------------------

        private string LoadContent()
        {
            if (this.FileContent == null)
            {
                try
                {
                    // **假设 SystemFileReader.ReadContext(Path) 存在且返回 string**
                    this.FileContent = SystemFileReader.ReadContext(Path);
                }
                catch (Exception ex) // 捕获文件读取可能出现的异常
                {
                    throw new InvalidDataException($"无法读取文件内容: {Path}. 错误: {ex.Message}", ex);
                }

                if (string.IsNullOrEmpty(this.FileContent))
                {
                    throw new InvalidDataException($"文件内容为空：{Path}");
                }
            }
            return this.FileContent;
        }

        /**
         * 🌟 改进的 LoadBytes 方法
         * 使用 System.IO.File.ReadAllBytes() 确保兼容性，并增加 try-catch 提高健壮性。
         */
        private byte[] LoadBytes()
        {
            if (this.Bytes == null)
            {
                try
                {
                    // 使用 System.IO.File.ReadAllBytes() 直接读取所有字节
                    this.Bytes = File.ReadAllBytes(Path);
                }
                catch (FileNotFoundException)
                {
                    throw new InvalidDataException($"文件不存在: {Path}");
                }
                catch (UnauthorizedAccessException)
                {
                    throw new InvalidDataException($"没有访问权限: {Path}");
                }
                catch (Exception ex)
                {
                    // 捕获其他如 IO 错误、路径太长等问题
                    throw new InvalidDataException($"读取字节文件失败: {Path}. 错误: {ex.Message}", ex);
                }

                // 检查读取到的字节数组是否为空
                if (this.Bytes == null || this.Bytes.Length == 0)
                {
                    throw new InvalidDataException($"文件内容为空或无法读取字节：{Path}");
                }
            }
            return this.Bytes;
        }
    }
}