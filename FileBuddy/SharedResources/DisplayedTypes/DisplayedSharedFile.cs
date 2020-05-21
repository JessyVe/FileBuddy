using System;
using System.Diagnostics.CodeAnalysis;

namespace SharedRessources.DisplayedTypes
{
    public class DisplayedSharedFile : IComparable<DisplayedSharedFile>, IEquatable<DisplayedSharedFile>
    {
        public string SharedFileName { get; set; }
        public string ApiPath { get; set; }
        public string OwnerName { get; set; }
        public DateTime UploadDate { get; set; }

        public string Timestamp => GetTimeSinceSent();

        public int CompareTo([AllowNull] DisplayedSharedFile other)
        {
            if(Timestamp == other.Timestamp) 
                return 0;

            return Timestamp.CompareTo(other.Timestamp);
        }

        public bool Equals(DisplayedSharedFile other)
        {
            if (Timestamp.Equals(other.Timestamp) && OwnerName.Equals(other.OwnerName) && ApiPath.Equals(other.ApiPath)) 
                return true;

            return false;
        }

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
