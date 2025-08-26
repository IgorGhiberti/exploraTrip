using Xunit;
using Domain.User;
using Bogus;
using Domain.ValueObjects;

namespace Tests.Entities
{
    public sealed class UserTests
    {
        private readonly Faker _faker = new("pt_BR");
        private readonly User _user;

        public UserTests()
        {
            _user = new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool());
        }

        [Fact]
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

        [Fact]
        public void UpdateUserMethod_GivenSomeParameter_ThenShouldSetThePropertieCorrectly()
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

        [Fact]
        public void ActiveUserMethod_GivenAnyUserActiveParameter_ThenShouldSetActiveEqualsTrue()
        {
            _user.ActivateUser();

            Assert.True(_user.Active);
        }

        [Fact]
        public void DisableUserMethod_GivenAnyUserActiveParameter_ThenShouldSetActiveEqualsFalse()
        {
            _user.DisableUser();

            Assert.False(_user.Active);
        }

        [Fact]
        public void UpdateHashPasswordMethod_GivenAnyStringParameter_ThenShouldSetTheNewHashPassword()
        {
            string expectedHash = _faker.Lorem.Text();
            _user.UpdateHashPassword(expectedHash);

            Assert.Equal(expectedHash, _user.HashPassword);
        }

        [Fact]
        public void EmailCreateMethod_GivenValidEmail_ShouldReturnSuccessResultWithCorrectValue()
        {
            string expectedEmailVal = _faker.Person.Email;

            var result = Email.Create(expectedEmailVal);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedEmailVal, result.Data!.Value);
        }

        [Fact]
        public void EmailCreateMethod_GivenInvalidEmail_ShouldReturnNotSuccessResultWithErrorMessage()
        {
            string expectedIncorrectEmail = _faker.Person.FullName;
            string expectedMessage = "Email inválido";

            var result = Email.Create(expectedIncorrectEmail);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(result.Message, expectedMessage);
        }
    }
}