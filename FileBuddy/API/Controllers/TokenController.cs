using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SharedResources.DataAccess.UserAccess;
using SharedResources.Services.TokenLogic;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private static readonly log4net.ILog Log =
             log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ITokenService _tokenService;
        private readonly IUserAccess _userAccessService;

        public TokenController()
        {
            _tokenService = new TokenService();
            _userAccessService = new UserAccess();
        }

        /// <summary>
        /// Returns a refreshed authentication token.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh/{accessToken}/{refreshToken}")]
        public IActionResult RefreshToken(string accessToken, string refreshToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var mailAddress = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value; 

            if(mailAddress == null)
            {
                Log.Error("Needed claim was not found. Token can not be refreshed.");
                return BadRequest();
            }

            var user = _userAccessService.LoadAllUsersFromDatabase().SingleOrDefault(u => u.MailAddress == mailAddress);
            if (user == null || user.RefreshToken != refreshToken)
            {
                Log.Error("Given user was not recognized. Unable to refresh token. ");
                return BadRequest();
            }

            var newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            _userAccessService.UpdateUserInformation(user);

            Log.Debug("Token was successfully refreshed.");
            return new ObjectResult(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }

        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult RevokeToken()
        {
            var username = User.Identity.Name;
            var user = _userAccessService.LoadAllUsersFromDatabase().SingleOrDefault(u => u.Name == username);

            if (user == null)
            {
                Log.Error("Given user was not recognized. Unable to revoke token. ");
                return BadRequest();
            }
            user.RefreshToken = null;
            _userAccessService.UpdateUserInformation(user);

            Log.Debug("Token was successfully revoked.");
            return NoContent();
        }
    }
}