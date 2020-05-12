using System;

namespace SharedRessources.Dtos
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string MailAddress { get; set; }
        public DateTime AccountCreationDate { get; set; }
        public byte[] ProfilPicture { get; set; }
        public string UserDevices { get; set; }
        public string UserGroups { get; set; }
    }
}
