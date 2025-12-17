using System;
using UnityEngine; // 必须引入 Unity 引擎命名空间

namespace Assets.C_.common
{
    public class FileResource
    {
        public string Path { get; private set; }
        public string FileContent { get; private set; } = null;
        public byte[] Bytes { get; private set; } = null;
        public string Type { get; private set; }

        public FileResource(string content)
        {
            this.FileContent = content;
            this.Type = "content";
            this.Path = "Injected_From_Unity";
        }

        public FileResource(string path, string type)
        {
            // 🚨 重要：这里接收到的 path 应该是逻辑路径，例如 "item/0"
            this.Path = path;
            this.Type = type.ToLower();
        }

        public void Init()
        {
            if (Path == "Injected_From_Unity") return;

            if (Type == "byte")
            {
                LoadBytesFromResources();
            }
            else if (Type == "content")
            {
                LoadContentFromResources();
            }
            else
            {
                throw new NotSupportedException($"不支持的文件资源类型: {Type}");
            }
        }

        private void LoadContentFromResources()
        {
            if (this.FileContent == null)
            {
                // 使用 Unity 的 Resources 加载文本
                TextAsset asset = Resources.Load<TextAsset>(Path);
                if (asset != null)
                {
                    this.FileContent = asset.text;
                }
                else
                {
                    Debug.LogError($"[FileResource] 文本加载失败: {Path}");
                }
            }
        }

        private void LoadBytesFromResources()
        {
            if (this.Bytes == null)
            {
                // 1. 先尝试按 TextAsset 加载（用于读取 .bytes 或 .json 的原始字节）
                TextAsset asset = Resources.Load<TextAsset>(Path);
                if (asset != null)
                {
                    this.Bytes = asset.bytes;
                    return;
                }

                // 2. 如果是图片，按 Texture2D 加载并转换
                Texture2D tex = Resources.Load<Texture2D>(Path);
                if (tex != null)
                {
                    // 🚨 注意：这要求图片勾选了 "Read/Write Enabled"
                    this.Bytes = tex.EncodeToPNG();
                    return;
                }

                Debug.LogError($"[FileResource] 字节加载失败: {Path}");
            }
        }
    }
}