using DevFreela.API.DTOs;
using DevFreela.Application.DTOs;

namespace DevFreela.Application.Services;

public interface IProjectService
{
    ResultViewModel<List<ProjectItemViewModel>> GetAll(string search, int page, int size);
    ResultViewModel<ProjectViewModel> GetById(int id);
    ResultViewModel<int> Insert(CreateProjectDTO model);
    ResultViewModel Update(int id, UpdateProjectDTO model);
    ResultViewModel Delete(int id);
    ResultViewModel Start(int id);
    ResultViewModel Complete(int id);
    ResultViewModel InsertComment(int id, CreateProjectCommentDTO model);

}