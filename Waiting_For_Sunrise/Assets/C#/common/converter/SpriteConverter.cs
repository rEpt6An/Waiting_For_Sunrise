using UnityEngine;

namespace Assets.C_.common
{
    public class SpriteConverter
    {
        public static Sprite ConvertBytesToSprite(byte[] bytes)
        {
            // 创建Texture2D对象
            Texture2D texture = new Texture2D(2, 2);

            // 加载图像数据到Texture2D
            if (texture.LoadImage(bytes))
            {
                // 从Texture2D创建Sprite
                Sprite sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
                return sprite;
            }

            return null;
        }
    }
}