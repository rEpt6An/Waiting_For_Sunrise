namespace Assets.C_.common
{
    public abstract class AbstractRegister<I, O> : IRegister
    {
        protected abstract I GetFileResource();

        protected abstract IResourceManager<I, O> GetResourceManager();

        public void Register()
        {
            I fileResource = GetFileResource();
            IResourceManager<I, O> resourceManager = GetResourceManager();
            resourceManager.Load(fileResource);
        }
    }
}