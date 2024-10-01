using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Services.Implrmentations
{
    public class GeneralService:IGeneralService
    {
        public List<SelectListItem> GenderTypes()
        {
            List<SelectListItem> genders = new()
            {
                new SelectListItem
                {
                    Text = "مرد",
                    Value = "M"
                },
                new SelectListItem
                {
                    Text = "زن",
                    Value = "W"
                }
            };
            return genders;
        }

        public List<SelectListItem> EducationTypes()
        {
            List<SelectListItem> educationTypes = new()
            {
                new SelectListItem
                {
                    Text="دیپلم",
                    Value="Dip"
                },
                new SelectListItem
                {
                    Text="کارشناسی",
                    Value="B"
                },
                new SelectListItem
                {
                    Text="کارشناسی ارشد",
                    Value="M"
                },
                new SelectListItem
                {
                    Text="دکترا",
                    Value="D"
                }
            };
            return educationTypes;
        }

        public List<SelectListItem> MariltalTypes()
        {
            List<SelectListItem> maritalTypes = new()
            {
                new SelectListItem
                {
                    Text = "مجرد",
                    Value = "S"
                },
                new SelectListItem
                {
                    Text = "متاهل",
                    Value = "M"
                }
            };
            return maritalTypes;
        }

        public List<SelectListItem> ProvinceDepartmentTypes()
        {
            List<SelectListItem> provinceDepartmentTypes = new()
            {
                new SelectListItem
                {
                    Text = "شعبه 1",
                    Value = "1"
                }
            };
            return provinceDepartmentTypes;
        }
        public List<SelectListItem> CountyDepartmentTypes()
        {
            List<SelectListItem> countyDepartmentTypes = new()
            {
                new SelectListItem
                {
                    Text = "شعبه 1",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "شعبه 2",
                    Value = "2"
                },
                new SelectListItem
                {
                    Text = "شعبه 3",
                    Value = "3"
                }
            };
            return countyDepartmentTypes;
        }

        public List<SelectListItem> DistrictDepartmentTypes()
        {
            List<SelectListItem> districtDepartmentTypes = new()
            {
                new SelectListItem
                {
                    Text = "شعبه 1",
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = "شعبه 2",
                    Value = "2"
                },
                new SelectListItem
                {
                    Text = "شعبه 3",
                    Value = "3"
                }
            };
            return districtDepartmentTypes;
        }

        public List<SelectListItem> EmploymentTypes()
        {
            List<SelectListItem> employment = new()
            {
                new SelectListItem
                {
                    Text = "آزمایشی",
                    Value = "T"
                },
                new SelectListItem
                {
                    Text = "قراردادی",
                    Value = "C"
                },
                new SelectListItem
                {
                    Text = "رسمی",
                    Value = "O"
                },
            };
            return employment;
        }
    }
}
