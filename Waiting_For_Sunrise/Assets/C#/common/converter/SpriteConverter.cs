using System;
using UnityEngine;

namespace Assets.C_.common
{
    public class SpriteConverter
    {
        public static Sprite ConvertBytesToSprite(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) throw new ArgumentNullException("bytes is Null");

            // 优化 1: 构造 Texture2D
            // 参数 3 (Default): TextureFormat.RGBA32 (保证透明通道)
            // 参数 4 (false): 关闭 Mipmaps (这对 UI Sprite 很重要，保证清晰)
            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);

            // 优化 2: 设置过滤模式
            // Point: 像素风游戏用; Bilinear: 普通高清图用
            texture.filterMode = FilterMode.Bilinear;

            // 加载图像数据
            // LoadImage 会自动调整 texture 的宽和高
            if (texture.LoadImage(bytes))
            {
                // 创建 Sprite
                Sprite sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f) // 中心锚点
                );

                // 建议：给 Sprite 命名，方便在 Inspector 调试
                sprite.name = "LoadedSprite_" + texture.width + "x" + texture.height;

                return sprite;
            }

            return null;
        }
    }
}