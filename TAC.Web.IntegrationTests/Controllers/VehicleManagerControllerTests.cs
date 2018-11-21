using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using TAC.Dto;
using TAC.Web.Models.Api.Request;
using Xunit;

namespace TAC.Web.IntegrationTests.Controllers
{
    public class VehicleManagerControllerTests : IClassFixture<FakeWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public VehicleManagerControllerTests(FakeWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task VehicleManagerController_CanListCustomers()
        {
            var httpResponse = await _client.GetAsync("/api/VehicleManager/GetCustomers");
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<List<CustomerInfo>>(stringResponse);
            customers.Should().NotBeNull();
            customers.Count.Should().BeGreaterThan(0);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task VehicleManagerController_CanListVehicles()
        {
            var httpResponse = await _client.GetAsync("/api/VehicleManager/GetVehicles");
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var vehicles = JsonConvert.DeserializeObject<List<VehicleInfo>>(stringResponse);
            vehicles.Should().NotBeNull();
            vehicles.Count.Should().BeGreaterThan(0);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task VehicleManagerController_CanListVehiclesByCustomerId()
        {
            var httpResponse = await _client.GetAsync("/api/VehicleManager/GetVehicles?customerId=1");
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var vehicles = JsonConvert.DeserializeObject<List<VehicleInfo>>(stringResponse);
            vehicles.Should().NotBeNull();
            vehicles.Count.Should().BeGreaterThan(0);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task VehicleManagerController_CanListVehiclesByVehicleStatusType()
        {
            var httpResponse = await _client.GetAsync("/api/VehicleManager/GetVehicles?vehicleStatusType=-1");
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var vehicles = JsonConvert.DeserializeObject<List<VehicleInfo>>(stringResponse);
            vehicles.Should().NotBeNull();
            vehicles.Count.Should().BeGreaterThan(0);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task VehicleManagerController_CanListVehiclesByCustomerIdAndVehicleStatusType()
        {
            var httpResponse = await _client.GetAsync("/api/VehicleManager/GetVehicles?customerId=1&vehicleStatusType=-1");
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var vehicles = JsonConvert.DeserializeObject<List<VehicleInfo>>(stringResponse);
            vehicles.Should().NotBeNull();
            vehicles.Count.Should().BeGreaterThan(0);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task VehicleManagerController_CanListVehicleStatuses()
        {
            var httpResponse = await _client.GetAsync("/api/VehicleManager/GetVehicleStatuses");
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var vehicleStatuses = JsonConvert.DeserializeObject<Dictionary<string, string>>(stringResponse);
            vehicleStatuses.Should().NotBeNull();
            vehicleStatuses.Count.Should().BeGreaterThan(0);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task VehicleManagerController_CanFindVehicle()
        {
            var vehicle = FakeData.GetVehicle;
            var request = new GetVehicleRequest { VIN = vehicle.VIN, RegistrationNumber = vehicle.RegistrationNumber };
            var httpResponse = await _client.PostAsync<GetVehicleRequest>("/api/VehicleManager/FindVehicle", request, new JsonMediaTypeFormatter());
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var vehicleInfo = JsonConvert.DeserializeObject<VehicleInfo>(stringResponse);
            vehicleInfo.Should().NotBeNull();
            vehicleInfo.VIN.Should().Be(vehicle.VIN);
            vehicleInfo.RegistrationNumber.Should().Be(vehicle.RegistrationNumber);
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task VehicleManagerController_CanUpdateVehicleStatus()
        {
            var vehicle = FakeData.GetVehicle;
            var request = new UpdateStatusRequest { VIN = vehicle.VIN, RegistrationNumber = vehicle.RegistrationNumber };
            var httpResponse = await _client.PutAsync<UpdateStatusRequest>("/api/VehicleManager/UpdateStatus", request, new JsonMediaTypeFormatter());
            httpResponse.EnsureSuccessStatusCode();
            httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
