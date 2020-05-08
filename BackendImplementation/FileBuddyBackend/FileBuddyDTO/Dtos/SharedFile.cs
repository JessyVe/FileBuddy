using System;
using System.Collections.Generic;

namespace SharedRessources.Dtos
{
    public class SharedFile : IHashable
    {
        public string FileName { get; set; }
        public DateTime UploadDate { get; set; }
        public string OwnerUserId { get; set; }
        public string HashId { get; set; }

        public string APIPath { get; set; }
        public IList<string> AuthorizedAccessGrantedTo { get; set; } // list of user hashes being allow to download to file next to the owner
    }
}