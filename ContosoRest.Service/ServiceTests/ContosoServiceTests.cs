using ContosoRest.Models.Domain;
using ContosoRest.Models.Enum;
using ContosoRest.Models.Shared;
using ContosoRest.Repository.Mocks;
using ContosoRest.Service.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace ContosoRest.Service.ServiceTests
{
    public class ContosoServiceTests : IClassFixture<ServiceTestsClassFixture>
    {
        #region -- Properties -----
        // Define all of the "injectable" items to allow the execution of the 
        // service code
        private ServiceTestsClassFixture _classFixture;
        private MockContosoRepo _mockContosoRepo;
        private ContosoService _contosoService;

        // Build input model objects and result model objects
        private ContosoModel _inputCreateContosoModel;
        private ContosoModel _updateContosoModel;
        private ContosoModel _resultContosoModel;
        private List<ContosoModel> _resultContosoModels;
        #endregion

        #region -- Constructor/Setup -----
        public ContosoServiceTests(ServiceTestsClassFixture fixture, ITestOutputHelper output)
        {
            _classFixture = fixture;
            _classFixture.ConfigureLogging(output);
            ParameterSetup();
        }

        private void ParameterSetup()
        {
            // Since this is for input, we do not want an Id. It will be assigned when 
            // the model is created.
            _inputCreateContosoModel = new ContosoModel()
            {
                Description = "Third ContosoModel"
            };

            _updateContosoModel = new ContosoModel()
            {
                Id = 3,
                Description = "Modified ContosoModel"
            };

            _resultContosoModel = new ContosoModel()
            {
                Id = 3,
                Description = "Third ContosoModel"
            };

            _resultContosoModels = new List<ContosoModel>()
            {
                new ContosoModel() { Id = 1, Description = "Original ContosoModel"},
                new ContosoModel() { Id = 3, Description = "Third ContosoModel"},
            };
        }
        #endregion

        #region -- Test Methods -----
        [Fact]
        public void ContosoService_CreateContoso_Valid()
        {
            // Setup
            int expectedResult = 3;
            _mockContosoRepo = new MockContosoRepo().MockCreateContosoAsync(_resultContosoModel);
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Exercise
            var result = _contosoService.CreateContosoAsync(_inputCreateContosoModel);

            // Verify
            Assert.Equal(expectedResult, result.Result.Id);

            // Teardown - Not needed for this test
        }

        [Fact]
        public void ContosoService_CreateContoso_Invalid()
        {
            // Setup
            _mockContosoRepo = new MockContosoRepo().MockCreateContosoAsyncFails();
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Exercise
            var result = _contosoService.CreateContosoAsync(_inputCreateContosoModel);

            // Verify
            Assert.True(result.Result == null);

            // Teardown - Not needed for this test
        }

        [Fact]
        public void ContosoService_GetContoso_Single_Valid()
        {
            // Setup
            _mockContosoRepo = new MockContosoRepo().MockGetContosoAsync(_resultContosoModel);
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Exercise
            var result = _contosoService.GetContosoAsync(_resultContosoModel.Id);

            // Verify
            Assert.Equal(_resultContosoModel, result.Result);

            // Teardown - Not needed for this test
        }

        [Fact]
        public void ContosoService_GetContoso_Single_Invalid()
        {
            // Setup
            _mockContosoRepo = new MockContosoRepo().MockGetContosoAsyncFails();
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Exercise
            var result = _contosoService.GetContosoAsync(-1);

            // Verify
            Assert.True(result.Result == null);

            // Teardown - Not needed for this test
        }

        [Fact]
        public void ContosoService_GetContoso_Multiple_Valid()
        {
            // Setup
            _mockContosoRepo = new MockContosoRepo().MockGetContosoAsync(_resultContosoModels);
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Exercise
            var result = _contosoService.GetContosoAsync();

            // Verify
            Assert.Equal(_resultContosoModels, result.Result);

            // Teardown - Not needed for this test
        }

        [Fact]
        public void ContosoService_GetContoso_Multiple_Invalid()
        {
            // Setup
            _mockContosoRepo = new MockContosoRepo().MockGetContosoAsync(new List<ContosoModel>());
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Exercise
            var result = _contosoService.GetContosoAsync();

            // Verify
            Assert.True(result.Result.Count == 0);

            // Teardown - Not needed for this test
        }

        [Fact]
        public void ContosoService_UpdateContoso_Valid()
        {
            // Setup
            _mockContosoRepo = new MockContosoRepo().MockUpdateContosoAsync(_updateContosoModel);
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Exercise
            var result = _contosoService.UpdateContosoAsync(_updateContosoModel);

            // Verify
            Assert.Equal(_updateContosoModel, result.Result);

            // Teardown - Not needed for this test
        }

        [Fact]
        public void ContosoService_UpdateContoso_Invalid()
        {
            // Setup
            _mockContosoRepo = new MockContosoRepo().MockUpdateContosoAsyncFails();
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Exercise
            var result = _contosoService.UpdateContosoAsync(_updateContosoModel);

            // Verify
            Assert.True(result.Result == null);

            // Teardown - Not needed for this test
        }

        [Fact]
        public void ContosoService_DeleteContoso_Valid()
        {
            // Setup
            _mockContosoRepo = new MockContosoRepo()
                .MockDeleteContosoAsync(OperationResult.Deleted)
                .MockGetContosoAsync(_resultContosoModel);
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Exercise
            var result = _contosoService.DeleteContosoAsync(3);

            // Verify
            Assert.Equal(OperationResult.Deleted, result.Result);
            // Also validate that the Delete mock got called one time
            _mockContosoRepo.VerifyDeleteContosoAsync(Times.Once());

            // Teardown - Not needed for this test
        }

        [Fact]
        public void ContosoService_DeleteContoso_Invalid()
        {
            // Setup
            _mockContosoRepo = new MockContosoRepo()
                .MockDeleteContosoAsync(OperationResult.NotFound)
                .MockGetContosoAsyncFails();
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Exercise
            var result = _contosoService.DeleteContosoAsync(5);

            // Verify
            Assert.Equal(OperationResult.NotFound, result.Result);
            // Also validate that the Delete mock never got called
            _mockContosoRepo.VerifyDeleteContosoAsync(Times.Never());

            // Teardown - Not needed for this test
        }

        #endregion
    }
}
