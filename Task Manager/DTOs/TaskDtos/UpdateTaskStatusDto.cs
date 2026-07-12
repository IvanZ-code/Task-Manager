using System.ComponentModel.DataAnnotations;
using Task_Manager.Enums;

namespace Task_Manager.DTOs.TaskDtos
{
    public class UpdateTaskStatusDto
    {
        [Required(ErrorMessage = "Status is required")]
        [EnumDataType(typeof(TaskState), ErrorMessage = "Incorrect status value")]
        public TaskState Status { get; set; }
    }
}
