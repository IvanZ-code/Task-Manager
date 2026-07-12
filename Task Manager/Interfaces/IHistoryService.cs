using Task_Manager.DTOs.HistoryDtos;
using Task_Manager.Models;

namespace Task_Manager.Interfaces;

public interface IHistoryService
{
    Task AddRecord(
        int taskId,
        int userId,
        HistoryAction action,
        string? description = null,
        string? metadata = null);

    Task<IEnumerable<HistoryDto>> GetHistory(
    int taskId);
}
