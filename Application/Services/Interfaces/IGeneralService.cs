using Microsoft.AspNetCore.Mvc.Rendering;

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
        List<SelectListItem> Areas();

    }
}
