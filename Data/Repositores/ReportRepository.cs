using AutoMapper;
using Data.Context;
using Data.Extensions;
using Domain.DTOs.General;
using Domain.DTOs.Security.Report;
using Domain.DTOs.Security.User;
using Domain.Entities.Security.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositores
{
    public class ReportRepository : IReportRepository
    {
        #region Constructor
        private readonly HRMContext _context;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public ReportRepository(HRMContext context,
            IMapper mapper,
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository)
        {
            _context = context;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        #endregion


        public List<DisplayChartVM>? GetCountUsers(AreaVM model)
        {
            if (model == null)
                return null;

            var context = _userRepository.GetUsersQuery(model);

            List<DisplayChartVM> result = new();

            var active = context.Where(u => u.IsActived)
                                .Count();

            result.Add(new DisplayChartVM { ChartTitle = "active", ChartValue = active });



            var disable = context.Where(u => !u.IsActived)
                                 .Count();

            result.Add(new DisplayChartVM { ChartTitle = "disable", ChartValue = disable });

            return result;
        }

        public List<DisplayChartVM>? GetCountRoles(DisplayReportVM model)
        {
            if (model == null)
                return null;

            var context = _context.UserRoles.Include(ur => ur.User)
                                             .Include(ur => ur.Role)
                                             .Include(ur => ur.User.Department)
                                             .IgnoreQueryFilters()
                                             .Where(ur => ur.User.Department.Area == model.Section &&
                                                          ur.User.Department.Province == model.Province &&
                                                          ur.User.Department.County == model.County &&
                                                          ur.User.Department.District == model.District)
                                             .AsQueryable();
            var result = new List<DisplayChartVM>();

            var titles = context.Select(r => r.Role.Title).Distinct().ToList();

            for (int i = 0; i < titles.Count; i++)
            {
                int count = context.Where(ur => ur.Role.Title == titles[i])
                                   .Count();

                result.Add(new DisplayChartVM { ChartTitle = titles[i], ChartValue = count });
            }

            return result;
        }

        public List<DisplayChartVM>? GetCountEmoloyments(DisplayReportVM model)
        {
            if (model == null)
                return null;

            var context = _context.Users.Where(u => u.Department.Area == model.Section &&
                                                  u.Department.Province == model.Province &&
                                                  u.Department.County == model.County &&
                                                  u.Department.District == model.District &&
                                                  u.IsActived)
                                        .ToList();

            List<DisplayChartVM> result = new();

            var titles = context.Select(u => u.Employment).Distinct().ToList();

            for (int i = 0; i < titles.Count; i++)
            {
                int count = context.Where(u => u.Employment == titles[i])
                                   .Count();

                result.Add(new DisplayChartVM { ChartTitle = titles[i], ChartValue = count });
            }

            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].ChartTitle == "T")
                {
                    result[i].ChartTitle = "آزمایشی";
                }
                else if (result[i].ChartTitle == "C")
                {
                    result[i].ChartTitle = "قراردادی";
                }
                else
                {
                    result[i].ChartTitle = "رسمی";

                }
            }

            return result;

        }


        public List<DisplayChartVM>? GetCountGenders(AreaVM model)
        {
            if (model == null)
                return null;

            var context = _userRepository.GetUsersQuery(model);

            List<DisplayChartVM> result = new();

            var man = context.Where(u => u.IsActived &&
                                         u.Gender == "M")
                                .Count();

            result.Add(new DisplayChartVM { ChartTitle = "مرد", ChartValue = man });


            var woman = context.Where(u => u.IsActived &&
                                           u.Gender == "W")
                               .Count();

            result.Add(new DisplayChartVM { ChartTitle = "زن", ChartValue = woman });

            return result;
        }

        public List<DisplayChartVM>? GetCountEducations(DisplayReportVM model)
        {
            if (model == null)
                return null;

            var context = _context.Users.Where(u => u.Department.Area == model.Section &&
                                                    u.Department.Province == model.Province &&
                                                    u.Department.County == model.County &&
                                                    u.Department.District == model.District &&
                                                    u.IsActived)
                                        .ToList();

            List<DisplayChartVM> result = new();

            var titles = context.Select(u => u.Education).Distinct().ToList();

            for (int i = 0; i < titles.Count; i++)
            {
                int count = context.Where(u => u.Education == titles[i])
                                   .Count();

                result.Add(new DisplayChartVM { ChartTitle = titles[i], ChartValue = count });
            }

            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].ChartTitle == "Dip")
                {
                    result[i].ChartTitle = "دیپلم";
                }
                else if (result[i].ChartTitle == "B")
                {
                    result[i].ChartTitle = "کارشناسی";
                }
                else if (result[i].ChartTitle == "M")
                {
                    result[i].ChartTitle = "کارشناسی ارشد";
                }
                else
                {
                    result[i].ChartTitle = "دکترا";
                }
            }

            return result;
        }
        public DisplayHistoriesUsersVM? CalculateMinHistory(DisplayReportVM model)
        {
            if (model == null)
                return null;
            var user = (from item in _context.Users
                        where item.Department.Area == model.Section &&
                              item.Department.Province == model.Province &&
                              item.Department.County == model.County &&
                              item.Department.District == model.District &&
                              item.IsActived
                        orderby item.RegisterDate descending
                        select new DisplayHistoriesUsersVM
                        {
                            UserName = item.UserName,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Roles = item.UserRoles.Select(r => r.Role.Title).ToList(),
                            History = item.RegisterDate.CalculateHistory(),
                        }
                        ).FirstOrDefault();

            return user;
        }

        public DisplayHistoriesUsersVM? CalculateMaxHistory(DisplayReportVM model)
        {
            if (model == null)
                return null;
            var user = (from item in _context.Users
                        where item.Department.Area == model.Section &&
                              item.Department.Province == model.Province &&
                              item.Department.County == model.County &&
                              item.Department.District == model.District &&
                              item.IsActived
                        orderby item.RegisterDate ascending
                        select new DisplayHistoriesUsersVM
                        {
                            UserName = item.UserName,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Roles = item.UserRoles.Select(r => r.Role.Title).ToList(),
                            History = item.RegisterDate.CalculateHistory(),
                        }
                        ).FirstOrDefault();

            return user;
        }

        public HourlyWorkVM? CalculateHourlyWork(DisplayReportVM model)
        {
            if (model == null)
                return null;

            var users = _context.Users.Where(item => item.Department.Area == model.Section &&
                                             item.Department.Province == model.Province &&
                                             item.Department.County == model.County &&
                                             item.Department.District == model.District &&
                                             item.IsActived)
                                      .ToList();

            if (!users.Any())
                return null;

            var dailyWorkHistories = users.Select(u => u.RegisterDate.CalculateDailyWorkHistory());

            var min = dailyWorkHistories.Min();
            var max = dailyWorkHistories.Max();

            HourlyWorkVM data = new()
            {
                Max = max,
                Min = min
            };
            return data;
        }





    }
}