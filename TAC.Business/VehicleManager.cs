using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAC.Domain;
using TAC.Domain.Infrastructure.Repositories;
using TAC.Dto;

namespace TAC.Business
{
    public class VehicleManager : IVehicleManager
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;
        private readonly IVehicleRepository _vehicleRepository;
        public VehicleManager(ICustomerRepository customerRepository, IVehicleRepository vehicleRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }
        public async Task<VehicleInfo> FindVehicle(string VIN, string registrationNumber)
        {
            var vehicle = await _vehicleRepository.GetByFilter(x => x.VIN == VIN && x.RegistrationNumber == registrationNumber);

            var vehicleInfo = _mapper.Map<VehicleInfo>(vehicle);

            return vehicleInfo;
        }
        public async Task<IList<CustomerInfo>> GetAllCustomers()
        {
            return await _customerRepository.GetAll().Select(x => _mapper.Map<CustomerInfo>(x)).ToListAsync();
        }
        public async Task<IList<VehicleInfo>> GetAllVehicles(IVehicleSearchCriteria vehicleSearchCriteria)
        {
            var vehicleSearchCriteriaValidator = new VehicleSearchCriteriaValidator();
            var validationResult = vehicleSearchCriteriaValidator.Validate(vehicleSearchCriteria);

            if (!validationResult.IsValid)
                return new List<VehicleInfo>();

            var query = _vehicleRepository.GetAll();

            if (vehicleSearchCriteria.CustomerId.HasValue)
                query = query.Where(x => x.CustomerId == vehicleSearchCriteria.CustomerId.Value);

            if (vehicleSearchCriteria.VehicleStatus.HasValue)
                query = query.Where(x => x.VehicleStatus == vehicleSearchCriteria.VehicleStatus);

            return await query.Include(x => x.Customer).Select(x => _mapper.Map<VehicleInfo>(x)).ToListAsync();
        }
        public async Task<IList<VehicleInfo>> GetUnavailableVehicles()
        {
            var now = DateTime.Now;

            var halfAndMinute = new TimeSpan(0, 1, 30);

            var result = await _vehicleRepository.ListByFilter(x => x.ConnectedOn.HasValue && now.Subtract(x.ConnectedOn.Value) > halfAndMinute);

            return result.Select(x => _mapper.Map<VehicleInfo>(x)).ToList();
        }
        public Dictionary<string, string> GetVehicleStatuses()
        {
            var statuses = new Dictionary<string, string>();

            foreach (var name in Enum.GetNames(typeof(VehicleStatus)))
            {
                statuses.Add(((int)Enum.Parse(typeof(VehicleStatus), name)).ToString(), name);
            }

            return statuses;
        }
        public async Task<IList<VehicleInfo>> PopulateRandomVehicle(int recordCount)
        {
            if (recordCount <= 0)
                return new List<VehicleInfo>();

            return await _vehicleRepository.GetAll().OrderBy(v => Guid.NewGuid()).Take(recordCount).Select(x => _mapper.Map<VehicleInfo>(x)).ToListAsync();
        }
        public int TotalVehicleCount()
        {
            return _vehicleRepository.GetAll().Count();
        }
        public async Task UpdateLastConnection(string VIN, string registrationNumber)
        {
            var vehicle = await _vehicleRepository.GetByFilter(x => x.VIN == VIN && x.RegistrationNumber == registrationNumber);

            vehicle.VehicleStatus = VehicleStatus.Connected;

            vehicle.LastConnectedOn = vehicle.ConnectedOn = DateTime.Now;

            await _vehicleRepository.Update(vehicle.Id, vehicle);
        }
        public async Task VehichleDisconnect(string VIN, string registrationNumber)
        {
            var vehicle = await _vehicleRepository.GetByFilter(x => x.VIN == VIN && x.RegistrationNumber == registrationNumber);

            vehicle.VehicleStatus = VehicleStatus.Disconnected;

            vehicle.ConnectedOn = null;

            await _vehicleRepository.Update(vehicle.Id, vehicle);
        }
    }
}
