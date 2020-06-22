using System.Collections.Generic;

namespace SharedResources.Dtos
{
    public class UserGroup
    {
        public string GroupName { get; set; }
        public IList<int> UserIds { get; set; }
    }
}
