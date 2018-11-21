using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TAC.Business;
using TAC.Dto;
using TAC.Web.Models.Api.Request;

namespace TAC.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleManagerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IVehicleManager _vehicleManager;

        public VehicleManagerController(IVehicleManager vehicleManager, IMapper mapper)
        {
            _vehicleManager = vehicleManager;
            _mapper = mapper;
        }

        [HttpPut]
        [Route("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody]UpdateStatusRequest updateStatusRequest)
        {
            await _vehicleManager.UpdateLastConnection(updateStatusRequest.VIN, updateStatusRequest.RegistrationNumber);

            return Ok();
        }

        [HttpPost]
        [Route("FindVehicle")]
        public async Task<VehicleInfo> FindVehicle([FromBody]GetVehicleRequest getVehicleRequest)
        {
            var vehicleInfo = await _vehicleManager.FindVehicle(getVehicleRequest.VIN, getVehicleRequest.RegistrationNumber);

            return vehicleInfo;
        }

        [HttpGet]
        [Route("GetCustomers")]
        public async Task<IList<CustomerInfo>> GetCustomers()
        {
            var customers = await _vehicleManager.GetAllCustomers();

            return customers;
        }

        [HttpGet]
        [Route("GetVehicles")]
        public async Task<IList<VehicleInfo>> GetVehicles(int? customerId, VehicleStatusType? vehicleStatusType)
        {
            var vehicleSearchCriteria = new VehicleSearchCriteria()
            {
                CustomerId = customerId,
                VehicleStatus = vehicleStatusType.ToVehicleStatus()
            };

            var vehicles = await _vehicleManager.GetAllVehicles(vehicleSearchCriteria);

            return vehicles;
        }

        [HttpGet]
        [Route("GetVehicleStatuses")]
        public Dictionary<string, string> GetVehicleStatuses()
        {
            var vehicles = _vehicleManager.GetVehicleStatuses();

            return vehicles;
        }
    }
}
