using Task_Manager.DTOs.TaskDtos;

namespace Task_Manager.Interfaces;

public interface ITaskService
{
    Task<TaskDto> CreateTask(
        CreateTaskDto dto,
        int managerId
    );


    Task<IEnumerable<TaskDto>> GetEmployeeTasks(
        int employeeId
    );


    Task<ManagerTasksDto> GetManagerTasks(
        int managerId
    );


    Task<IEnumerable<TaskDto>> GetAllTasks();


    Task<bool> UpdateStatus(
        int taskId,
        UpdateTaskStatusDto dto,
        int userId,
        string role
    );
}
