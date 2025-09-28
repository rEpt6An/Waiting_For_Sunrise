

using System.Collections.Generic;

namespace Assets.C_.common
{
    public interface IResourceManager<I, O>
    {
        O Get(int id);

        List<O> GetAll();

        void Load(I i);
    }
}