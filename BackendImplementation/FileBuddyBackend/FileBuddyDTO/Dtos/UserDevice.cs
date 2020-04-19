using System.Security;

namespace SharedRessources.Dtos
{
    public class UserDevice
    {
        public string Name { get; set; }
        public SecureString MacAddress { get; set; }
    }
}
