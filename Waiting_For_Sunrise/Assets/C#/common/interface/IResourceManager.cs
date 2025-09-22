

namespace Assets.C_.common
{
    public interface IResourceManager<I, O>
    {
        O Get(int id);

        void Load(I i);
    }
}