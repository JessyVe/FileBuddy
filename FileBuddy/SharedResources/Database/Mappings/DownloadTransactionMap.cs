using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedResources.Dtos;

namespace SharedResources.Database.Mappings
{
    public class DownloadTransactionMap
    {
        public DownloadTransactionMap(EntityTypeBuilder<DownloadTransaction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ReceiverUserId).HasColumnName("receiver_user_id").IsRequired();
            builder.Property(x => x.SharedFileId).HasColumnName("shared_file_id").IsRequired();
            builder.Property(x => x.DownloadDate).HasColumnName("download_date").IsRequired();
        }
    }
}
