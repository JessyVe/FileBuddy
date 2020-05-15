using SharedRessources.Dtos;
using System;

namespace SharedRessources.DisplayedTypes
{
    public class DisplayedSharedFile 
    {
        public string SharedFileName { get; set; }
        public string ApiPath { get; set; }
        public string OwnerName { get; set; }
        public DateTime UploadDate { get; set; }

        public string Timestamp => GetTimeSinceSent();
        private string GetTimeSinceSent()
        {
            return "hello";
        }
    }
}
