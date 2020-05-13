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
            builder.Property(x => x.OwnerUserId).HasColumnName("owner_user_id").IsRequired();
            builder.Property(x => x.UploadDate).HasColumnName("upload_date").IsRequired();
        }
    }
}
