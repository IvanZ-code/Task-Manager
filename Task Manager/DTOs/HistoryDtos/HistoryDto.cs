using Task_Manager.Enums;

namespace Task_Manager.DTOs.HistoryDtos;

public class HistoryDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public string User { get; set; } = string.Empty;

    public HistoryAction Action { get; set; } 

    public string? Description { get; set; }

    public string? Metadata { get; set; }
}
