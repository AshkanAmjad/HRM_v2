using Application.Services.Interfaces;
using AutoMapper;
using Data.Extensions;
using Domain.DTOs.Portal.Document;
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
        private readonly IRoleRepository _roleRepository;
        private readonly IDocumentService _documentService;
        private readonly IMapper _mapper;

        public TransferService(ITransferRepository transferRepository,
                               IUserRepository userRepository,
                               IUserRoleRepository userRoleRepository,
                               IDocumentService documentService,
                               IRoleRepository roleRepository,
                               IMapper mapper)
        {
            _transferRepository = transferRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _documentService = documentService;
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
            model.AreaUploader = area.Section;
            model.CountyUploader = area.County;
            model.DistrictUploader = area.District;
            model.UploadDate = DateTime.Now;
            model.IsActived = true;

            if (model.UserIdReceiver != $"{Guid.Empty}")
            {
                var userIdReceiver = new Guid(model.UserIdReceiver);
                var userName = _userRepository.GetUserNameByUserId(userIdReceiver);
                model.UserIdReceiver = userName;
            }
            else if (model.UserIdReceiver == $"{Guid.Empty}")
            {
                model.UserIdReceiver = "";
            }

            if(model.RoleReceiver != $"{Guid.Empty}")
            {
                var roleIdReceiver = new Guid(model.RoleReceiver);
                var roleReceiver = _roleRepository.GetRoleTitleById(roleIdReceiver);
                model.RoleReceiver = roleReceiver;
            }
            else if (model.UserIdReceiver == $"{Guid.Empty}")
            {
                model.RoleReceiver = "";
            }
            #endregion

            bool result = _transferRepository.Register(model, out message);

            if (result && model.Document != null)
            {
                UploadVM uploadToServer = _mapper.Map<UploadVM>(model);
                uploadToServer.Name = "Transfer";
                uploadToServer.Title = $"{model.Title}-{model.UserIdReceiver}-{model.RoleReceiver}-{model.UploadDate.ToShamsiFileUpload()}";

                _documentService.UploadDocumentToServer(uploadToServer);
            }

            return result;
        }
    }
}
