namespace Task_Manager.DTOs.StatisticsDtos;

public class StatisticsDto
{
    public int TotalUsers { get; set; }

    public int Admins { get; set; }

    public int Managers { get; set; }

    public int Employees { get; set; }

    public int TotalTasks { get; set; }

    public int NewTasks { get; set; }

    public int InProgressTasks { get; set; }

    public int IsOverdueTasks { get; set; }

    public int OnHoldTasks  { get; set; }

    public int CompletedTasks { get; set; }

    public int ClosedTasks { get; set; }

    public int CancelledTasks { get; set; }

    public int TotalComments { get; set; }

    public int DeletedComments { get; set; }

    public int TaskHistoryRecords { get; set; }

    public int AuditRecords { get; set; }
}
