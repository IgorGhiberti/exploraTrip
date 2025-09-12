using Application.Interfaces;
using Bogus;
using Domain.Entities;
using Moq;
using Xunit;
using Application.Users;
using System.Threading.Tasks;
using Application.Users.DTOs;
using Domain.ValueObjects;
using Domain.Intfaces;

namespace Tests.Services
{
    [Trait("Service", "UserService")]
    public sealed class UserServicesTests
    {
        private readonly Faker _faker = new("pt_BR");
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPasswordCryptography> _passwordCryptographyMock;
        private readonly Mock<ICache> _cacheMock;
        private readonly UserServices _userServices;

        public UserServicesTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordCryptographyMock = new Mock<IPasswordCryptography>();
            _cacheMock = new Mock<ICache>();
            _userServices = new UserServices(_userRepositoryMock.Object, _passwordCryptographyMock.Object, _cacheMock.Object);
        }

        [Trait("Method", "Authenticate")]
        [Fact]
        public async Task GivenAListOfUsers_ShouldReturnAListOfViewUsersDto()
        {
            var usersList = new List<User>
            {
                new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool()),
                new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool()),
                new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool()),
                new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool()),
                new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool()),
                new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool()),
                new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool()),
                new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool()),
                new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool())
            };
            _userRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(usersList);

            var result = await _userServices.GetAll();

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.All(result.Data, dto =>
            {
                var expectedUser = usersList.FirstOrDefault(u => u.Id == dto.Id);
                Assert.Equal(expectedUser!.Id, dto.Id);
                Assert.Equal(expectedUser!.UserName, dto.UserName);
                Assert.Equal(expectedUser.Email!.Value, dto.Email);
                Assert.Equal(expectedUser.Active, dto.IsActive);
            });
        }

        [Trait("Method", "Authenticate")]
        [Fact]
        public async Task GivenTheCorrectCredentials_ThenShouldAuthenticateTheUser()
        {
            string expectedAnswer = "User successfully authenticated!";
            string email = _faker.Person.Email;
            var user = new User(email, _faker.Person.FullName, _faker.Lorem.Text(), email, _faker.Random.Bool());
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(user.Email!.Value)).ReturnsAsync(user);

            LoginUserDTO userDto = new LoginUserDTO(user.Email!.Value, user.HashPassword);
            string userStoredHash = user.HashPassword;
            _passwordCryptographyMock.Setup(p => p.ValidateHash(userDto.Password, userStoredHash, userDto.Email)).Returns(true);

            var result = await _userServices.AuthenticateUser(userDto);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedAnswer, result.Data);
        }

        [Trait("Method", "Authenticate")]
        [Fact]
        public async Task GivenTheWrongCredentials_ThenShouldReturnAnErrorMessage()
        {
            string expectedAnswer = "Authentication failed.";
            string email = _faker.Person.Email;
            var user = new User(email, _faker.Person.FullName, _faker.Lorem.Text(), email, _faker.Random.Bool());
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(user.Email!.Value)).ReturnsAsync(user);

            LoginUserDTO userDto = new LoginUserDTO(user.Email!.Value, user.HashPassword);
            string userStoredHash = "12345";
            _passwordCryptographyMock.Setup(p => p.ValidateHash(userDto.Password, userStoredHash, userDto.Email)).Returns(false);

            var result = await _userServices.AuthenticateUser(userDto);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(expectedAnswer, result.Message);
        }


        [Trait("Method", "GetUserByEmail")]
        [Fact]
        public async Task GivenAnEmailThatDoesNotExist_ThenShouldReturnAnErrorMessage()
        {
            string expectedResult = "Email not registered in the system.";
            string emailUser = _faker.Person.Email;
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(emailUser)).ReturnsAsync((User?)null);

            LoginUserDTO userDto = new LoginUserDTO(_faker.Lorem.Text(), emailUser);

            var result = await _userServices.AuthenticateUser(userDto);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(expectedResult, result.Message);
        }

        [Trait("Method", "GetUserById")]
        [Fact]
        public async Task GivenAnIdThatExistsInTheDatabase_ThenShouldReturnAUser()
        {
            var user = new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool());
            _userRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).ReturnsAsync(user);

            var result = await _userServices.GetUserById(user.Id);

            ViewUserDTO expectedViewDTO = new ViewUserDTO(result.Data!.Id, result.Data.UserName, result.Data.Email, result.Data.IsActive);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedViewDTO, result.Data);
        }

        [Trait("Method", "GetUserById")]
        [Fact]
        public async Task GivenAnIdThatDoesNotExist_ThenShouldReturnAnExpectedErrorMessage()
        {
            string expectedMessage = "User does not exist.";
            var user = new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool());
            _userRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).ReturnsAsync((User?)null);

            var result = await _userServices.GetUserById(user.Id);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(expectedMessage, result.Message);
        }

        [Trait("Method", "ActiveUser")]
        [Fact]
        public async Task GivenAnIdThatExists_ThenShouldReturnAnUserWithActiveEqualsTrue()
        {
            var user = new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool());
            _userRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).ReturnsAsync(user);

            var result = await _userServices.ActiveUser(user.Id);

            ViewUserDTO expectedViewDTO = new ViewUserDTO(user.Id, user.UserName, user.Email!.Value, true);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedViewDTO, result.Data);
        }

        [Trait("Method", "ActiveUser")]
        [Fact]
        public async Task GivenAnIdThatDoesNotExist_ThenShouldReturnAnErrorMessage()
        {
            string expectedMessage = "User does not exist.";
            var user = new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool());
            _userRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).ReturnsAsync((User?)null);

            var result = await _userServices.ActiveUser(user.Id);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(expectedMessage, result.Message);
        }

        [Trait("Method", "DisableUser")]
        [Fact]
        public async Task GivenAnIdThatExists_ThenShouldReturnUserWithActiveEqualsFalse()
        {
            var user = new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool());
            _userRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).ReturnsAsync(user);

            var result = await _userServices.DisableUser(user.Id);

            ViewUserDTO expectedViewDTO = new ViewUserDTO(user.Id, user.UserName, user.Email!.Value, false);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedViewDTO, result.Data);
        }

        [Trait("Method", "DisableUser")]
        [Fact]
        public async Task GivenAnIdThatDoesNotExistInDisable_ThenShouldReturnAnErrorMessage()
        {
            string expectedMessage = "User does not exist.";
            var user = new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool());
            _userRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).ReturnsAsync((User?)null);

            var result = await _userServices.DisableUser(user.Id);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(expectedMessage, result.Message);
        }

        [Trait("Method", "IsUserActive")]
        [Fact]
        public async Task GivenAnIdThatExists_ThenShouldReturnUserActiveStatus()
        {
            var user = new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool());
            _userRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).ReturnsAsync(user);
            bool expectedResult = user.Active;

            var result = await _userServices.IsUserActive(user.Id);

            Assert.True(result.IsSuccess);
            Assert.Equal(expectedResult, result.Data);
        }

        [Trait("Method", "IsUserActive")]
        [Fact]
        public async Task GivenAnIdThatDoesNotExistIsUserActive_ThenShouldReturnAnErrorMessage()
        {
            string expectedMessage = "User does not exist.";
            var user = new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, true);
            _userRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).ReturnsAsync((User?)null);

            var result = await _userServices.IsUserActive(user.Id);

            Assert.False(result.IsSuccess);
            Assert.Equal(expectedMessage, result.Message);
        }

        [Trait("Method", "GetUserByEmail")]
        [Fact]
        public async Task GivenAnEmailThatExists_ThenShouldReturnUser()
        {
            var expectedUser = new User(_faker.Person.Email, _faker.Person.FullName, _faker.Lorem.Text(), _faker.Person.Email, _faker.Random.Bool());
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(expectedUser.Email!.Value)).ReturnsAsync(expectedUser);

            var result = await _userServices.GetUserByEmail(expectedUser.Email!.Value);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(expectedUser.Id, result.Data.Id);
            Assert.Equal(expectedUser.UserName, result.Data.UserName);
            Assert.Equal(expectedUser.Email, result.Data.Email);
            Assert.Equal(expectedUser.Active, result.Data.Active);
        }

        [Trait("Method", "GetUserByEmail")]
        [Fact]
        public async Task GivenAnEmailThatDoesNotExist_ThenShouldReturnError()
        {
            string email = _faker.Person.Email;
            string expectedMessage = "Email not registered in the system.";
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email)).ReturnsAsync((User?)null);

            var result = await _userServices.GetUserByEmail(email);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(expectedMessage, result.Message);
        }
    }
}