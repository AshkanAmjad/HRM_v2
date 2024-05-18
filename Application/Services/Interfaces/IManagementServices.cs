
using System.Web.Mvc;

namespace Application.Services.Interfaces
{
    public interface IManagementServices
    {
        List<SelectListItem> GetAreas();
    }
}
