using GtcRest.Models.Domain;
using GtcRest.Models.Shared;
using GtcRest.Repository.Mocks;
using GtcRest.Service.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
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
        private GtcModel _UpdateGtcModel;
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
            TestSetup();
        }

        private void TestSetup()
        {
            // Create the mocks we will need for the testing
            _mockGtcRepo = new MockGtcRepo()
                .MockCreateGtcAsync(_resultGtcModel)
                .MockGetGtcAsync(_resultGtcModel)
                .MockGetGtcAsync(_resultGtcModels)
                .MockUpdateGtcAsync(_UpdateGtcModel)
                .MockDeleteGtcAsync(Models.Enum.OperationResult.Deleted);

            //Setup the Service
            _gtcService = new GtcService(_settings, new NullLogger<GtcService>(), _mockGtcRepo.Object);
        }

        private void ParameterSetup()
        {
            // Since this is for input, we do not want an Id. It will be assigned when 
            // the model is created.
            _inputCreateGtcModel = new GtcModel()
            {
                Description = "Third GtcModel"
            };

            _UpdateGtcModel = new GtcModel()
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
        public void GtcService_CreateGtc()
        {
            Log.ForContext<GtcServiceTests>().Information("Starting test GtcService_CreateGtc_Valid");
            // Arrange
            int expectedResult = 3;

            // Act - Pass one
            var result = _gtcService.CreateGtcAsync(_inputCreateGtcModel);

            // Assert
            Assert.Equal(expectedResult, result.Result.Id);

            // Act - Pass two
            result = _gtcService.CreateGtcAsync(_inputCreateGtcModel);

            // Assert Pass two
            Assert.True(result == null);
        }
        #endregion
    }
}
