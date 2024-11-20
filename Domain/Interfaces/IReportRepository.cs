using Domain.DTOs.General;
using Domain.DTOs.Security.Report;
using Domain.Entities.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IReportRepository
    {
        List<DisplayChartVM>? GetCountUsers(AreaVM model);
        List<DisplayChartVM>? GetCountRoles(DisplayReportVM model);
        List<DisplayChartVM>? GetCountGenders(AreaVM model);
        List<DisplayChartVM>? GetCountEmoloyments(DisplayReportVM model);
        List<DisplayChartVM>? GetCountEducations(DisplayReportVM model);
        DisplayHistoriesUsersVM? CalculateMinHistory(DisplayReportVM model);
        DisplayHistoriesUsersVM? CalculateMaxHistory(DisplayReportVM model);
        HourlyWorkVM? CalculateHourlyWork(DisplayReportVM model);

    }
}