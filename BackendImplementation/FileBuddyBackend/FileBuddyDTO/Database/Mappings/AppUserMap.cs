using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedRessources.Dtos;

namespace DatabaseConnection.Database.Mappings
{
    public class AppUserMap 
    {
        public AppUserMap(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.MailAddress).HasColumnName("mail_address").IsRequired();
            builder.Property(x => x.AccountCreationDate).HasColumnName("account_creation_date").IsRequired();
            builder.Property(x => x.ProfilPicture).HasColumnName("profil_picture");

            builder.Property(x => x.UserDevices).HasColumnName("user_devices");
            builder.Property(x => x.UserGroups).HasColumnName("user_groups");
        }
    }
}
