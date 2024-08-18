using DAL.Entity;
using DAL.Tools.ListingHelper;
using DBL.AuditTrail_Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utils.Enums;
using Utils.Model;

namespace E_commerce.Controllers.AuditTrail_Controller
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = nameof(Enum_UserRole.Admin))]
    public class AuditTrailController : BaseAPIController
    {
        private IAuditTrailService _auditTrailService;
        public AuditTrailController(IAuditTrailService auditTrailService) 
        {
            _auditTrailService = auditTrailService;
        }

        [HttpGet]
        [Route("GetAuditTrailList")]
        public async Task<IActionResult> GetAuditTrailList([FromQuery] FilterParameters filterParameters)
        {
            ApiResponse<PagedResult<T_AuditTrail>>? apiResponse = null;

            try
            {
                var result = await _auditTrailService.GetPagedListAsync(filterParameters);

                // Create a success response using ApiResponse<T>
                apiResponse = ApiResponse<PagedResult<T_AuditTrail>>.CreateSuccessResponse(result, "Get Audit Trail List Successful");
            }
            catch (Exception ex)
            {
                apiResponse = ApiResponse<PagedResult<T_AuditTrail>>.CreateErrorResponse($"Get Audit Trail List Failed. Exception: {ex.Message}");
            }


            return Ok(apiResponse);
        }
    }
}
