using SharedRessources.Dtos;

namespace SharedRessources.Services
{
    public class FileHashingEngine : HashingEngine
    {
        protected override string GenerateSourceString(IHashable hashable)
        {
            var sharedFile = hashable as SharedFile;

            // This combination can only appear ones; if a user tries to upload 
            // multiple files with the same filename the will be numbered.
            return $"{sharedFile.FileName}{sharedFile.UploadDate.ToString()}{sharedFile.OwnerUserId}";
        }
    }
}
