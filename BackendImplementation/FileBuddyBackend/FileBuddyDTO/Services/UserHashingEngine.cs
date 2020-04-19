using SharedRessources.Dtos;

namespace SharedRessources.Services
{
    public class UserHashingEngine : HashingEngine
    {
        protected override string GenerateSourceString(IHashable hashable)
        {
            var user = hashable as User;

            if(user.Seed == default)
                  user.Seed = _random.Next();

            return $"{user.Name}{user.AccountCreationDate}{user.Seed}";
        }
    }
}
