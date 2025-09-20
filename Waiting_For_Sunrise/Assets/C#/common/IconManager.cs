using System.Collections.Generic;

namespace Assets.C_.common
{
    public class IconManager : IIconManager
    {   
        private readonly List<Icon> ICONS = new();

        public Icon GetIcon(int iconId)
        {
            Icon icon = ICONS[iconId];
            if (icon == null)
            {
                throw new ItemException("无法通过iconId查找到该图标，iconId：" + iconId);
            }
            return icon;
        }
    }
}