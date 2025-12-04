using Assets.C_.item;

namespace Assets.C_.common
{
    public class EquipmentRegister : AbstractRegister<FileResource, Equipment>
    {
        private static readonly string EQUIPMENT_JSON_FILE_RESOURCE_PATH = "Assets\\Resources\\json\\equipment.json";
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