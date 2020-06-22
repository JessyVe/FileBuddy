using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedResources.Dtos
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string MailAddress { get; set; }
        public DateTime AccountCreationDate { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string UserDevices { get; set; }
        public string UserGroups { get; set; }

        [NotMapped]
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
