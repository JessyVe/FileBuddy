using System;

namespace SharedRessources.Dtos
{
    public class DownloadTransaction
    {
        public DateTime TransactionDate { get; set; }
        public string DowloadUserHashId { get; set; }
        public string FileHashId { get; set; }
    }
}
