using System.Threading.Tasks;
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
    public class ProjectsController : ControllerBase
    {

        private IProjectRepository _projectRepository;
        private IUserRepository _userRepository;

        private CustomResponse NotFoundError = new CustomResponse(status: "error", message: "That project does not exist.");
        private CustomResponse UnauthorizedError = new CustomResponse(
            status: "error",
            message: "You are not authorized for this action."
        );

        public ProjectsController(ProjectManagerSupervisor projectManagerSupervisor)
        {
            _projectRepository = projectManagerSupervisor.ProjectRepository;
            _userRepository = projectManagerSupervisor.UserRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var _currentUser = (User)HttpContext.Items["User"];
            if (_currentUser.AccountType != AccountType.Admin)
            {
                return Unauthorized(UnauthorizedError);
            }
            var projects = _projectRepository.GetAll();
            return Ok(new CustomResponse(status: "success", message: "All projects fetched.", projects));
        }

        [HttpGet]
        [Route("user/{id}")]
        public IActionResult GetByUser(int userId)
        {
            var _currentUser = (User)HttpContext.Items["User"];
            if (_currentUser.AccountType != AccountType.Admin && _currentUser.Id != userId)
            {
                return Unauthorized(UnauthorizedError);
            }
            var projects = _projectRepository.GetByUser(userId);
            return Ok(new CustomResponse(status: "success", message: "Projects fetched.", projects));
        }

        [HttpGet]
        [Route("{projectId}")]
        public IActionResult GetProjectById(int projectId)
        {

            var project = _projectRepository.GetById(projectId);
            if (project == null)
            {
                return NotFound(NotFoundError);
            }
            var _currentUser = (User)HttpContext.Items["User"];
            if (_currentUser.AccountType != AccountType.Admin && _currentUser.Id != project.OwnerId)
            {
                return Unauthorized(UnauthorizedError);
            }
            return Ok(new CustomResponse(status: "success", message: "Project fetched.", project));
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDTO projectDTO)
        {
            var _currentUser = (User)HttpContext.Items["User"];
            if (ModelState.IsValid)
            {
                var newProject = await _projectRepository.Create(projectDTO, _currentUser.Id);
                return Ok(new CustomResponse(status: "success", message: "Project created.", newProject));
            }
            return BadRequest(new CustomResponse(ModelState));
        }


        [HttpPut]
        [Route("update/{projectId}")]
        public async Task<IActionResult> UpdateProject([FromBody]UpdateProjectDTO projectDTO, int projectId)
        {
            var _currentUser = (User)HttpContext.Items["User"];
            if (ModelState.IsValid)
            {
                var project = _projectRepository.GetById(projectId);
                if(project == null) return BadRequest(new CustomResponse(status: "error", message: "Invalid project ID"));
                if (_currentUser.Id != project.OwnerId) return Unauthorized(UnauthorizedError);
                var updatedProject = await _projectRepository.Update(projectDTO, projectId);
                if (updatedProject == null) return NotFound(NotFoundError);
                return Ok(new CustomResponse(status: "success", message: "Project updated", updatedProject));
            }
            return BadRequest(new CustomResponse(ModelState));
        }

        [HttpDelete]
        [Route("delete/{projectId}")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            var _currentUser = (User)HttpContext.Items["User"];
            var project = _projectRepository.GetById(projectId);
            if ( _currentUser.Id != project.OwnerId) return Unauthorized(UnauthorizedError);
            var deletedProject = await _projectRepository.Delete(projectId);
            if (deletedProject == null)  return NotFound(NotFoundError);
            return Ok(new CustomResponse(message: "Project deleted", status: "success", data: deletedProject));
        }
    }
}