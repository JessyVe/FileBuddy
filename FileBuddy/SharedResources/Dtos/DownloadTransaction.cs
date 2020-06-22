using System;

namespace SharedResources.Dtos
{
    public class DownloadTransaction
    {
        public int Id { get; set; }
        public int ReceiverUserId { get; set; }
        public int SharedFileId { get; set; }
        public DateTime DownloadDate { get; set; }
    }
}
