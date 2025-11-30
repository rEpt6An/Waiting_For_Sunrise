using System;
using System.IO;

namespace Assets.C_.common
{
    public class ValidChecker
    {
        public static void CheckIsValidWindowsPath(string path)
        {
            if (!IsValidWindowsPath(path))
            {
                throw new ArgumentException("这个资源路径有误: "+path+". :<");
            }
        }

        public static bool IsValidWindowsPath(string path)
        {/*
            if (string.IsNullOrWhiteSpace(path))
                return false;

            try {
                string fullPath = Path.GetFullPath(path);
                return Path.IsPathRooted(path) &&
                       path.IndexOfAny(Path.GetInvalidPathChars()) == -1;
            }
            catch {
                return false;
            }*/
            return true;
        }

    }
}