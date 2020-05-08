using SharedRessources.Dtos;

namespace SharedRessources.DataAccess.Authentification
{
    // https://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
    // https://www.example-code.com/csharp/firebase_jwt_authentication.asp
    public static class TokenGenerator
    {
        public static AuthentificationToken GenerateAuthentificationToken()
        {
            return new AuthentificationToken()
            {
                AccessToken = GenerateAccessToken(),
                RefreshToken = GenerateRefreshToken()
            };
        }

        private static string GenerateAccessToken()
        {
            return string.Empty;
        }

        private static string GenerateRefreshToken()
        {
            return string.Empty;
        }
    }
}
