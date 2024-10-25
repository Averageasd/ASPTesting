using EmployeesApp.Validation;

namespace Tests.Validation
{
    public class AccountNumberValidationTest
    {
        private readonly AccountNumberValidation _accountNumberValidation;

        public AccountNumberValidationTest()
        {
            _accountNumberValidation = new AccountNumberValidation();
        }

        [Fact]
        public void IsValid_ValidAccountNumber_ReturnsTrue()
        {
            Assert.True(_accountNumberValidation.IsValid("123-4543234576-23"));
        }

        // using theory: provide some data to this method as a parameter
        [Theory]

        // data provided as parameters to the method
        [InlineData("1234-3454565676-23")]
        [InlineData("12-3454565676-23")]
        public void IsValid_AccountNumberFirstPartWrong_ReturnsFalse(string accountNumber)
        {
            Assert.False(_accountNumberValidation.IsValid(accountNumber));
        }

        // account validation should return false because middle part does not have right amount of digits: 10
        [Theory]
        [InlineData("123-345456567-23")]
        [InlineData("123-345456567633-23")]
        public void IsValid_AccountNumberMiddlePartWrong_ReturnsFalse(string accNumber)
        {
            Assert.False(_accountNumberValidation.IsValid(accNumber));
        }

        // account validation should return false because last part does not have right amount of digits : 2
        [Theory]
        [InlineData("123-3434545656-2")]
        [InlineData("123-3454565676-233")]
        public void IsValid_AccountNumberLastPartWrong_ReturnsFalse(string accNumber)
        {
            Assert.False(_accountNumberValidation.IsValid(accNumber));
        }

        // throw arguments since the acc numbers contain "invalid" delimeters
        [Theory]
        [InlineData("123-345456567633=23")]
        [InlineData("123+345456567633-23")]
        [InlineData("123+345456567633=23")]
        public void IsValid_InvalidDelimeters_ThrowsArgumentsException(string accNumber)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _accountNumberValidation.IsValid(accNumber);
            });
        }
    }
}
