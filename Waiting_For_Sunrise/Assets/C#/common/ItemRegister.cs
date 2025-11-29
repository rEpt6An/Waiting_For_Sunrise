namespace Assets.C_.common
{
    public class ItemRegister: AbstractRegister<FileResource, Item>
    {
        private static readonly string ITEM_JSON_FILE_RESOURCE_PATH = "Assets/json/Item.json";
        private static readonly string ITEM_JSON_FILE_RESOURCE_TYPE = "content";

        protected override FileResource GetFileResource()
        {
            return FileResourceFactory.CreateFileResource(ITEM_JSON_FILE_RESOURCE_PATH, ITEM_JSON_FILE_RESOURCE_TYPE);
        }

        protected override IResourceManager<FileResource, Item> GetResourceManager()
        {
            return ItemManager.GetInstance();
        }
    }
}