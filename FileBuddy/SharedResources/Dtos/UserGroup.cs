using System.Collections.Generic;

namespace SharedRessources.Dtos
{
    public class UserGroup
    {
        public string GroupName { get; set; }
        public IList<int> UserIds { get; set; }
    }
}
