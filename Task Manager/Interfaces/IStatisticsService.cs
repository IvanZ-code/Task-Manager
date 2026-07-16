using Task_Manager.DTOs.StatisticsDtos;

namespace Task_Manager.Interfaces;

public interface IStatisticsService
{
    Task<StatisticsDto> GetStatistics();
}
