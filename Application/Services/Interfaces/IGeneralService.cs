using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Application.Services.Interfaces
{
    public interface IGeneralService
    {
        List<SelectListItem> GenderTypes();
        List<SelectListItem> EducationTypes();
        List<SelectListItem> MariltalTypes();
        List<SelectListItem> ProvinceDepartmentTypes();
        List<SelectListItem> CountyDepartmentTypes();
        List<SelectListItem> DistrictDepartmentTypes();
        List<SelectListItem> EmploymentTypes();

    }
}
