using System;

namespace SharedRessources.Dtos
{
    public class SharedFile
    {
        public int Id { get; set; }
        public string SharedFileName { get; set; }
        public string ApiPath { get; set; }
        public int OwnerUserId { get; set; }
        public string UploadDate { get; set; }
    }
}
