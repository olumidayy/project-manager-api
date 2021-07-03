using BC = BCrypt.Net.BCrypt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.ApplicationCore.Entities.DTOs;
using ProjectManager.Common;
using ProjectManager.ApplicationCore.Services;
using Microsoft.AspNetCore.Authorization;

namespace ProjectManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private IUserService _userService;
        private IAuthenticationService _authenticationService;

        public AuthenticationController(IUserService userService, IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO userDTO)
        {
            
            if (ModelState.IsValid)
            {
                if(userDTO.Password != userDTO.ConfirmPassword)
                {
                    return BadRequest(new CustomResponse(status: "error", message: "Passwords do not match."));
                }
                if (_userService.GetByEmail(userDTO.Email) == null)
                {
                    var newUser = await _userService.Create(userDTO);
                    await _userService.SendWelcomeEmail(newUser);
                    return Ok(new CustomResponse(status: "success", message: "Registration", new
                    {
                        token = _authenticationService.GenerateJwtToken(newUser)
                    }));
                }
                return BadRequest(new CustomResponse(status: "error", message: "Account already exists."));
            }
            return BadRequest(new CustomResponse(ModelState));
        }

        [HttpPost]
        [Route("Sign-In")]
        public IActionResult SignIn([FromBody] SignInUserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetByEmail(userDTO.Email);
                if (user == null || !BC.Verify(userDTO.Password, user.Hash))
                {
                    return BadRequest(new CustomResponse(status: "error", message: "Invalid email or password."));
                }
                return Ok(new CustomResponse(
                    status: "success",
                    message: "Sign in successsful.",
                    data: new { token = _authenticationService.GenerateJwtToken(user) }
                ));
            }
            return BadRequest(new CustomResponse(ModelState));
        }
    }
}