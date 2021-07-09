## Definitions
Most of these definitions come from Gerard Maszaros' [web site](http://xunitpatterns.com/index.html).
- **SUT** _System Under Test_: The part of the application being tested
- **DOC** _Depended on Component_: The component(s) that the SUT depend on to properly function.
- **Test Double** a black box component that mimics all the inputs and outputs of the real component (the DOC) it is replacing.
- **[Mock](http://xunitpatterns.com/Mock%20Object.html)** A powerful way to implement [Behavior Verification](http://xunitpatterns.com/Behavior%20Verification.html) while avoiding [Test Code Duplication](http://xunitpatterns.com/Test%20Code%20Duplication.html) between similar tests by delegating the job of verifying the [indirect outputs](http://xunitpatterns.com/indirect%20output.html) of the SUT entirely to a Test Double

NOTE about implementing Mocks: "_tests written using Mock Objects look different from more traditional tests because all the expected behavior must be specified before the SUT is exercised. This makes them harder to write and to understand for test automation neophytes. This factor may be enough to cause us to prefer writing our tests using Test Spys._"

## xUnit Test Layout for the Solution
There are several places where testing and test double code reside in this solution, and each place has a specific reason for being used.

#### Extensions.Tests Project
This project contains all tests and necessary test artifacts for executing automated tests against the **Extensions** project. Since the Extensions project contains only a bunch of extension methods, there is no need to create any test doubles. This is a situation where creating a separate project that only contains test code is preferred. It allows the testing to be done at any level, but it also allows you to easily exclude publishing the test code to production.

#### Mocks folders in various component projects
Projects like the ContosoRest.Repo project contain a Mocks folder. Inside there, you will see a Mock file for every Repo class in the Repos folder. By creating fluent extension methods for our mocked objects, we can easily determine what behaviors and patterns should be mocked while we build the Repo code. In other words, create your mock while you create the object it is mocking. For more information, see [this article](https://dev.azure.com/collins-devops/Testing%20the%20Testing%20Strategy/_git/RestApiSample?path=%2FContosoRest.Repository%2FMocks%2Freadme.md&_a=preview)

#### Tests folders in various component projects
The ContosoRest.Service project contains a 'ServiceTests' folder. In there you will find a test class with the unit tests. For more information, see  [this article](https://dev.azure.com/collins-devops/Testing%20the%20Testing%20Strategy/_git/RestApiSample?path=%2FContosoRest.Service%2FServiceTests%2Freadme.md&_a=preview)

#### Shared folder in the Models project
Here you will find the various [class fixture](https://xunit.net/docs/shared-context#class-fixture) files for use in the tests.

## Further Reading
- [Test Doubles](http://xunitpatterns.com/Test%20Double.html)
- [xUnit Theory Tests](https://exceptionnotfound.net/using-xunit-theory-and-inlinedata-to-test-c-extension-methods/)
- [Creating Strongly Typed Theory Data](https://andrewlock.net/creating-strongly-typed-xunit-theory-test-data-with-theorydata/)
- [Controlling the Order of xUnit Test Execution](https://docs.microsoft.com/en-us/dotnet/core/testing/order-unit-tests?pivots=xunit)
- [Fluent Test Cases with Moq](https://exceptionnotfound.net/using-moq-to-create-fluent-test-classes-in-asp-net-core/)
- [Unit Testing with ILogger&lt;T&gt;](https://codeburst.io/unit-testing-with-net-core-ilogger-t-e8c16c503a80)
- [Moq Quick Start](https://github.com/Moq/moq4/wiki/Quickstart)