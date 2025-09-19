using System;
using System.IO;

namespace Assets.C_.common
{
    public class SystemFileReader
    {
        public static string Read(string filePath)
        {
            string content = null;
            try
            {
                content = File.ReadAllText(filePath);
            }
            catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
            {
                Console.WriteLine($"文件或目录不存在: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"无权限访问文件: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"IO错误: {ex.Message}");
            }
            if (String.IsNullOrEmpty(content))
            {
                Console.WriteLine($"读取文件为空白，路径: {filePath}");
            }
            return content;
        }
    }
}