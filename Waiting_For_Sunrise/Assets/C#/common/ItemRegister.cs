namespace Assets.C_.common
{
    public class ItemRegister: IRegister
    {
        private static readonly string ITEM_JSON_FILE_RESOURCE_PATH = "D:\\develop\\UNITY\\projects\\Waiting_For_Sunrise\\Waiting_For_Sunrise\\Assets\\json\\Item.json";
        private static readonly string ITEM_JSON_FILE_RESOURCE_TYPE = "content";

        public void Register()
        {
            FileResource fileResource = FileResourceFactory.CreateFileResource(ITEM_JSON_FILE_RESOURCE_PATH, ITEM_JSON_FILE_RESOURCE_TYPE);
            ItemManager itemManager = ItemManager.Instance;
            itemManager.Load(fileResource);
        }
    }
}