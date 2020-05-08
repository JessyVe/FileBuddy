using System;
using System.Collections.Generic;

namespace SharedRessources.Dtos
{
    public class FullUserData : IHashable, IComparable<FullUserData>
    {       
        public int Seed { get; set; }
        public string Name { get; set; }
        public string Password { get; set; } // TODO: Change to SecureString
        public string MailAddress { get; set; } // TODO: Change to SecureString
        public string ProfilePicture { get; set; } // TODO: How do you do this right?; Download image and save local path

        public IList<string> UserDevices { get; set; }
        public DateTime AccountCreationDate { get; set; }


        public string HashId { get; set; }

        public int CompareTo(FullUserData user)
        {
            return HashId.CompareTo(user.HashId);
        }
    }
}
