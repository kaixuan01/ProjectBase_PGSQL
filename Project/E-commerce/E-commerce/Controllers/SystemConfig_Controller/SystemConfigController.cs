﻿using DAL.Entity;
using DAL.Tools.ListingHelper;
using DBL.SystemConfig_Service;
using E_commerce.Tools;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Utils;
using Utils.Enums;
using Utils.Model;

namespace E_commerce.Controllers.SystemConfig_Controller
{
    [ApiController]
    [Route("[controller]")]
    public class SystemConfigController : BaseAPIController
    {
        private readonly ISystemConfigService _systemConfigService;

        public SystemConfigController(ISystemConfigService systemConfigSevice)
        {
            _systemConfigService = systemConfigSevice;
        }

        [HttpGet]
        [Route("GetSystemConfigList")]
        public async Task<IActionResult> GetSystemConfigList()
        {
            ApiResponse<List<T_SystemConfig>>? apiResponse = null;

            try
            {
                var result = await _systemConfigService.GetSystemConfigList();

                // Create a success response using ApiResponse<T>
                apiResponse = ApiResponse<List<T_SystemConfig>>.CreateSuccessResponse(result, "Get System Config List Successful");
            }
            catch (Exception ex)
            {
                LogHelper.FormatMainLogMessage(LogLevelEnums.Error, $"Exception when System Config List, Message: {ex.Message}", ex);

                apiResponse = ApiResponse<List<T_SystemConfig>>.CreateErrorResponse($"Get System Config Failed. Exception: {ex.Message}");
            }

            return Ok(apiResponse);
        }

        [HttpPost]
        [Route("UpdateSystemConfig")]
        public async Task<IActionResult> UpdateSystemConfig([FromBody] UpdateSystemConfig_REQ oReq)
        {
            ApiResponse<string>? apiResponse = null;

            try
            {
                LogHelper.FormatMainLogMessage(LogLevelEnums.Information, $"Receive Request to update system config, Request: {JsonConvert.SerializeObject(oReq)}");

                var oResp = await _systemConfigService.UpdateAsync(oReq);

                switch (oResp.Code)
                {
                    case RespCode.RespCode_Success:
                        // Create a success response using ApiResponse<T>
                        apiResponse = ApiResponse<string>.CreateSuccessResponse(null, oResp.Message);
                        break;

                    case RespCode.RespCode_Failed:
                        // Create a error response using ApiResponse<T>
                        apiResponse = ApiResponse<string>.CreateErrorResponse(oResp.Message);

                        break;
                    default: // Default is throw exception message
                        // Create a error response using ApiResponse<T>
                        apiResponse = ApiResponse<string>.CreateErrorResponse(oResp.Message);

                        break;
                        // return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                apiResponse = ApiResponse<String>.CreateErrorResponse($"Create User Failed. Exception: {ex.Message}");

                LogHelper.FormatMainLogMessage(LogLevelEnums.Error, $"Exception when create user, Message: {ex.Message}", ex);
            }


            return Ok(apiResponse);
        }

    }
}