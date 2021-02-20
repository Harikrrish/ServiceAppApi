using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAppApi.ModelDTO;
using ServiceAppApi.ModelDTO.Account;
using ServiceAppApi.Services.AccountService;
using System;

namespace ServiceAppApi.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        #region Injectors
        private IAccountService AccountService { get; set; }

        #endregion

        #region Constructor
        public AccountController(IAccountService accountService)
        {
            AccountService = accountService;
        }
        #endregion

        #region Login
        [Route("login")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string phoneNumber, string password)
        {

            IActionResult actionResult = null;
            try
            {
                ResponseDTO response = new ResponseDTO();
                response = AccountService.Login(phoneNumber, password);
                actionResult = Ok(new { success = response.Success, message = response.Message, data = response.Data });
            }
            catch (Exception ex)
            {
                actionResult = Ok(new { success = false, message = ex.Message });
            }
            return actionResult;
        }
        #endregion

        #region Register
        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register([FromBody]RegisterDTO register)
        {

            IActionResult actionResult = null;
            try
            {
                ResponseDTO response = new ResponseDTO();
                response = AccountService.Register(register);
                actionResult = Ok(new { success = response.Success, message = response.Message, data = response.Data });
            }
            catch (Exception ex)
            {
                actionResult = Ok(new { success = false, message = ex.Message });
            }
            return actionResult;
        }
        #endregion
    }

}
