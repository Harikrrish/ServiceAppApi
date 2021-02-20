
using ServiceAppApi.ModelDTO;
using ServiceAppApi.Models;
using ServiceAppApi.Repositories.UnitOfWork;
using C = ServiceAppApi.Constants.Constants;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System;
using System.Linq;
using System.Linq.Expressions;
using ServiceAppApi.ModelDTO.Account;
using Utility = ServiceAppApi.Utilities.Utilities;

namespace ServiceAppApi.Services.AccountService
{
    public class AccountService : IAccountService
    {
        #region Injectors
        private IRepository<PartyRole> PartyRoleRepository { get; set; }
        private IRepository<Role> RoleRepository { get; set; }
        private IUnitOfWork UnitOfWork { get; set; }
        #endregion

        #region Constructor
        public AccountService(IRepository<PartyRole> partyRoleRepository, IRepository<Role> roleRepository, IUnitOfWork unitOfWork)
        {
            PartyRoleRepository = partyRoleRepository;
            RoleRepository = roleRepository;
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region Login
        public ResponseDTO Login(string phoneNumber, string password)
        {
            ResponseDTO response = new ResponseDTO();
            if (phoneNumber != null && phoneNumber.Length == 10 && Utility.IsValidPhoneNumber(phoneNumber))
            {
                if (password != null && password.Length >= 6)
                {
                    PartyRole partyRole = GetPartyRole<PartyRole>(a => a.PhoneNumber == phoneNumber && a.IsActive).FirstOrDefault();
                    if (partyRole != null)
                    {
                        bool isPasswordCorrect = VerifyHashedPassword(partyRole.Password, password);
                        if (isPasswordCorrect)
                        {
                            response.Message = C.LOGGED_IN_SUCCESSFULLY;
                            response.Success = true;
                        }
                        else
                        {
                            response.Message = C.ENTER_VALID_PASSWORD;
                        }
                    }
                    else
                    {
                        response.Message = C.USER_NOT_FOUND;
                    }
                }
                else
                {
                    response.Message = C.ENTER_VALID_PASSWORD;
                }
            }
            else
            {
                response.Message = C.ENTER_VALID_PHONE_NUMBER;
            }
            return response;
        }
        #endregion

        #region Register
        public ResponseDTO Register(RegisterDTO registerDTO)
        {
            ResponseDTO response = new ResponseDTO();
            if (!string.IsNullOrWhiteSpace(registerDTO.Name))
            {
                if (registerDTO.PhoneNumber != null && registerDTO.PhoneNumber.Length == 10 && Utility.IsValidPhoneNumber(registerDTO.PhoneNumber))
                {
                    if (registerDTO.Email != null && Utility.IsValidEmail(registerDTO.Email))
                    {
                        if (registerDTO.Password != null && registerDTO.Password.Length >= 6)
                        {
                            PartyRole partyRole = GetPartyRole<PartyRole>(a => (a.PhoneNumber == registerDTO.PhoneNumber || a.Email == registerDTO.Email) && a.IsActive).FirstOrDefault();
                            if (partyRole != null)
                            {
                                response.Message = C.USER_ALREADY_EXISTS;
                            }
                            else
                            {
                                string passwordHash = HashPassword(registerDTO.Password);
                                Role role = GetRole<Role>(a => a.RoleName == C.CUSTOMER_ROLE).FirstOrDefault();
                                if (role != null)
                                {
                                    PartyRole newUser = new PartyRole
                                    {
                                        Name = registerDTO.Name,
                                        PhoneNumber = registerDTO.PhoneNumber,
                                        Email = registerDTO.Email,
                                        Password = passwordHash,
                                        IsActive = true,
                                        RoleId = role.RoleId
                                    };
                                    SavePartyRole(newUser);
                                    response.Message = C.REGISTERED_SUCCESSFULLY;
                                    response.Success = true;
                                }
                            }
                        }
                        else
                        {
                            response.Message = C.ENTER_VALID_PASSWORD;
                        }
                    }
                    else
                    {
                        response.Message = C.ENTER_VALID_EMAIL;
                    }
                }
                else
                {
                    response.Message = C.ENTER_VALID_PHONE_NUMBER;
                }
            }
            else
            {
                response.Message = C.ENTER_VALID_NAME;
            }

            return response;
        }
        #endregion

        #region GetPartyRole
        public IQueryable<T> GetPartyRole<T>(Expression<Func<T, bool>> predicate = null) where T : PartyRole
        {
            IQueryable<T> result = null;

            result = PartyRoleRepository.FindAll().OfType<T>();

            if (predicate != null)
            {
                result = result.Where(predicate);
            }

            return result;
        }
        #endregion

        #region GetRole
        public IQueryable<T> GetRole<T>(Expression<Func<T, bool>> predicate = null) where T : Role
        {
            IQueryable<T> result = null;

            result = RoleRepository.FindAll().OfType<T>();

            if (predicate != null)
            {
                result = result.Where(predicate);
            }

            return result;
        }
        #endregion

        #region SavePartyRole
        public void SavePartyRole(PartyRole partyRole)
        {
            if (partyRole.PartyRoleId > 0)
                PartyRoleRepository.Add(partyRole);
            else
                PartyRoleRepository.Attach(partyRole);

            Commit();
        }
        #endregion

        private void Commit()
        {
            UnitOfWork.Commit();
        }

        private static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        private bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return buffer3.SequenceEqual(buffer4);
        }
    }
}
