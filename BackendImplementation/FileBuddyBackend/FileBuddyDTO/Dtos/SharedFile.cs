using System;

namespace SharedRessources.Dtos
{
    public class SharedFile : IHashable
    {
        public string FileName { get; set; }
        public DateTime UploadDate { get; set; }
        public string OwnerUserId { get; set; }
        public string HashId { get; set; }
    }
}