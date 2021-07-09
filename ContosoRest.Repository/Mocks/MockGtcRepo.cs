using ContosoRest.Interfaces.Repository;
using ContosoRest.Models.Domain;
using ContosoRest.Models.Enum;
using Moq;
using System.Collections.Generic;

namespace ContosoRest.Repository.Mocks
{
    // Class design based on https://exceptionnotfound.net/using-moq-to-create-fluent-test-classes-in-asp-net-core/

    public class MockContosoRepo : Mock<IContosoRepo>
    {
        // CreateContosoAsync(ContosoModel ContosoModel)
        public MockContosoRepo MockCreateContosoAsync(ContosoModel result)
        {
            this.Setup(x => x.CreateContosoAsync(It.IsAny<ContosoModel>())).ReturnsAsync(result);
            return this;
        }

        public MockContosoRepo MockCreateContosoAsyncFails()
        {
            this.Setup(x => x.CreateContosoAsync(It.IsAny<ContosoModel>())).ReturnsAsync((ContosoModel)null);
            return this;
        }

        // GetContosoAsync()
        public MockContosoRepo MockGetContosoAsync(List<ContosoModel> result)
        {
            this.Setup(x => x.GetContosoAsync()).ReturnsAsync(result);
            return this;
        }
        
        // GetContosoAsync(int id)
        public MockContosoRepo MockGetContosoAsync(ContosoModel result)
        {
            this.Setup(x => x.GetContosoAsync(It.IsAny<int>())).ReturnsAsync(result);
            return this;
        }

        public MockContosoRepo MockGetContosoAsyncFails()
        {
            this.Setup(x => x.GetContosoAsync(It.IsAny<int>())).ReturnsAsync((ContosoModel)null);
            return this;
        }

        //UpdateContosoAsync(ContosoModel Contoso)
        public MockContosoRepo MockUpdateContosoAsync(ContosoModel result)
        {
            this.Setup(x => x.UpdateContosoAsync(It.IsAny<ContosoModel>())).ReturnsAsync(result);
            return this;
        }

        public MockContosoRepo MockUpdateContosoAsyncFails()
        {
            this.Setup(x => x.UpdateContosoAsync(It.IsAny<ContosoModel>())).ReturnsAsync((ContosoModel)null);
            return this;
        }

        // DeleteContosoAsync
        public MockContosoRepo MockDeleteContosoAsync(OperationResult result)
        {
            this.Setup(x => x.DeleteContosoAsync(It.IsAny<ContosoModel>())).ReturnsAsync(result);
            return this;
        }

        // Verify how many times the call to the repo is made. For instance, if
        // the call to GetContosoModel fails, we will never call the Delete method in
        // the repo.
        public MockContosoRepo VerifyDeleteContosoAsync(Times times)
        {
            Verify(x => x.DeleteContosoAsync(It.IsAny<ContosoModel>()), times);
            return this;
        }
    }
}
