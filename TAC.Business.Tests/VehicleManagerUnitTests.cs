using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TAC.Domain;
using TAC.Domain.Infrastructure.Repositories;
using TAC.Dto;
using Xunit;

namespace TAC.Business.Tests
{
    public class VehicleManagerUnitTests : IDisposable
    {
        private Mock<ICustomerRepository> _customerRepositoryMock;
        private Mock<IVehicleRepository> _vehicleRepositoryMock;

        public VehicleManagerUnitTests()
        {
            Mapper.Initialize(x => { x.AddProfile<DtoMappingProfile>(); });
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _vehicleRepositoryMock = new Mock<IVehicleRepository>();
        }

        [Fact]
        public void VehicleManager_GetAllVehicles_ShouldBeAbleToReturnAllVehicles()
        {
            IVehicleManager vehicleManager = new VehicleManager(_customerRepositoryMock.Object, _vehicleRepositoryMock.Object, Mapper.Instance);
            Mock<IVehicleSearchCriteria> vehicleSearchCriteria = new Mock<IVehicleSearchCriteria>();
            vehicleManager.GetAllVehicles(vehicleSearchCriteria.Object);
            _vehicleRepositoryMock.Verify(c => c.GetAll(), Times.Once);
        }

        [Fact]
        public void VehicleManager_GetAllVehicles_ShouldBeAbleToReturnTotalVehicleCount()
        {
            IQueryable<Vehicle> vehicles = new List<Vehicle>
            {
                new Vehicle { Id = 1, VIN = "YS2R4X20005399401", RegistrationNumber = "ABC123", CustomerId = 1, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown },
                new Vehicle { Id = 2, VIN = "VLUR4X20009093588", RegistrationNumber = "DEF456", CustomerId = 1, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown }
            }.AsQueryable();

            _vehicleRepositoryMock.Setup(x => x.GetAll()).Returns(vehicles.AsQueryable());
            IVehicleManager vehicleManager = new VehicleManager(_customerRepositoryMock.Object, _vehicleRepositoryMock.Object, Mapper.Instance);
            var result = vehicleManager.TotalVehicleCount();
            _vehicleRepositoryMock.Verify(c => c.GetAll(), Times.Once);
            result.Should().Be(2);
        }

        [Fact]
        public void VehicleManager_GetAllVehicles_ShouldBeAbleToReturnEmptyListWhenSearchCriteriaIsNull()
        {
            IVehicleManager vehicleManager = new VehicleManager(_customerRepositoryMock.Object, _vehicleRepositoryMock.Object, Mapper.Instance);
            var result = vehicleManager.GetAllVehicles(null).Result;
            result.Should().NotBeNull();
            result.Count.Should().Be(0);
        }

        [Fact]
        public void VehicleManager_GetAllCustomers_ShouldBeAbleToReturnAllCustomers()
        {
            IVehicleManager vehicleManager = new VehicleManager(_customerRepositoryMock.Object, _vehicleRepositoryMock.Object, Mapper.Instance);
            vehicleManager.GetAllCustomers();
            _customerRepositoryMock.Verify(c => c.GetAll(), Times.Once);
        }

        [Fact]
        public void VehicleManager_FindVehicle_ShouldBeAbleToFindVehicleByVINAndRegistrationNumber()
        {
            var vehicle = new Vehicle { Id = 1, VIN = "YS2R4X20005399401", RegistrationNumber = "ABC123", CustomerId = 1, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown };
            _vehicleRepositoryMock.Setup(x => x.GetByFilter(It.IsAny<Expression<Func<Vehicle, bool>>>())).Returns(Task<Vehicle>.Run(() => { return vehicle; }));

            IVehicleManager vehicleManager = new VehicleManager(_customerRepositoryMock.Object, _vehicleRepositoryMock.Object, Mapper.Instance);
            vehicleManager.FindVehicle(It.IsAny<string>(), It.IsAny<string>());
            _vehicleRepositoryMock.Verify(c => c.GetByFilter(It.IsAny<Expression<Func<Vehicle, bool>>>()), Times.Once);
        }

        [Fact]
        public void VehicleManager_UpdateLastConnection_ShouldBeAbleToUpdateLastConnectionOfVehicleByVINAndRegistrationNumber()
        {
            var vehicle = new Vehicle { Id = 1, VIN = "YS2R4X20005399401", RegistrationNumber = "ABC123", CustomerId = 1, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown };
            _vehicleRepositoryMock.Setup(x => x.GetByFilter(It.IsAny<Expression<Func<Vehicle, bool>>>())).Returns(Task<Vehicle>.Run(() => { return vehicle; }));
            _vehicleRepositoryMock.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Vehicle>())).Returns(It.IsAny<Task>());

