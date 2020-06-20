using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedRessources.Dtos;
using SharedRessources.Services.TokenLogic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private static readonly log4net.ILog Log =
             log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ITokenService _tokenService;

        public TokenController()
        {
            _tokenService = new TokenService();
        }

        /// <summary>
        /// Returns a refreshed authentification token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("refresh")]
        //public async Task<IActionResult> RefreshToken(string token, string refreshToken)
        //{
        //    var principal = _tokenService.GetPrincipalFromExpiredToken(token);
        //    var username = principal.Identity.Name; //this is mapped to the Name claim by default

        //    var user = _usersDb.Users.SingleOrDefault(u => u.Username == username);
        //    if (user == null || user.RefreshToken != refreshToken)
        //        return BadRequest();

        //    var newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
        //    var newRefreshToken = _tokenService.GenerateRefreshToken();

        //    user.RefreshToken = newRefreshToken;
        //    await _usersDb.SaveChangesAsync();

        //    return new ObjectResult(new
        //    {
        //        token = newJwtToken,
        //        refreshToken = newRefreshToken
        //    });
        //}

        ///// <summary>
        ///// Revokes to token of the user sending the request.
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[Authorize]
        //[Route("revoketoken")]
        //public async Task<IActionResult> Revoke()
        //{
        //    var username = User.Identity.Name;

        //    var user = _usersDb.Users.SingleOrDefault(u => u.Username == username);

        //    if (user == null)
        //        return BadRequest();

        //    user.RefreshToken = null;

        //    await _usersDb.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}