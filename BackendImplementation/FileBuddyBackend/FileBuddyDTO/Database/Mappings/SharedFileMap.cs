using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedRessources.Dtos;

namespace DatabaseConnection.Database.Mappings
{
    public class SharedFileMap
    {
        public SharedFileMap(EntityTypeBuilder<SharedFile> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.SharedFileName).HasColumnName("shared_file_name").IsRequired();
            builder.Property(x => x.ApiPath).HasColumnName("api_path").IsRequired();
            builder.Property(x => x.FileTransactionId).HasColumnName("file_transaction_id").IsRequired();
        }
    }
}
