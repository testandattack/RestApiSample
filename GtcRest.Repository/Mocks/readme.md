# Repo Mocking Overview

### Repo Layer Methods. 
Each one needs at least one mock, but any methods that can return a `null` value will need two mocks.In addition we are using a Moq feature called [verify](https://docs.educationsmediagroup.com/unit-testing-csharp/moq/verifications#explicit-verification) to validate the behavior of the Delete method
```csharp
        Task<GtcModel> CreateGtcAsync(GtcModel GtcModel);
        Task<List<GtcModel>> GetGtcAsync();
        Task<GtcModel> GetGtcAsync(int Id);
        Task<GtcModel> UpdateGtcAsync(GtcModel Gtc);
        Task<OperationResult> DeleteGtcAsync(GtcModel GtcModel);
```


### Repo Mocks: 
Each Repo method needs at least one mock, but any repo methods that can return a `null` value will need two mocks, because we cannot pass a `null` value into the setup as a response. I will cover every Mock below. The design for this setup is based off of [creating fluent test classes](https://exceptionnotfound.net/using-moq-to-create-fluent-test-classes-in-asp-net-core/): We start off by creating a class that is based off the Mock of our GtcRepo Interface class:

```csharp
    public class MockGtcRepo : Mock<IGtcRepo>
```
This ensures we have the proper "contract" (from the Interface) for the Mock engine to build our mock. Next we create mock methods that will be used to initialize and instantiate the mocks during our testing. Here is the first method:   
```csharp
        // CreateGtcAsync(GtcModel GtcModel)
        public MockGtcRepo MockCreateGtcAsync(GtcModel result)
        {
            this.Setup(x => x.CreateGtcAsync(It.IsAny<GtcModel>())).ReturnsAsync(result);
            return this;
        }
```
Let's break down what each part is doing:
First, this mock is for the call CreateGtcAsync(GtcModel GtcModel)
```csharp
        // CreateGtcAsync(GtcModel GtcModel)
```
Next, is the method creation. It is
- a public method
- returns the current instance of the entire class
- is named by pre-pending the word 'Mock' to the method name it is mocking
- the passed in parameter will represent the response value that the mock should use
```csharp
        public MockGtcRepo MockCreateGtcAsync(GtcModel result)
```
Then, since this class is based off of Moq, we use the [Setup](https://documentation.help/Moq/98C52E48.htm) call in Moq, passing in the expression that this Moq is for the 'CreateGtcAsync' (```x => x.CreateGtcAsync```) method. We use the passed in parameter ```It``` to define the type of value that can legally be passed into the method, in this case ANY valid ```GtcModel```. Finally we say that the mock should return the ```GtcModel``` that was passed into this method.
```csharp
            this.Setup(x => x.CreateGtcAsync(It.IsAny<GtcModel>())).ReturnsAsync(result);
```
Finally, we pass back the the entire mock.
```csharp
            return this;
```
Below is the second method in our mocking class. This one uses the same name as the first method, EXCEPT that
- it adds 'Fails' to the name, indicating that this mock will simulate a failure of the repo method.
- it does not take a response object as a parameter
- it returns a null value.

```csharp
        // CreateGtcAsync(GtcModel GtcModel)
        public MockGtcRepo MockCreateGtcAsyncFails()
        {
            this.Setup(x => x.CreateGtcAsync(It.IsAny<GtcModel>())).ReturnsAsync((GtcModel)null);
            return this;
        }
```
Here is the Mock for the ```GetGtcAsync()``` repo call. For this method, we only need a single mock since the method is not SUPPOSED TO return null. If there are no items found, the repo returns an empty list. 
```csharp
        // GetGtcAsync()
        public MockGtcRepo MockGetGtcAsync(List<GtcModel> result)
        {
            this.Setup(x => x.GetGtcAsync()).ReturnsAsync(result);
            return this;
        }
```
For the ```GetGtcAsync(int id)``` repo call, we again need two mocks since it is possible to return a null value The first mock is used for mocking successful calls and the second for mocking failed calls. Also, notice that the ```It.IsAny()``` is now taking any valid integer (since that is the type of item passed into the repo method).
```csharp
        // GetGtcAsync(int id)
        public MockGtcRepo MockGetGtcAsync(GtcModel result)
        {
            this.Setup(x => x.GetGtcAsync(It.IsAny<int>())).ReturnsAsync(result);
            return this;
        }

        // GetGtcAsync(int id)
        public MockGtcRepo MockGetGtcAsyncFails()
        {
            this.Setup(x => x.GetGtcAsync(It.IsAny<int>())).ReturnsAsync((GtcModel)null);
            return this;
        }
```
The Update also requires two mocks...
```csharp
        //UpdateGtcAsync(GtcModel Gtc)
        public MockGtcRepo MockUpdateGtcAsync(GtcModel result)
        {
            this.Setup(x => x.UpdateGtcAsync(It.IsAny<GtcModel>())).ReturnsAsync(result);
            return this;
        }

        //UpdateGtcAsync(GtcModel Gtc)
        public MockGtcRepo MockUpdateGtcAsyncFails()
        {
            this.Setup(x => x.UpdateGtcAsync(It.IsAny<GtcModel>())).ReturnsAsync((GtcModel)null);
            return this;
        }
```
Now we get to the fun part. For simulating the DeleteGtcAsync(), we want to check both passed and failed, However, the failed part will be tested in a totally different way. If we look at the DeleteGtcAsync() service method, we see that it uses the GetGtcAsync(int id) repo call to see if the item exists before calling the Delete repo method. Since we have already created those mocks, we can re-use them. So we know that we will not call the Delete repo method unless there is an object to delete and we have gotten it.
```csharp
        // DeleteGtcAsync
        public MockGtcRepo MockDeleteGtcAsync(OperationResult result)
        {
            this.Setup(x => x.DeleteGtcAsync(It.IsAny<GtcModel>())).ReturnsAsync(result);
            return this;
        }
```
So how do we test the failure part? we could validate the results of the GetGtcAsync call, but I would rather check by seeing if the DeleteGtcRepo method gets called. In other words, if the GetGtcAsync fails, we should never call DeleteGtcAsync. So we use a Moq feature called [verify](https://docs.educationsmediagroup.com/unit-testing-csharp/moq/verifications#explicit-verification) to validate the behavior of the Delete method. If the Delete call is successful, the Delete mock will get called one time. If the Delete method is unsuccessful, the Delete mock will get called zero times.
```csharp
        // Verify how many times the call to the repo is made. For instance, if
        // the call to GetGtcModel fails, we will never call the Delete method in
        // the repo.
        public MockGtcRepo VerifyDeleteGtcAsync(Times times)
        {
            Verify(x => x.DeleteGtcAsync(It.IsAny<GtcModel>()), times);
            return this;
        }
```

