using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedRessources.Dtos;

namespace DatabaseConnection.Database.Mappings
{
    public class FileTransactionMap
    {
        public FileTransactionMap(EntityTypeBuilder<FileTransaction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.SenderUserId).HasColumnName("sender_user_id").IsRequired();
            builder.Property(x => x.TransactionDate).HasColumnName("transaction_date").IsRequired();
        }
    }
}
