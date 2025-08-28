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
    public class UserServicesTests
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
                var originalUser = usersList.FirstOrDefault(u => u.Id == dto.Id);
                Assert.Equal(originalUser!.Id, dto.Id);
                Assert.Equal(originalUser!.UserName, dto.UserName);
                Assert.Equal(originalUser.Email!.Value, dto.Email);
                Assert.Equal(originalUser.Active, dto.IsActive);
            });
        }

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
            Assert.Equal(result.Data, expectedAnswer);
        }

        [Fact]
        public async Task GivenTheWrongCredentials_ThenShouldReturnAErrorMessage()
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
            Assert.Equal(result.Message, expectedAnswer);
        }

        [Fact]
        public async Task GivenAnEmailThatDoesNotExist_ThenShouldReturnAErrorMessage()
        {
            string expectedResult = "Email not registered in the system.";
            string emailUser = _faker.Person.Email;
            _userRepositoryMock.Setup(repo => repo.GetUserByEmail(emailUser)).ReturnsAsync((User?)null);

            LoginUserDTO userDto = new LoginUserDTO(_faker.Lorem.Text(), emailUser);

            var result = await _userServices.AuthenticateUser(userDto);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal(result.Message, expectedResult);
        }
    }
}