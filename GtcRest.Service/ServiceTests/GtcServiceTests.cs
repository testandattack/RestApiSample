using GtcRest.Models.Domain;
using GtcRest.Models.Enum;
using GtcRest.Models.Shared;
using GtcRest.Repository.Mocks;
using GtcRest.Service.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace GtcRest.Service.ServiceTests
{
    public class GtcServiceTests : IClassFixture<GtcServiceTestsClassFixture>
    {
        #region -- Properties -----
        // Define all of the "injectable" items to allow the execution of the 
        // service code
        private GtcServiceTestsClassFixture _classFixture;
        private MockGtcRepo _mockGtcRepo;
        private IOptionsSnapshot<Settings> _settings;
        private GtcService _gtcService;

        // Build an input model object and a result model object
        private GtcModel _inputCreateGtcModel;
        private GtcModel _updateGtcModel;
        private GtcModel _resultGtcModel;
        private List<GtcModel> _resultGtcModels;
        #endregion

        #region -- Constructor/Setup -----
        public GtcServiceTests(GtcServiceTestsClassFixture fixture, ITestOutputHelper output)
        {
            _classFixture = fixture;
            _settings = Settings.CreateIOptionSnapshotMock(_classFixture.settings);
            _classFixture.ConfigureLogging(output);
            ParameterSetup();
        }

        private void ParameterSetup()
        {
            // Since this is for input, we do not want an Id. It will be assigned when 
            // the model is created.
            _inputCreateGtcModel = new GtcModel()
            {
                Description = "Third GtcModel"
            };

            _updateGtcModel = new GtcModel()
            {
                Id = 3,
                Description = "Modified GtcModel"
            };

            _resultGtcModel = new GtcModel()
            {
                Id = 3,
                Description = "Third GtcModel"
            };

            _resultGtcModels = new List<GtcModel>()
            {
                new GtcModel() { Id = 1, Description = "Original GtcModel"},
                new GtcModel() { Id = 3, Description = "Third GtcModel"},
            };
        }
        #endregion

        #region -- Test Methods -----
        [Fact]
        public void GtcService_CreateGtc_Valid()
        {
            // Arrange
            int expectedResult = 3;
            _mockGtcRepo = new MockGtcRepo().MockCreateGtcAsync(_resultGtcModel);
            _gtcService = new GtcService(_settings, new NullLogger<GtcService>(), _mockGtcRepo.Object);

            // Act
            var result = _gtcService.CreateGtcAsync(_inputCreateGtcModel);

            // Assert
            Assert.Equal(expectedResult, result.Result.Id);
        }

        [Fact]
        public void GtcService_CreateGtc_Invalid()
        {
            // Arrange
            _mockGtcRepo = new MockGtcRepo().MockCreateGtcAsyncFails();
            _gtcService = new GtcService(_settings, new NullLogger<GtcService>(), _mockGtcRepo.Object);

            // Act
            var result = _gtcService.CreateGtcAsync(_inputCreateGtcModel);

            // Assert
            Assert.True(result.Result == null);
        }

        [Fact]
        public void GtcService_GetGtc_Single_Valid()
        {
            // Arrange
            _mockGtcRepo = new MockGtcRepo().MockGetGtcAsync(_resultGtcModel);
            _gtcService = new GtcService(_settings, new NullLogger<GtcService>(), _mockGtcRepo.Object);

            // Act
            var result = _gtcService.GetGtcAsync(_resultGtcModel.Id);

            // Assert
            Assert.Equal(_resultGtcModel, result.Result);
        }

        [Fact]
        public void GtcService_GetGtc_Single_Invalid()
        {
            // Arrange
            _mockGtcRepo = new MockGtcRepo().MockGetGtcAsyncFails();
            _gtcService = new GtcService(_settings, new NullLogger<GtcService>(), _mockGtcRepo.Object);

            // Act
            var result = _gtcService.GetGtcAsync(-1);

            // Assert
            Assert.True(result.Result == null);
        }

        [Fact]
        public void GtcService_GetGtc_Multiple_Valid()
        {
            // Arrange
            _mockGtcRepo = new MockGtcRepo().MockGetGtcAsync(_resultGtcModels);
            _gtcService = new GtcService(_settings, new NullLogger<GtcService>(), _mockGtcRepo.Object);

            // Act
            var result = _gtcService.GetGtcAsync();

            // Assert
            Assert.Equal(_resultGtcModels, result.Result);
        }

        [Fact]
        public void GtcService_GetGtc_Multiple_Invalid()
        {
            // Arrange
            _mockGtcRepo = new MockGtcRepo().MockGetGtcAsync(new List<GtcModel>());
            _gtcService = new GtcService(_settings, new NullLogger<GtcService>(), _mockGtcRepo.Object);

            // Act
            var result = _gtcService.GetGtcAsync();

            // Assert
            Assert.True(result.Result.Count == 0);
        }

        [Fact]
        public void GtcService_UpdateGtc_Valid()
        {
            // Arrange
            _mockGtcRepo = new MockGtcRepo().MockUpdateGtcAsync(_updateGtcModel);
            _gtcService = new GtcService(_settings, new NullLogger<GtcService>(), _mockGtcRepo.Object);

            // Act
            var result = _gtcService.UpdateGtcAsync(_updateGtcModel);

            // Assert
            Assert.Equal(_updateGtcModel, result.Result);
        }

        [Fact]
        public void GtcService_UpdateGtc_Invalid()
        {
            // Arrange
            _mockGtcRepo = new MockGtcRepo().MockUpdateGtcAsyncFails();
            _gtcService = new GtcService(_settings, new NullLogger<GtcService>(), _mockGtcRepo.Object);

            // Act
            var result = _gtcService.UpdateGtcAsync(_updateGtcModel);

            // Assert
            Assert.True(result.Result == null);
        }

        [Fact]
        public void GtcService_DeleteGtc_Valid()
        {
            // Arrange
            _mockGtcRepo = new MockGtcRepo()
                .MockDeleteGtcAsync(OperationResult.Deleted)
                .MockGetGtcAsync(_resultGtcModel);
            _gtcService = new GtcService(_settings, new NullLogger<GtcService>(), _mockGtcRepo.Object);

            // Act
            var result = _gtcService.DeleteGtcAsync(3);

            // Assert
            Assert.Equal(OperationResult.Deleted, result.Result);


        }

        [Fact]
        public void GtcService_DeleteGtc_Invalid()
        {
            // Arrange
            _mockGtcRepo = new MockGtcRepo()
                .MockDeleteGtcAsync(OperationResult.NotFound)
                .MockGetGtcAsyncFails();
            _gtcService = new GtcService(_settings, new NullLogger<GtcService>(), _mockGtcRepo.Object);

            // Act
            var result = _gtcService.DeleteGtcAsync(5);

            // Assert
            Assert.Equal(OperationResult.NotFound, result.Result);
            // Also validate that the Delete mock never got called
            _mockGtcRepo.VerifyDeleteGtcAsync(Times.Never());
        }

        #endregion
    }
}
