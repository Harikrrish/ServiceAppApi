using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAppApi.Models;
using ServiceAppApi.Services.CommonService;
using System;

namespace ServiceAppApi.Controllers
{
    [Route("api/[controller]")]
    public class CommonController : Controller
    {
        #region Injectors
        private ICommonService CommonService { get; set; }

        #endregion

        #region Constructor
        public CommonController(ICommonService commonService)
        {
            CommonService = commonService;
        }
        #endregion

        #region SaveProduct
        [Route("saveproduct")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult SaveProduct()
        {
            
            IActionResult actionResult = null;
            try
            {
                Product product = new Product
                {
                    ProductName = "AC"
                };
                CommonService.SaveProduct(product);
                actionResult = Ok(new { success = true, message = string.Empty, data = string.Empty });
            }
            catch (Exception ex)
            {
                actionResult = Ok(new { success = false, message = ex.Message });
            }
            return actionResult;
        }
        // changes
        #endregion
    }
}
