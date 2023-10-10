using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net;
using TestTaskUser.DTO;
using TestTaskUser.Helpers;
using TestTaskUser.Models;
using TestTaskUser.Services;

namespace TestTaskUser.Controllers
{
    /// <summary>
    ///  Класс для запросов пользователей
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение списка всех пользователей (роли: User, Admin, Support, SuperAdmin)
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="parameters">Параметры для отображения</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Доступ запрещен</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [ProducesResponseType(typeof(List<UserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [HttpPost("GetAllUsers")]
        [Authorize(Roles = "User, Admin, Support, SuperAdmin")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers([FromBody] ParameterPack parameters)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                throw new ArgumentException(messages);
            }
            var users = await _userService.ReadAll(parameters);
            var usersDto = _mapper.Map<List<UserDto>>(users);

            return Ok(usersDto);
        }

        /// <summary>
        /// Получение пользователя по id (роли: User, Admin, Support, SuperAdmin)
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">Id пользователя</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Некорректные параметры запроса</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Доступ запрещен</response>
        /// <response code="404">Запрашиваемый ресурс не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [HttpGet("GetUser/{id}")]
        [Authorize(Roles = "User, Admin, Support, SuperAdmin")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _userService.Read(id);
            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        /// <summary>
        /// Добавление пользователю роли (роли: SuperAdmin)
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="userId">Id пользователя</param>
        /// <param name="roleId">Id роли</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Некорректные параметры запроса</response>
        /// <response code="404">Запрашиваемый ресурс не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [ProducesResponseType(typeof(List<UserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [HttpPut("AddRole/{userId}/{roleId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<List<UserDto>>> AddRole(int userId, int roleId)
        {
            var users = await _userService.AddRole(userId, roleId);
            var usersDto = _mapper.Map<List<UserDto>>(users);

            return Ok(usersDto);
        }

        /// <summary>
        /// Изменение информации о пользователе по его id (роли: User, Admin, Support, SuperAdmin)
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="userId">Id пользователя</param>
        /// <param name="userDto">Пользователь</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Некорректные параметры запроса</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Доступ запрещен</response>
        /// <response code="404">Запрашиваемый ресурс не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [ProducesResponseType(typeof(List<UserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [HttpPut("UpdateUserInfo/{userId}")]
        [Authorize(Roles = "User, Admin, Support, SuperAdmin")]
        public async Task<ActionResult<List<UserDto>>> UpdateUserInfo(int userId, [FromBody] UserInfoDto userDto)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                throw new ArgumentException(messages);
            }
            var user = _mapper.Map<User>(userDto);
            var users = await _userService.Update(userId, user);
            var usersDto = _mapper.Map<List<UserDto>>(users);

            return Ok(usersDto);
        }

        /// <summary>
        /// Удаление пользователя по его id (роли: Admin, SuperAdmin)
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="id">Id пользователя</param>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Некорректные параметры запроса</response>
        /// <response code="401">Пользователь не авторизован</response>
        /// <response code="403">Доступ запрещен</response>
        /// <response code="404">Запрашиваемый ресурс не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
        [ProducesResponseType(typeof(List<UserDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        [HttpGet("DeleteUser/{id}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<ActionResult<UserDto>> DeleteUser(int id)
        {
            var users = await _userService.Delete(id);
            var usersDto = _mapper.Map<List<UserDto>>(users);

            return Ok(usersDto);
        }
    }
}
