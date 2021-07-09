# Service Testing Overview
### Service Layer Methods
```csharp
        Task<ContosoModel> CreateContosoAsync(ContosoModel contoso);
        Task<List<ContosoModel>> GetContosoAsync();
        Task<ContosoModel> GetContosoAsync(int id);
        Task<ContosoModel> UpdateContosoAsync(ContosoModel contoso);
        Task<OperationResult> DeleteContosoAsync(int id);
```

## Needed Tests
Each exposed ervice method needs a `passed` and `failed` test case. NOTE: There should be at least one test for every scenario that could happen within each method, so you may end up with more than two test methods for each service layer method.  
```csharp
        public void ContosoService_CreateContoso_Valid()
        public void ContosoService_CreateContoso_Invalid()
        public void ContosoService_GetContoso_Single_Valid()
        public void ContosoService_GetContoso_Single_Invalid()
        public void ContosoService_GetContoso_Multiple_Valid()
        public void ContosoService_GetContoso_Multiple_Invalid()
        public void ContosoService_UpdateContoso_Valid()
        public void ContosoService_UpdateContoso_Invalid()
        public void ContosoService_DeleteContoso_Valid()
        public void ContosoService_DeleteContoso_Invalid()
```
So far, all information needed has already been provided by the application. In order to write the tests described in the above list, we will need to setup a series of mocks and corresponding data.

### Repo Mocks: 
Each Repo method needs at least one mock, but any repo methods that can return a `null` value will need two mocks, because we cannot pass a `null` value into the setup as a response. All of the below Mocks should already exist in the Repo project.

```csharp
        // CreateContosoAsync(ContosoModel ContosoModel)
        public MockContosoRepo MockCreateContosoAsync(ContosoModel result)
        public MockContosoRepo MockCreateContosoAsyncFails()

        // GetContosoAsync()
        public MockContosoRepo MockGetContosoAsync(List<ContosoModel> result)
        
        // GetContosoAsync(int id)
        public MockContosoRepo MockGetContosoAsync(ContosoModel result)
        public MockContosoRepo MockGetContosoAsyncFails()

        //UpdateContosoAsync(ContosoModel Contoso)
        public MockContosoRepo MockUpdateContosoAsync(ContosoModel result)
        public MockContosoRepo MockUpdateContosoAsyncFails()

        // DeleteContosoAsync
        public MockContosoRepo MockDeleteContosoAsync(OperationResult result)

        // Verify how many times the call to the repo is made.
        public MockContosoRepo VerifyDeleteContosoAsync(Times times)
```

