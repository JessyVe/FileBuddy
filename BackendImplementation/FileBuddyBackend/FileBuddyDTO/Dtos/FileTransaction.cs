using System;

namespace SharedRessources.Dtos
{
    public class FileTransaction
    {
        public int Id { get; set; }
        public int SenderUserId { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
