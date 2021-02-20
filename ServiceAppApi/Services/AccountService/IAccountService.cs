
using ServiceAppApi.ModelDTO;
using ServiceAppApi.ModelDTO.Account;
using ServiceAppApi.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ServiceAppApi.Services.AccountService
{
    public interface IAccountService
    {
        ResponseDTO Login(string phoneNumber, string password);
        ResponseDTO Register(RegisterDTO registerDTO);
        IQueryable<T> GetPartyRole<T>(Expression<Func<T, bool>> predicate = null) where T : PartyRole;
        IQueryable<T> GetRole<T>(Expression<Func<T, bool>> predicate = null) where T : Role;
    }
}
