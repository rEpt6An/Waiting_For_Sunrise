using Assets.C_.item;

namespace Assets.C_.common
{
    public class EquipmentRegister : AbstractRegister<FileResource, Equipment>
    {
        // ✅ 必须修改：去掉 Assets/Resources/ 前缀和 .json 后缀
        // 统一路径分隔符为正斜杠 /
        private static readonly string EQUIPMENT_JSON_FILE_RESOURCE_PATH = "json/equipment";
        private static readonly string EQUIPMENT_JSON_FILE_RESOURCE_TYPE = "content";

        protected override FileResource GetFileResource()
        {
            return FileResourceFactory.CreateFileResource(EQUIPMENT_JSON_FILE_RESOURCE_PATH, EQUIPMENT_JSON_FILE_RESOURCE_TYPE);
        }

        protected override IResourceManager<FileResource, Equipment> GetResourceManager()
        {
            return EquipmentManager.GetInstance();
        }
    }
}