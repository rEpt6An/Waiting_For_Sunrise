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
                Console.WriteLine($"�ļ���Ŀ¼������: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"��Ȩ�޷����ļ�: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"IO����: {ex.Message}");
            }
            if (String.IsNullOrEmpty(content))
            {
                Console.WriteLine($"��ȡ�ļ�Ϊ�հף�·��: {filePath}");
            }
            return content;
        }
    }
}