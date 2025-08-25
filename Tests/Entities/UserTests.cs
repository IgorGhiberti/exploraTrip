using Xunit;
using Domain.User;
using Bogus;

namespace Tests.Entities
{
    public sealed class UserTests
    {
        private readonly Faker _faker = new("pt_BR");

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

            var user = new User("igorgh@gmail.com", "Igor", "12345", "igorgh@gmail.com");
            user.UpdateUser(expectedEmail, expectedUserName);

            if (!string.IsNullOrWhiteSpace(expectedEmail))
                Assert.Equal(expectedEmail, user.Email!.Value);
            Assert.Equal(user.Email!.Value, user.Email!.Value);

            if (!string.IsNullOrWhiteSpace(expectedUserName))
                Assert.Equal(expectedUserName, user.UserName);
            Assert.Equal(user.UserName, user.UserName);
        }

        [Fact]
        public void ActiveUserMethod_GivenAnyUserActiveParameter_ThenShouldSetActiveEqualsTrue()
        {
            var user = new User("igorgh@gmail.com", "Igor", "12345", "igorgh@gmail.com", _faker.Random.Bool());

            user.ActivateUser();

            Assert.True(user.Active);
        }

        [Fact]
        public void DisableUserMethod_GivenAnyUserActiveParameter_ThenShouldSetActiveEqualsFalse()
        {
            var user = new User("igorgh@gmail.com", "Igor", "12345", "igorgh@gmail.com", _faker.Random.Bool());

            user.DisableUser();

            Assert.False(user.Active);
        }
    }
}