using AutoMapper;
using Dapper;
using Microsoft.EntityFrameworkCore;
using RapidPayTest.Application.DTOs;
using RapidPayTest.Application.DTOs.ViewModel;
using RapidPayTest.Application.Exceptions;
using RapidPayTest.Application.Interfaces.Repositories;
using RapidPayTest.Application.Wrappers;
using RapidPayTest.Domain.Entities;
using RapidPayTest.Infrastructure.Data;
using System;
using System.Data;
using System.Threading.Tasks;

namespace RapidPayTest.Infrastructure.Repositories
{
    public class CardManagementRepository : ICardManagementRepository
    {
        private readonly PrincipalContext _principalContext;

        public CardManagementRepository(PrincipalContext principalContext)
        {
            _principalContext = principalContext;
        }

        public async Task<CardManagement> UpdateAsync(CardManagement cardManagement, Transaction transaction)
        {
            await _principalContext.Database.BeginTransactionAsync();

            try
            {
                //Card Management Balance Update
                _principalContext.Update(cardManagement);
                await _principalContext.SaveChangesAsync();

                //Transaction Created
                _principalContext.Add(transaction);
                await _principalContext.SaveChangesAsync();

                await _principalContext.Database.CommitTransactionAsync();
                return cardManagement;
            }

            catch (System.Exception ex)
            {
                await _principalContext.Database.RollbackTransactionAsync();
                throw new ApiException("Error trying to update balance, ex=" + ex.GetBaseException().ToString());
            }
        }
    }
}
