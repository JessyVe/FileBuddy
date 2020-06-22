using System;

namespace SharedResources.Dtos
{
    public class SharedFile
    {
        public int Id { get; set; }
        public string SharedFileName { get; set; }
        public string ApiPath { get; set; }
        public int OwnerUserId { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
