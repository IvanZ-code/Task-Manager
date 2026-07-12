namespace Task_Manager.DTOs.TaskDtos;

public class TaskGroupsDto
{
    public IEnumerable<TaskDto> Closed { get; set; } = [];
    public IEnumerable<TaskDto> Cancelled { get; set; } = [];
    public IEnumerable<TaskDto> Active { get; set; } = [];
}
