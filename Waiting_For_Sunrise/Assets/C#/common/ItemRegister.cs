namespace Assets.C_.common
{
    public class ItemRegister : AbstractRegister<FileResource, Item>
    {
        // ✅ 必须修改：Resources.Load 不需要前缀 Assets/Resources/ 和后缀 .json
        // 逻辑路径即为：json/Item
        private static readonly string ITEM_JSON_FILE_RESOURCE_PATH = "json/Item";
        private static readonly string ITEM_JSON_FILE_RESOURCE_TYPE = "content";

        protected override FileResource GetFileResource()
        {
            // 调用工厂，底层会通过 Resources.Load<TextAsset>("json/Item") 加载
            return FileResourceFactory.CreateFileResource(ITEM_JSON_FILE_RESOURCE_PATH, ITEM_JSON_FILE_RESOURCE_TYPE);
        }

        protected override IResourceManager<FileResource, Item> GetResourceManager()
        {
            return ItemManager.GetInstance();
        }
    }
}