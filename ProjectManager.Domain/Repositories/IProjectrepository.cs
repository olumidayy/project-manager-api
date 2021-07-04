using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManager.Domain.DTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Repositories
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetAll();
        Project GetById(int projectId);
        IEnumerable<Project> GetByUser(int id);
        Task<Project> Create(ProjectDTO projectDTO, int userId);
        Task<Project> Update(UpdateProjectDTO projectDTO, int userId);
        Task<Project> Delete(int projectId);
    }
}