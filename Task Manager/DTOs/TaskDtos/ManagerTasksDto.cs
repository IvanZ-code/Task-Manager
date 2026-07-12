namespace Task_Manager.DTOs.TaskDtos;

public class ManagerTasksDto
{
    public TaskGroupsDto CreatedTasks { get; set; } = new();
    public TaskGroupsDto AssignedTasks { get; set; } = new();
}
