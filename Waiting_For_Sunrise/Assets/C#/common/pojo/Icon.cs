using UnityEngine;

namespace Assets.C_.common
{
    public class Icon
    {
        public int Id { get; private set; }
        public Sprite Image { get; set; } = null;

        public Icon(int id, Sprite image)
        {
            Id = id;
            Image = image;
        }
    }
}