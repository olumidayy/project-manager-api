using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.ApplicationCore.Entities;
using ProjectManager.ApplicationCore.Entities.DTOs;
using ProjectManager.ApplicationCore.Enums;
using ProjectManager.ApplicationCore.Services;
using ProjectManager.Common;

namespace ProjectManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {

        private IUserService _userService;

        private CustomResponse NotFoundError = new CustomResponse(status: "error", message: "That user does not exist.");
        private CustomResponse UnauthorizedError = new CustomResponse(
            status: "error", 
            message: "You are not authorized for this action."
        );

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var _currentUser = (User)HttpContext.Items["User"];
            if(_currentUser?.AccountType != AccountType.Admin)
            {
                return Unauthorized(UnauthorizedError);
            }
            var users = _userService.GetAll();
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
            var user = _userService.GetById(userId);
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
                var user =  _userService.GetByEmail(sendOtpDTO.Email);
                if(user != null)
                {
                    await _userService.SendRecoveryEmail(user);
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
                var user = _userService.GetByEmail(confirmOtpDTO.Email);
                if(user != null)
                {
                    bool isValid = _userService.ConfirmOTP(user, confirmOtpDTO.Otp);
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
                var userToUpdate = await _userService.Update(userDTO, _currentUser.Id);
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
                var user = _userService.GetByEmail(changePasswordDTO.Email);
                if(user != null)
                {
                    user = await _userService.ChangePassword(user, changePasswordDTO.Otp, changePasswordDTO.NewPassword);
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
            var userToDelete = await _userService.Delete(userId);
            if(userToDelete == null)
            {
                return NotFound(NotFoundError);
            }
            return Ok(new CustomResponse(status: "success", message: "User deleted.", userToDelete));
        }
    }
}