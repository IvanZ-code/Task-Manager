using System.ComponentModel.DataAnnotations;

namespace Task_Manager.DTOs.CommentDtos;

public class UpdateCommentDto
{

    [Required(ErrorMessage = "Comment is required")]
    [StringLength(255, MinimumLength = 1,
        ErrorMessage = "Comment must contain 1 to 255 characters")]
    public string Text { get; set; } = string.Empty;
}
