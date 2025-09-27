using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.C_.player.player
{
    class Experience:IExperience
    {
        public int CurrentExperiencePoints { get; private set; }

        public void ChangeExperience(int changeExperience)
        {
            CurrentExperiencePoints = CurrentExperiencePoints + changeExperience;
        }

        public void Init()
        {
            CurrentExperiencePoints = 0;
        }
        public Experience()
        {
            Init();
        }
    }
}
