using UnityEngine.UI;

namespace Assets.C_.common
{
    public class Icon
    {
        public int Id { get; set; }
        public Image Image { get; set; } = null;

        public Icon(int id, Image image)
        {
            Id = id;
            Image = image;
        }
    }
}