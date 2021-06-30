﻿using GtcRest.Interfaces.Repository;
using GtcRest.Models.Domain;
using GtcRest.Models.Enum;
using Moq;
using System.Collections.Generic;

namespace GtcRest.Repository.Mocks
{
    // Class design based on https://exceptionnotfound.net/using-moq-to-create-fluent-test-classes-in-asp-net-core/

    public class MockGtcRepo : Mock<IGtcRepo>
    {

        // Setup the CreateGtcAsync call so that it can return either 
        // a successful GtcModel or null (if the call fails).
        public MockGtcRepo MockCreateGtcAsync(GtcModel result)
        {
            this.SetupSequence(x => x.CreateGtcAsync(It.IsAny<GtcModel>()))
                .ReturnsAsync(result)
                .ReturnsAsync((GtcModel)null);
            return this;
        }

        // GetGtcAsync(int id)
        public MockGtcRepo MockGetGtcAsync(GtcModel result)
        {
            this.Setup(x => x.GetGtcAsync(It.IsAny<int>())).ReturnsAsync(result);
            return this;
        }

        // GetGtcAsync()
        public MockGtcRepo MockGetGtcAsync(List<GtcModel> result)
        {
            this.Setup(x => x.GetGtcAsync()).ReturnsAsync(result);
            return this;
        }

        // Setup the UpdateGtcAsync call so that it can return either 
        // a successful GtcModel or null (if the call fails).
        public MockGtcRepo MockUpdateGtcAsync(GtcModel result)
        {
            this.SetupSequence(x => x.UpdateGtcAsync(It.IsAny<GtcModel>()))
                .ReturnsAsync(result)
                .ReturnsAsync((GtcModel)null);
            return this;
        }

        // DeleteGtcAsync
        public MockGtcRepo MockDeleteGtcAsync(OperationResult result)
        {
            this.Setup(x => x.DeleteGtcAsync(It.IsAny<GtcModel>())).ReturnsAsync(result);
            return this;
        }

        // Verify how many times the call to the repo is made. For instance, if
        // the call to GetGtcModel fails, we will never call the Delete method in
        // the repo.
        public MockGtcRepo VerifyDeleteGtcAsync(Times times)
        {
            Verify(x => x.DeleteGtcAsync(It.IsAny<GtcModel>()), times);
            return this;
        }
    }
}