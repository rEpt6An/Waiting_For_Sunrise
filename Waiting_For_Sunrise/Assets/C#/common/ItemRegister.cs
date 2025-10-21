namespace Assets.C_.common
{
    public class ItemRegister: AbstractRegister
    {
        private static readonly string ITEM_JSON_FILE_RESOURCE_PATH = "D:\\develop\\UNITY\\projects\\Waiting_For_Sunrise\\Waiting_For_Sunrise\\Assets\\json\\Item.json";
        private static readonly string ITEM_JSON_FILE_RESOURCE_TYPE = "content";

        protected override object GetFileResource()
        {
            return FileResourceFactory.CreateFileResource(ITEM_JSON_FILE_RESOURCE_PATH, ITEM_JSON_FILE_RESOURCE_TYPE);
        }

        protected override IResourceManager<object, object> GetResourceManager()
        {
            return (IResourceManager<object, object>) ItemManager.GetInstance();
        }
    }
}