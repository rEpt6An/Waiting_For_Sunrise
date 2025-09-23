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

        public static byte[] ReadBytes(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("�ļ�·������Ϊ�ջ�հ��ַ���", nameof(filePath));
            }
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"�ļ�������: {filePath}");
                return null;
            }
            try
            {
                return File.ReadAllBytes(filePath);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"û��Ȩ�޷����ļ�: {filePath}\n{ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"��ȡ�ļ�ʱ����IO����: {filePath}\n{ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"δ֪����: {ex.Message}");
            }
            return null;
        }
    }
}