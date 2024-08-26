namespace DevFreela.API.DTOs;

public record CreateProjectCommentDTO(string Content, int IdProject, int IdUser);