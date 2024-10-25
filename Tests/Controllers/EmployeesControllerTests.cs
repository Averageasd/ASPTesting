using EmployeesApp.Contracts;
using EmployeesApp.Controllers;
using EmployeesApp.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace Tests.Controllers
{
    public class EmployeesControllerTests
    {
        // with mock, we dont have to initialize all dependencies to return correct values.
        // make code simpler

        // test fails and no depdendecy isolation, we cannot tell if it fails because of errors in controller or repository
        private readonly Mock<IEmployeeRepository> _mockRepo;
        private readonly EmployeesController _controller;

        public EmployeesControllerTests()
        {

            // create mocked object for repository
            _mockRepo = new Mock<IEmployeeRepository>();

            // pass instance of mocked employee repo to controller
            _controller = new EmployeesController(_mockRepo.Object);

        }

        // verify that Index action returns a result of type ViewResult
        [Fact]
        public void Index_ActionExecutes_ReturnsViewForIndex()
        {
            var result = _controller.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_ActionExecutes_ReturnsExactNumberOfEmployees()
        {

            // mock method GetAll() of repo with Setup.
            // simply return list of employees
            // now this list of employees will be passed to View as shown
            // in method Index
            _mockRepo.Setup(repo => repo.GetAll())
                .Returns(new List<Employee>() { new Employee(), new Employee() });

            var result = _controller.Index();

            // verify that result is of type ViewResult
            // if successful, cast object to type ViewResult
            var viewResult = Assert.IsType<ViewResult>(result);

            // verify that model binded to viewResult is of type List<Employee>
            // if successful, cast object to type List<Employee>
            var employees = Assert.IsAssignableFrom<List<Employee>>(viewResult.Model);

            // check that there are 2 employees in list
            Assert.Equal(2, employees.Count);

        }

        [Fact]
        public void Create_ActionExecutes_ReturnsViewForCreate()
        {
            var createResult = _controller.Create();
            Assert.IsType<ViewResult>(createResult);
        }

        [Fact]
        public void Create_InvalidModelState_ReturnsView()
        {
            _controller.ModelState.AddModelError("Name", "Name is required");

            // invalid employee without name
            var employee = new Employee { Age = 25, AccountNumber = "255-8547963214-41" };

            // pass invalid employee object to method
            // we will get view that has employee model binded to it
            var result = _controller.Create(employee);

            var viewResult = Assert.IsType<ViewResult>(result);

            // verify that Model binded to viewResult is of type Employee
            // if it is, converted it into object of corresponding type
            var testEmployee = Assert.IsType<Employee>(viewResult.Model);

            // make sure we get the same employee back
            Assert.Equal(employee.AccountNumber, testEmployee.AccountNumber);
            Assert.Equal(employee.Age, testEmployee.Age);
        }

        [Fact]
        public void Create_InvalidModelState_CreateEmployeeNeverExecutes()
        {
            _controller.ModelState.AddModelError("Name", "Name is required");
            var employee = new Employee { Age = 34 };

            // create invalid employee
            _controller.Create(employee);

            // Verify to test whether the method is called with correct amount of times,
            // with correct arguments and has expected behavior
            // since we pass in an invalid employee, we will return a View that is binded
            // to employee model.
            // in this case, CreateEmployee is never executed. 
            _mockRepo.Verify(x => x.CreateEmployee(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public void Create_ModelStateValid_CreateEmployeeCalledOnce()
        {
            Employee? emp = null;

            _mockRepo.Setup(r => r.CreateEmployee(It.IsAny<Employee>()))
                // capture argument passed into CreateEmployee, grab it and assign it
                // to variable emp
                .Callback<Employee>(x => emp = x);

            // valid employee object
            var employee = new Employee
            {
                Name = "Test Employee",
                Age = 32,
                AccountNumber = "123-5435789603-21"
            };

            // call Create with valid employee
            // now createEmployee of employeeRepo will be called
            // and we can access to argument of this method and assign the passed-in employee to emp
            _controller.Create(employee);

            // verify that createEmployee is called once for valid employee
            _mockRepo.Verify(x => x.CreateEmployee(It.IsAny<Employee>()), Times.Once);

            // verify that we get back same employee object after calling Create
            Assert.Equal(emp.Name, employee.Name);
            Assert.Equal(emp.Age, employee.Age);
            Assert.Equal(emp.AccountNumber, employee.AccountNumber);
        }

        [Fact]
        public void Create_ActionExecuted_RedirectsToIndexAction()
        {   
            // valid employee object
            var employee = new Employee
            {
                Name = "Test Employee",
                Age = 45,
                AccountNumber = "123-4356874310-43"
            };

            // after we call Create with valid employee, method redirects to Index
            var result = _controller.Create(employee);

            // we will get back object of RedirectToActionResult after adding a valid employee
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            // verify that mockRepo is called once
            _mockRepo.Verify(x => x.CreateEmployee(It.IsAny<Employee>()), Times.Once);

            // the action name we redirect to should be Index
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