            IVehicleManager vehicleManager = new VehicleManager(_customerRepositoryMock.Object, _vehicleRepositoryMock.Object, Mapper.Instance);
            vehicleManager.UpdateLastConnection(It.IsAny<string>(), It.IsAny<string>());
            _vehicleRepositoryMock.Verify(c => c.GetByFilter(It.IsAny<Expression<Func<Vehicle, bool>>>()), Times.Once);
            _vehicleRepositoryMock.Verify(c => c.Update(It.IsAny<int>(), It.IsAny<Vehicle>()), Times.Once);
        }

        [Fact]
        public void VehicleManager_VehicleDisconnect_ShouldBeAbleToMakeVehicleDisconnectedByVINAndRegistrationNumber()
        {
            var vehicle = new Vehicle { Id = 1, VIN = "YS2R4X20005399401", RegistrationNumber = "ABC123", CustomerId = 1, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown };
            _vehicleRepositoryMock.Setup(x => x.GetByFilter(It.IsAny<Expression<Func<Vehicle, bool>>>())).Returns(Task<Vehicle>.Run(() => { return vehicle; }));
            _vehicleRepositoryMock.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Vehicle>())).Returns(It.IsAny<Task>());

            IVehicleManager vehicleManager = new VehicleManager(_customerRepositoryMock.Object, _vehicleRepositoryMock.Object, Mapper.Instance);
            vehicleManager.VehichleDisconnect(It.IsAny<string>(), It.IsAny<string>());
            _vehicleRepositoryMock.Verify(c => c.GetByFilter(It.IsAny<Expression<Func<Vehicle, bool>>>()), Times.Once);
            _vehicleRepositoryMock.Verify(c => c.Update(It.IsAny<int>(), It.IsAny<Vehicle>()), Times.Once);
        }

        [Fact]
        public void VehicleManager_GetUnavailableVehicles_ShouldBeAbleToReturnAllUnavailableVehicles()
        {
            var expiredConnectedOnTime = DateTime.Now.Subtract(new TimeSpan(0, 1, 30));
            IList<Vehicle> vehicles = new List<Vehicle>
            {
                new Vehicle { Id = 1, VIN = "YS2R4X20005399401", RegistrationNumber = "ABC123", CustomerId = 1, LastConnectedOn = null, ConnectedOn = expiredConnectedOnTime, VehicleStatus = VehicleStatus.Connected },
            };

            _vehicleRepositoryMock.Setup(x => x.ListByFilter(It.IsAny<Expression<Func<Vehicle, bool>>>())).Returns(Task<Vehicle>.Run(() => { return vehicles; }));
            IVehicleManager vehicleManager = new VehicleManager(_customerRepositoryMock.Object, _vehicleRepositoryMock.Object, Mapper.Instance);
            var result = vehicleManager.GetUnavailableVehicles().Result;
            _vehicleRepositoryMock.Verify(c => c.ListByFilter(It.IsAny<Expression<Func<Vehicle, bool>>>()), Times.Once);
            result.Count.Should().Be(1);
        }

        [Fact]
        public void VehicleManager_PopulateRandomVehicle_ShouldBeAbleToPopulateRandomVehicle()
        {
            IVehicleManager vehicleManager = new VehicleManager(_customerRepositoryMock.Object, _vehicleRepositoryMock.Object, Mapper.Instance);
            vehicleManager.PopulateRandomVehicle(1);
            _vehicleRepositoryMock.Verify(c => c.GetAll(), Times.Once);
        }

        [Fact]
        public void VehicleManager_PopulateRandomVehicle_ShouldBeAbleToReturnEmptyWhenRecordCountLessThanOrEqualZero()
        {
            IVehicleManager vehicleManager = new VehicleManager(_customerRepositoryMock.Object, _vehicleRepositoryMock.Object, Mapper.Instance);
            var result = vehicleManager.PopulateRandomVehicle(-1).Result;
            result.Should().NotBeNull();
            result.Count.Should().Be(0);
        }

        [Fact]
        public void VehicleManager_GetVehicleStatuses_ShouldBeAbleToReturnVehicleStatuses()
        {
            IVehicleManager vehicleManager = new VehicleManager(_customerRepositoryMock.Object, _vehicleRepositoryMock.Object, Mapper.Instance);
            var statuses = vehicleManager.GetVehicleStatuses();
            statuses.Should().NotBeNull();
            statuses.Count.Should().BeGreaterOrEqualTo(1);
        }

        public void Dispose()
        {
            Mapper.Reset();
            _customerRepositoryMock.Reset();
            _customerRepositoryMock.Reset();
        }
    }
}
