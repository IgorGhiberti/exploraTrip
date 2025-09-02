using Xunit;
using Domain.Entities;
using Bogus;
using Domain.ValueObjects;

namespace Tests.Entities
{
    [Trait("Entities", "User")]
    public sealed class UserTests
    {
        private readonly Faker _faker = new("pt_BR");
        private readonly User _user;

        public UserTests()
        {
            _user = new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool());
        }

        [Fact]
        [Trait("Constructor", "")]
        public void Constructor_GivenAllParameters_ThenShouldSetThePropertiesCorrectly()
        {
            //Arrange
            var expectedEmail = _faker.Person.Email;
            var expectedUserName = _faker.Person.FullName;
            var expectedHashPassword = _faker.Random.AlphaNumeric(20);
            var expectedActive = true;

            var user = new User(expectedEmail, expectedUserName, expectedHashPassword, expectedEmail, expectedActive);

            //Assert
            Assert.Equal(expectedEmail, user.Email!.Value);
            Assert.Equal(expectedHashPassword, user.HashPassword);
            Assert.Equal(expectedUserName, user.UserName);
            Assert.Equal(expectedActive, user.Active);
        }   

        [Trait("Method", "Update")]
        [Fact]
        public void UpdateUserMethod_GivenSomeParameter_ThenShouldSetThePropertiesCorrectly()
        {
            string? expectedEmail = _faker.Person.Email.OrNull(_faker, 0.3f);
            string? expectedUserName = _faker.Person.FullName.OrNull(_faker, 0.1f);

            _user.UpdateUser(expectedEmail, expectedUserName);

            if (!string.IsNullOrWhiteSpace(expectedEmail))
                Assert.Equal(expectedEmail, _user.Email!.Value);
            Assert.Equal(_user.Email!.Value, _user.Email!.Value);

            if (!string.IsNullOrWhiteSpace(expectedUserName))
                Assert.Equal(expectedUserName, _user.UserName);
            Assert.Equal(_user.UserName, _user.UserName);
        }

        [Trait("Method", "ActiveUser")]
        [Fact]
        public void ActiveUserMethod_GivenAnyUserActiveParameter_ThenShouldSetActiveEqualsTrue()
        {
            _user.ActivateUser();

            Assert.True(_user.Active);
        }

        [Trait("Method", "DisableUser")]
        [Fact]
        public void DisableUserMethod_GivenAnyUserActiveParameter_ThenShouldSetActiveEqualsFalse()
        {
            _user.DisableUser();

            Assert.False(_user.Active);
        }

        [Trait("Method", "UpdateHashPassword")]
        [Fact]
        public void UpdateHashPasswordMethod_GivenAnyStringParameter_ThenShouldSetTheNewHashPassword()
        {
            string expectedHash = _faker.Lorem.Text();
            _user.UpdateHashPassword(expectedHash);

            Assert.Equal(expectedHash, _user.HashPassword);
        }

        [Trait("VO", "Email")]
        [Trait("Method", "Create")]
        [Fact]
        public void EmailCreateMethod_GivenValidEmail_ShouldReturnSuccessResultWithCorrectValue()
        {
            string expectedEmailVal = _faker.Person.Email;

            var result = Email.Create(expectedEmailVal);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedEmailVal, result.Data!.Value);
        }

        [Trait("VO", "Email")]
        [Trait("Method", "Create")]
        [Fact]
        public void EmailCreateMethod_GivenInvalidEmail_ShouldReturnUnsuccessfulResultWithErrorMessage()
        {
            string expectedIncorrectEmail = _faker.Person.FullName;
            string expectedMessage = "Invalid email format.";

            var result = Email.Create(expectedIncorrectEmail);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(result.Message, expectedMessage);
        }
    }
}