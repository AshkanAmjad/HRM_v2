using AutoMapper;
using Data.Context;
using Domain.DTOs.Portal.Transfer;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositores
{
    public class TransferRepository : ITransferRepository
    {
        #region Constructor
        private readonly HRMContext _context;
        private readonly IMapper _mapper;
        public TransferRepository(HRMContext context,
                                  IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion
        public bool Register(TransferRegisterVM model, out string message)
        {
            string checkMessage = "اطلاعات ناقص ارسال شده است.";
            if (model != null)
            {

                message = "";
                return true;
            }
            message = checkMessage;
            return false;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
