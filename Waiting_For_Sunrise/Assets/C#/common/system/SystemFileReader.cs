using System;
using System.IO;

namespace Assets.C_.common
{
    public class SystemFileReader
    {
        public static string ReadContext(string filePath)
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

        public static byte[] ReadBytes(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("文件路径不能为空或空白字符串", nameof(filePath));
            }
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"文件不存在: {filePath}");
                return null;
            }
            try
            {
                return File.ReadAllBytes(filePath);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"没有权限访问文件: {filePath}\n{ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"读取文件时发生IO错误: {filePath}\n{ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"未知错误: {ex.Message}");
            }
            return null;
        }
    }
}