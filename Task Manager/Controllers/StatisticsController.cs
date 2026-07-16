using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Task_Manager.Interfaces;

namespace Task_Manager.Controllers;


[ApiController]
[Route("api/statistics")]
[Authorize(Roles = "Admin")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetStatistics()
    {
        var statistics = await _statisticsService.GetStatistics();

        return Ok(statistics);
    }
}
