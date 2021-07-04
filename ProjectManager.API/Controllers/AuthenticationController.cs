using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectManager.API.Response;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.DTOs;
using BC = BCrypt.Net.BCrypt;
using ProjectManager.Domain.Supervisor;

namespace ProjectManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private IUserRepository _userRepository;
        private IAuthenticationRepository _authenticationRepository;

        public AuthenticationController(ProjectManagerSupervisor projectManagerSupervisor)
        {
            _authenticationRepository = projectManagerSupervisor.AuthenticationRepository;
            _userRepository = projectManagerSupervisor.UserRepository;
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
                if (_userRepository.GetByEmail(userDTO.Email) == null)
                {
                    var newUser = await _userRepository.Create(userDTO);
                    await _userRepository.SendWelcomeEmail(newUser);
                    return Ok(new CustomResponse(status: "success", message: "Registration", new
                    {
                        token = _authenticationRepository.GenerateJwtToken(newUser)
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
                var user = _userRepository.GetByEmail(userDTO.Email);
                if (user == null || !BC.Verify(userDTO.Password, user.Hash))
                {
                    return BadRequest(new CustomResponse(status: "error", message: "Invalid email or password."));
                }
                return Ok(new CustomResponse(
                    status: "success",
                    message: "Sign in successsful.",
                    data: new { token = _authenticationRepository.GenerateJwtToken(user) }
                ));
            }
            return BadRequest(new CustomResponse(ModelState));
        }
    }
}