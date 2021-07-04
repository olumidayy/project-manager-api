using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Domain.DTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;

namespace ProjectManager.Data.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApiDbContext _context;
        public ProjectRepository(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<Project> Create(ProjectDTO projectDTO, int userId)
        {
            var newProject = new Project() {
                Description = projectDTO.Description,
                LiveLink = projectDTO.LiveLink,
                Name = projectDTO.Name,
                OwnerId = userId,
                sourceCodeLink = projectDTO.SourceCodeLink,
                Technologies = projectDTO.Technologies
            };
            await _context.Projects.AddAsync(newProject);
            await _context.SaveChangesAsync();
            return newProject;
        }

        public IEnumerable<Project> GetAll()
        {
            return _context.Projects;
        }

        public Project GetById(int id)
        {
            return _context.Projects.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<Project> GetByUser(int id)
        {
            return _context.Projects.Where(p => p.OwnerId == id);
        }

        public async Task<Project> Update(UpdateProjectDTO projectDTO, int id)
        {
            var projectToUpdate = _context.Projects.FirstOrDefault(u => u.Id == id);
            if(projectToUpdate != null)
            {
                projectToUpdate.Description = projectDTO.Description;
                projectToUpdate.LiveLink = projectDTO.LiveLink;
                projectToUpdate.Name = projectDTO.Name;
                projectToUpdate.sourceCodeLink = projectDTO.SourceCodeLink;
                projectToUpdate.Technologies = projectDTO.Technologies;
                await _context.SaveChangesAsync();
            }
            return projectToUpdate;
        }

        public async Task<Project> Delete(int id)
        {
            var projectToDelete = _context.Projects.FirstOrDefault(u => u.Id == id);
            _context.Projects.Remove(projectToDelete);
            await _context.SaveChangesAsync();
            return projectToDelete;
        }
    }
}