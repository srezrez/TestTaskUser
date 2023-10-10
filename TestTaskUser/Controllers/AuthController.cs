using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using TestTaskUser.DTO;
using TestTaskUser.Models;
using TestTaskUser.Services;

namespace TestTaskUser.Controllers
{
    /// <summary>
    ///  Класс для авторизации и аутентификации
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IMapper mapper, IConfiguration configuration)
        {
            _userService = userService;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        /// Добавление нового пользователя / Регистрация
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="userDto">Пользователь</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="404">Запрашиваемый ресурс не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [ProducesResponseType(typeof(List<UserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [HttpPost("register")]
        public async Task<ActionResult<List<UserDto>>> Register([FromBody] UserInfoDto userDto)
        {
            if(!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                throw new ArgumentException(messages);
            }
            var user = _mapper.Map<User>(userDto);
            var users = await _userService.Create(user, userDto.Password);
            var usersDto = _mapper.Map<List<UserDto>>(users);

            return Ok(usersDto);
        }

        /// <summary>
        /// Вход в личный кабинет
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="userLoginDto">Пользователь</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Некорректные параметры запроса</response>
        /// <response code="404">Запрашиваемый ресурс не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [ProducesResponseType(typeof(List<UserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                throw new ArgumentException(messages);
            }
            var user = await _userService.Login(userLoginDto.Email, userLoginDto.Password);
            var userDto = _mapper.Map<UserDto>(user);
            var jwt = CreateToken(userDto);

            return Ok(jwt);
        }

        private string CreateToken(UserDto user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };
            var claimRoles = user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name));
            claims.AddRange(claimRoles);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:TokenKey").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
