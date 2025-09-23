using UnityEngine;

namespace Assets.C_.common
{
    public class SpriteConverter
    {
        public static Sprite ConvertBytesToSprite(byte[] bytes)
        {
            // ����Texture2D����
            Texture2D texture = new Texture2D(2, 2);

            // ����ͼ�����ݵ�Texture2D
            if (texture.LoadImage(bytes))
            {
                // ��Texture2D����Sprite
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