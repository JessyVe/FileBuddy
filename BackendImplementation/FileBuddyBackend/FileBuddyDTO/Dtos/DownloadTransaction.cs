using System;

namespace SharedRessources.Dtos
{
    public class DownloadTransaction
    {
        public DateTime TransactionDate { get; set; }
        public string UserHashId { get; set; }
        public string FileHashId { get; set; }
    }
}
