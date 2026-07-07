using Task_Manager.Models;

namespace Task_Manager.DTOs.TaskDtos
{
    public class UpdateTaskStatusDto
    {
        public TaskState Status { get; set; }
    }
}
