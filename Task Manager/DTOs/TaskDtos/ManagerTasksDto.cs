namespace Task_Manager.DTOs.TaskDtos;

public class ManagerTasksDto
{
    public IEnumerable<TaskDto> CreatedTasks { get; set; } = [];
    public IEnumerable<TaskDto> AssignedTasks { get; set; } = [];
}
