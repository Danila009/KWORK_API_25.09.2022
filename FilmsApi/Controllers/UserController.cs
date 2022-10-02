using FastestDeliveryApi.database;
using FilmsApi.DTOs.User;
using FilmsApi.Model.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreAppApi.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FilmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private EfModel _efModel;

        public UserController(EfModel model)
        {
            _efModel = model;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            User user = await _efModel.Users.FindAsync(idUser);

            if (user == null)
                return NotFound();

            return new UserDTO
            {
                Id = user.Id,
                Login = user.Login,
                Username = user.Username,
                Subscription = user.Subscription
            };
        }

        [Authorize]
        [HttpPatch("Subscription")]
        public async Task<ActionResult> PostSubscription(Boolean subscription)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return NotFound();

            int idUser = Convert.ToInt32(identity.FindFirst("Id").Value);

            User user = await _efModel.Users.FindAsync(idUser);

            if (user == null)
                return NotFound();

            user.Subscription = subscription;
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "ADMIN_USER")]
        [HttpPost("{id}/Admin")]
        public async Task<ActionResult> PostAdmin(int id)
        {
            User user = await _efModel.Users.FindAsync(id);

            if(user == null)
                return NotFound();

            _efModel.Users.Remove(user);
            _efModel.AdminUsers.Add(new AdminUser {
                Id = user.Id,
                Username = user.Username,
                Login = user.Login,
                Password = user.Password,
                Subscription = user.Subscription
            });

            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("Admin")]
        public async Task<ActionResult<List<AdminUser>>> GetUsersAdmin()
        {
            return await _efModel.AdminUsers.ToListAsync();
        }

        [Authorize(Roles = "ADMIN_USER")]
        [HttpGet("/api/Users")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            return await _efModel.Users.ToListAsync();
        }

        [HttpPost("Registration")]
        public async Task<ActionResult<RegistrationResultDTO>> Registration(
            RegistrationDTO registrationDTO
            )
        {
            if (_efModel.Users.Any(u => u.Login == registrationDTO.Login))
                return new RegistrationResultDTO
                {
                    Error = "Пользователь с таким login уже существует"
                };

            if (registrationDTO.Login.Length < 6 || registrationDTO.Password.Length < 6)
                return new RegistrationResultDTO
                {
                    Error = "" +
                    "Login должен состоять из 6 или больше символов \n" +
                    "Password должен состоять из 6 или больше символов \n"
                };

            _efModel.Users.Add(new User
            {
                Username = registrationDTO.Username,
                Login = registrationDTO.Login,
                Password = registrationDTO.Password,
                Subscription = false
            });
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Authorization")]
        public ActionResult<object> Token(AuthorizationDTO authorization)
        {
            var indentity = GetIdentity(authorization.Login, authorization.Password);

            if (indentity == null)
            {
                return BadRequest();
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    audience: TokenBaseOptions.AUDIENCE,
                    issuer: TokenBaseOptions.ISSUER,
                    notBefore: now,
                    claims: indentity.Claims,
                    expires: now.Add(TimeSpan.FromDays(TokenBaseOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(
                        TokenBaseOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = indentity.Name,
                role = indentity.FindFirst(ClaimsIdentity.DefaultRoleClaimType).Value
            };

            return response;
        }

        [NonAction]
        public ClaimsIdentity GetIdentity(string login, string password)
        {
            User user = _efModel.Users.FirstOrDefault(
                x => x.Login == login && x.Password == password
                );

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
                    new Claim("Id", user.Id.ToString())
                };

                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }
            return null;
        }
    }
}
