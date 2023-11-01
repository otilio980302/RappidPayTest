using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RapidPayTest.Application.DTOs;
using RapidPayTest.Application.DTOs.ViewModel;
using RapidPayTest.Application.Wrappers;
using RapidPayTest.Domain.Entities;

namespace RapidPayTest.Application.Interfaces.Repositories
{
    public interface ICardManagementRepository
    {
        Task<CardManagement> UpdateAsync(CardManagement cardManagement, Transaction transaction);
    }
}
