using System;

namespace SharedResources.DisplayedTypes
{
    public class DisplayedSharedFile 
    {
        public int Id { get; set; }
        public string SharedFileName { get; set; }
        public string ApiPath { get; set; }
        public string OwnerName { get; set; }
        public DateTime UploadDate { get; set; }

        public string Timestamp => GetTimeSinceSent();

        private string GetTimeSinceSent()
        {
            var minutesSinceSend = (int)(DateTime.Now - UploadDate).TotalMinutes;
            
            if(minutesSinceSend <= 1)
            {
                return "Just now";
            } else if(minutesSinceSend <= 59)
            {
                return $"{minutesSinceSend}min";
            }
            else if (minutesSinceSend < 1440)
            {
                return $"{minutesSinceSend / 60}h";
            }
            else if (minutesSinceSend < 10080)
            {
                return $"{minutesSinceSend / 60 / 24}days";
            }
            return UploadDate.ToString("yyyy-MM-dd H:mm");
        }
    }
}
