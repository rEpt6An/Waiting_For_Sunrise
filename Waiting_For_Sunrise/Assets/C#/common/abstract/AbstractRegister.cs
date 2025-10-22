namespace Assets.C_.common
{
    public abstract class AbstractRegister : IRegister
    {
        protected abstract object GetFileResource();

        protected abstract IResourceManager<object, object> GetResourceManager();

        public void Register()
        {
            // ob อ๊มห
            object fileResource = GetFileResource();
            IResourceManager<object, object> resourceManager = GetResourceManager();
            resourceManager.Load(fileResource);
        }
    }
}