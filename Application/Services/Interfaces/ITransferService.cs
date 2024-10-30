using Domain.DTOs.Portal.Transfer;
using Domain.DTOs.Security.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ITransferService
    {
        #region Transfer
        bool Register(TransferRegisterVM model, out string message);
        #endregion
    }
}