### The Test Class
Since we are using .NET Core Services, we need a way to setup and "host the services." For this we use a couple of features of xUnit, Class level variables and [Fixtures](https://xunit.net/docs/shared-context). Here is the important part to remember about xUnit testing and shared context: 

- Given a test class with a class fixture, a class constructor and a few different test methods, if you tell the test runner to execute all the tests, the following happens:
    1. the test runner will instantiate the fixture
    2. Any objects created inside the fixture constructor only get creted once and remain in scope for every test execution.
    3. the test runner will select a test to execute and instantiate the test class, passing in the created fixture using DI.
    4. Any objects inside the test class constructor get created, the test method gets executed.
    5. The class gets disposed.
    6. The runner goes back to step iii and repeat until all tests complete.
    7. The fixture and objects get disposed.

The following code defines the class fixture to use for the class:
```csharp
public class ContosoServiceTests : IClassFixture<ContosoServiceTestsClassFixture>
```
The following code defines the injectable objects to be used throughout the test code:
```csharp
        private ContosoServiceTestsClassFixture _classFixture;
        private MockContosoRepo _mockContosoRepo;
        private ContosoService _contosoService;
```
The following code defines all of the necessary input objects and expected output objects:
```csharp
        private ContosoModel _inputCreateContosoModel;
        private ContosoModel _updateContosoModel;
        private ContosoModel _resultContosoModel;
        private List<ContosoModel> _resultContosoModels;
```
The constructor has a couple of items of note. First, the (already created) class fixture is injected into the constructor, where it is assigned to our class variable. Second, we have added an [xUnit abstraction class](https://xunit.net/docs/capturing-output) used to facilitate the output of test info. By adding the abstraction to the arguments of the constructor, the xUnit runner will automatically inject the class into our tests. I then pass that class into the logger for the fixture (_I will show how that works in a minute_). The final line calls a method that instantiates all of our input and output objects.
```csharp
        public ContosoServiceTests(ContosoServiceTestsClassFixture fixture, ITestOutputHelper output)
        {
            _classFixture = fixture;
            _classFixture.ConfigureLogging(output);
            ParameterSetup();
        }
```
For the testing here, I am using Fact based test methods (parameter-less) here. To see the use of Theory based test methods, view the tests in the Extensions project for this application. As stated earlier, I create one "Pass" and one "Fail" test for each method in the layer I am testing. The nming convention I suggest using is as follows:
```csharp
<testLayerName>_<MethodName>_<Valid/Invalid>
```
Here we see a test that will execute the 'CreateContoso' method inside the 'ContosoService'
```csharp
        [Fact]
        public void ContosoService_CreateContoso_Valid()
```
### The Parts of the test itself
I follow the Arrange, Act, Assert format for my unit tests. 
**Arrange**
- First, we set an expected result.
- Next we create the necessary Repo Mock that the service will need.
- Finally we start the actual service, passing in the objects that the Service receives when being loaded inside the ASP.NET Core application:
    - an IOptionsSnapshot object that contains the settings for the application (details for this are below)
    - a Logger Service (we pass in the null logger service because we don't need to capture application logging during testing).
    - the Mocked ContosoRepo we just created
```csharp
    // Arrange
    int expectedResult = 3;
    _mockContosoRepo = new MockContosoRepo().MockCreateContosoAsync(_resultContosoModel);
    _contosoService = new ContosoService(_classFixture.snapshotSettings, new NullLogger<ContosoService>(), _mockContosoRepo.Object);
```
**Act**
We call the method being tested, passing in one of the parameters we created in the constructor, and assign the response to a variable
```csharp
            // Act
            var result = _contosoService.CreateContosoAsync(_inputCreateContosoModel);
```
**Assert**
We assert the expected response against the actual response
```csharp
            // Assert
            Assert.Equal(expectedResult, result.Result.Id);
```
### Handling methods that can return a null value
Earlier, I mentioned the need to create a separate Mock for handling tests where a null response needs to be produced. The CreateContosoAsync call is one where a null can be returned. This is why the Arrange and Assert portions of the ```ContosoService_CreateContoso_Invalid()``` method are different than the same parts of the ```ContosoService_CreateContoso_Valid``` method.
- We call a different Mock setup.
- We do not pass in an expected response (you can't pass in a ```null``` value).
- We have to Assert against an evaluation of the result. xUnit does not provide any mechanism to test for null  (nor does it need to if you use this methodology).
```csharp
            // Arrange
            _mockContosoRepo = new MockContosoRepo().MockCreateContosoAsyncFails();
```

```csharp
            // Assert
            Assert.True(result.Result == null);
```
### Handling methods that make multiple Repo calls
the DeleteContosoAsync() call in our service makes a call to `GetContosoAsync()` in the Repo, and a call to `DeleteContosoAsync()` in the Repo:
```csharp
        public async Task<OperationResult> DeleteContosoAsync(int id)
        {
            var contoso = await _contosoRepo.GetContosoAsync(id);
            if(contoso == null)
            {
                return OperationResult.NotFound;
            }
            return await _contosoRepo.DeleteContosoAsync(contoso);
        }
```
Therefore, we need to mock both of those calls in our test setups For the test that works, it looks like this:
```csharp
            // Arrange
            _mockContosoRepo = new MockContosoRepo()
                .MockDeleteContosoAsync(OperationResult.Deleted)
                .MockGetContosoAsync(_resultContosoModel);
```
In the test that fails, it looks like this:
```csharp
            // Arrange
            _mockContosoRepo = new MockContosoRepo()
                .MockDeleteContosoAsync(OperationResult.NotFound)
                .MockGetContosoAsyncFails();
```
Now we can complete the tests.

### Using the Verify Moq feature
In our Repo Mock, we created a mock for successful delete calls. We also created a [verify](https://docs.educationsmediagroup.com/unit-testing-csharp/moq/verifications#explicit-verification) mock that is tied to the Repo's DeleteContosoAsync call. To use that verification, we add a call to each test that validates how many times the Delete mock gets called. 
For the **Valid** test:
```csharp
            // Assert
            Assert.Equal(OperationResult.Deleted, result.Result);
            // Also validate that the Delete mock got called one time
            _mockContosoRepo.VerifyDeleteContosoAsync(Times.Once());
```
For the **Invalid** test:
```csharp
            // Assert
            Assert.Equal(OperationResult.NotFound, result.Result);
            // Also validate that the Delete mock never got called
            _mockContosoRepo.VerifyDeleteContosoAsync(Times.Never());
```
