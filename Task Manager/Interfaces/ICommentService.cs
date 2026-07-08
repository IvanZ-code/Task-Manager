using Task_Manager.DTOs.CommentDtos;

namespace Task_Manager.Interfaces;

public interface ICommentService
{
    Task<CommentDto> AddComment(
       int taskId,
       CreateCommentDto dto,
       int userId,
       string role);

    Task<IEnumerable<CommentDto>> GetComments(
        int taskId,
        int userId,
        string role);

    Task<bool> UpdateComment(
        int taskId,
        int commentId,
        UpdateCommentDto dto,
        int userId,
        string role);

    Task<bool> DeleteComment(
        int taskId,
        int commentId,
        int userId,
        string role);
}
