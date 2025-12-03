using Shared.Enums;

namespace Data.Entities.Syncronisation
{
    public class SyncornisationEntity:AEntityBase
    {
        public string SyncronisationName { get; set; } = string.Empty;
        public SyncTypeEnum SyncronisationType { get; set; }
    }
}
