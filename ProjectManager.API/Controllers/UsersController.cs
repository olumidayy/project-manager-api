using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.API.Response;
using ProjectManager.Domain.DTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Supervisor;

namespace ProjectManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {

        private IUserRepository _userRepository;

        private CustomResponse NotFoundError = new CustomResponse(status: "error", message: "That user does not exist.");
        private CustomResponse UnauthorizedError = new CustomResponse(
            status: "error", 
            message: "You are not authorized for this action."
        );

        public UsersController(ProjectManagerSupervisor projectManagerSupervisor)
        {
            _userRepository = projectManagerSupervisor.UserRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var _currentUser = (User)HttpContext.Items["User"];
            if(_currentUser?.AccountType != AccountType.Admin)
            {
                return Unauthorized(UnauthorizedError);
            }
            var users = _userRepository.GetAll();
            return Ok(new CustomResponse(status: "success", message: "All users fetched.", data: users));
        }

        [HttpGet]
        [Route("{userId}")]
        public IActionResult GetUserById(int userId)
        {
            var _currentUser = (User)HttpContext.Items["User"];
            if(_currentUser.AccountType != AccountType.Admin && _currentUser.Id != userId)
            {
                return Unauthorized(UnauthorizedError);
            }
            var user = _userRepository.GetById(userId);
            if(user == null)
            {
                return NotFound(NotFoundError);
            }
            return Ok(new CustomResponse(status: "success", message: "User fetched.", data: user));
        }

        [HttpPost]
        [Route("send-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> SendOTP([FromBody] SendOtpDTO sendOtpDTO)
        {
            if(ModelState.IsValid)
            {
                var user =  _userRepository.GetByEmail(sendOtpDTO.Email);
                if(user != null)
                {
                    System.Console.WriteLine(user.Name);
                    await _userRepository.SendRecoveryEmail(user);
                    return Ok(new CustomResponse(status: "success", message: "OTP sent."));
                }
                return NotFound(NotFoundError);
            }
            return BadRequest(new CustomResponse(ModelState));
        }

        [HttpPost]
        [Route("confirm-otp")]
        [AllowAnonymous]
        public IActionResult ConfirmOTP([FromBody] ConfirmOtpDTO confirmOtpDTO)
        {
            if(ModelState.IsValid)
            {
                var user = _userRepository.GetByEmail(confirmOtpDTO.Email);
                if(user != null)
                {
                    bool isValid = _userRepository.ConfirmOTP(user, confirmOtpDTO.Otp);
                    return Ok(new CustomResponse(status: "success", message: "OTP confirmed."));
                }
                return NotFound(NotFoundError);
            }
            return BadRequest(new CustomResponse(ModelState));
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO userDTO)
        {
            var _currentUser = (User)HttpContext.Items["User"];
            if(ModelState.IsValid)
            {
                var userToUpdate = await _userRepository.Update(userDTO, _currentUser.Id);
                if(userToUpdate != null)
                    return Ok(new CustomResponse(status: "success", message: "User updated.", userToUpdate));
                return NotFound(NotFoundError);
            }
            return BadRequest(new CustomResponse(ModelState));
        }
        
        [HttpPatch]
        [Route("change-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            if(ModelState.IsValid)
            {
                var user = _userRepository.GetByEmail(changePasswordDTO.Email);
                if(user != null)
                {
                    user = await _userRepository.ChangePassword(user, changePasswordDTO.Otp, changePasswordDTO.NewPassword);
                    return Ok(new CustomResponse(status: "success", message: "Password changed."));
                }
                return NotFound(NotFoundError);
            }
            return BadRequest(new CustomResponse(ModelState));
        }

        [HttpDelete]
        [Route("delete/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var _currentUser = (User)HttpContext.Items["User"];
            if(_currentUser.AccountType != AccountType.Admin && _currentUser.Id != userId)
            {
                return Unauthorized(UnauthorizedError);
            }
            var userToDelete = await _userRepository.Delete(userId);
            if(userToDelete == null)
            {
                return NotFound(NotFoundError);
            }
            return Ok(new CustomResponse(status: "success", message: "User deleted.", userToDelete));
        }
    }
}