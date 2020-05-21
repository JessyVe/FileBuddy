using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedRessources.Dtos;

namespace SharedRessources.Database.Mappings
{
    public class AuthorizedAccessMap
    {
        public AuthorizedAccessMap(EntityTypeBuilder<AuthorizedAccess> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired();
            builder.Property(x => x.SharedFileId).HasColumnName("shared_file_id").IsRequired();
        }
    }
}
