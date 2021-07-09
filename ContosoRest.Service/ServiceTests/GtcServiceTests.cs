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
    public class ContosoServiceTests : IClassFixture<ContosoServiceTestsClassFixture>
    {
        #region -- Properties -----
        // Define all of the "injectable" items to allow the execution of the 
        // service code
        private ContosoServiceTestsClassFixture _classFixture;
        private MockContosoRepo _mockContosoRepo;
        private ContosoService _contosoService;

        // Build input model objects and result model objects
        private ContosoModel _inputCreateContosoModel;
        private ContosoModel _updateContosoModel;
        private ContosoModel _resultContosoModel;
        private List<ContosoModel> _resultContosoModels;
        #endregion

        #region -- Constructor/Setup -----
        public ContosoServiceTests(ContosoServiceTestsClassFixture fixture, ITestOutputHelper output)
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
            // Arrange
            int expectedResult = 3;
            _mockContosoRepo = new MockContosoRepo().MockCreateContosoAsync(_resultContosoModel);
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Act
            var result = _contosoService.CreateContosoAsync(_inputCreateContosoModel);

            // Assert
            Assert.Equal(expectedResult, result.Result.Id);
        }

        [Fact]
        public void ContosoService_CreateContoso_Invalid()
        {
            // Arrange
            _mockContosoRepo = new MockContosoRepo().MockCreateContosoAsyncFails();
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Act
            var result = _contosoService.CreateContosoAsync(_inputCreateContosoModel);

            // Assert
            Assert.True(result.Result == null);
        }

        [Fact]
        public void ContosoService_GetContoso_Single_Valid()
        {
            // Arrange
            _mockContosoRepo = new MockContosoRepo().MockGetContosoAsync(_resultContosoModel);
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Act
            var result = _contosoService.GetContosoAsync(_resultContosoModel.Id);

            // Assert
            Assert.Equal(_resultContosoModel, result.Result);
        }

        [Fact]
        public void ContosoService_GetContoso_Single_Invalid()
        {
            // Arrange
            _mockContosoRepo = new MockContosoRepo().MockGetContosoAsyncFails();
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Act
            var result = _contosoService.GetContosoAsync(-1);

            // Assert
            Assert.True(result.Result == null);
        }

        [Fact]
        public void ContosoService_GetContoso_Multiple_Valid()
        {
            // Arrange
            _mockContosoRepo = new MockContosoRepo().MockGetContosoAsync(_resultContosoModels);
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Act
            var result = _contosoService.GetContosoAsync();

            // Assert
            Assert.Equal(_resultContosoModels, result.Result);
        }

        [Fact]
        public void ContosoService_GetContoso_Multiple_Invalid()
        {
            // Arrange
            _mockContosoRepo = new MockContosoRepo().MockGetContosoAsync(new List<ContosoModel>());
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Act
            var result = _contosoService.GetContosoAsync();

            // Assert
            Assert.True(result.Result.Count == 0);
        }

        [Fact]
        public void ContosoService_UpdateContoso_Valid()
        {
            // Arrange
            _mockContosoRepo = new MockContosoRepo().MockUpdateContosoAsync(_updateContosoModel);
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Act
            var result = _contosoService.UpdateContosoAsync(_updateContosoModel);

            // Assert
            Assert.Equal(_updateContosoModel, result.Result);
        }

        [Fact]
        public void ContosoService_UpdateContoso_Invalid()
        {
            // Arrange
            _mockContosoRepo = new MockContosoRepo().MockUpdateContosoAsyncFails();
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Act
            var result = _contosoService.UpdateContosoAsync(_updateContosoModel);

            // Assert
            Assert.True(result.Result == null);
        }

        [Fact]
        public void ContosoService_DeleteContoso_Valid()
        {
            // Arrange
            _mockContosoRepo = new MockContosoRepo()
                .MockDeleteContosoAsync(OperationResult.Deleted)
                .MockGetContosoAsync(_resultContosoModel);
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Act
            var result = _contosoService.DeleteContosoAsync(3);

            // Assert
            Assert.Equal(OperationResult.Deleted, result.Result);
            // Also validate that the Delete mock got called one time
            _mockContosoRepo.VerifyDeleteContosoAsync(Times.Once());


        }

        [Fact]
        public void ContosoService_DeleteContoso_Invalid()
        {
            // Arrange
            _mockContosoRepo = new MockContosoRepo()
                .MockDeleteContosoAsync(OperationResult.NotFound)
                .MockGetContosoAsyncFails();
            _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);

            // Act
            var result = _contosoService.DeleteContosoAsync(5);

            // Assert
            Assert.Equal(OperationResult.NotFound, result.Result);
            // Also validate that the Delete mock never got called
            _mockContosoRepo.VerifyDeleteContosoAsync(Times.Never());
        }

        #endregion
    }
}
