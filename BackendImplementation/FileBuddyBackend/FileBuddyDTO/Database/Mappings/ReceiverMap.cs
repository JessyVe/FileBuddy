using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedRessources.Dtos;

namespace DatabaseConnection.Database.Mappings
{
    public class ReceiverMap
    {
        public ReceiverMap(EntityTypeBuilder<Receiver> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ReceiverUserId).HasColumnName("receiver_user_id").IsRequired();
            builder.Property(x => x.FileTransactionId).HasColumnName("file_transaction_id").IsRequired();
        }
    }
}
