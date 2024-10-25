namespace IntegrationTests
{
    public class EmployeesControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;

        public EmployeesControllerIntegrationTests(TestingWebAppFactory<Program> appFactory)
        {
            _client = appFactory.CreateClient();
        }

        [Fact]
        public async Task Index_WhenCalled_ReturnsApplicationForm()
        {
            var response = await _client.GetAsync("/Employees");

            response.EnsureSuccessStatusCode();

            // convert response to a single string
            var responseString = await response.Content.ReadAsStringAsync();

            // make sure response 
            Assert.Contains("Mark", responseString);
            Assert.Contains("Evelin", responseString);
        }

        [Fact]
        public async Task Create_WhenCalled_ReturnsCreateForm()
        {
            var response = await _client.GetAsync("/Employees/Create");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Please provide a new employee data", responseString);
        }

        [Fact]
        public async Task Create_SentWrongModel_ReturnsViewWithErrorMessages()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Employees/Create");

            // invalid form model without account number
            var formModel = new Dictionary<string, string>
            {
                { "Name", "New Employee" },
                { "Age", "25" }
            };

            // content we will send to server
            postRequest.Content = new FormUrlEncodedContent(formModel);
            var postResponse = await _client.SendAsync(postRequest);
            postResponse.EnsureSuccessStatusCode();
            var responseString = await postResponse.Content.ReadAsStringAsync();
            Assert.Contains("Account number is required", responseString);
        }

        [Fact]
        public async Task Create_SentRightModel_ReturnsRedirectionToIndex()
        {
            // Arrange 
            // prepare post request
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Employees/Create");
            var formModel = new Dictionary<string, string>
            {
                { "Name", "New Employee" },
                { "Age", "25" },
                { "AccountNumber", "214-5874986532-21" }
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            // act
            // send post request with valid employee data to server
            var postResponse = await _client.SendAsync(postRequest);

            // result
            // make sure status code is 200
            postResponse.EnsureSuccessStatusCode();
            var responseString = await postResponse.Content.ReadAsStringAsync();

            // response contains data of newly inserted employee
            Assert.Contains("New Employee", responseString);
            Assert.Contains("25", responseString);

        }
    }
}
