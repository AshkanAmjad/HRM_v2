using Application.Services.Interfaces;
using AutoMapper;
using Domain.DTOs.Portal.Transfer;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implrmentations
{
    public class TransferService : ITransferService
    {
        #region Constructor
        private readonly ITransferRepository _transferRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;

        public TransferService(ITransferRepository transferRepository,
                               IUserRepository userRepository,
                               IUserRoleRepository userRoleRepository,
                               IMapper mapper)
        {
            _transferRepository = transferRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }
        #endregion
        public bool Register(TransferRegisterVM model, out string message)
        {


            var area = _userRepository.GetAreaUserByUserId(model.UserIdUploader);
            var role = _userRoleRepository.GetUserRolesByUserId(model.UserIdUploader);

            string rolesUploader = string.Join("-", role);

            #region Seed Data On Transfers Table
            model.TransferId = Guid.NewGuid();
            model.ProvinceUploader = area.Province;
            model.RoleUploader = rolesUploader;
            model.CountyUploader = area.County;
            model.DistrictUploader = area.District;
            model.UploadDate = DateTime.Now;
            model.IsActived = true;
            #endregion

            bool result = _transferRepository.Register(model, out message);
            return result;
        }
    }
}
