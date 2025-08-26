using Application.Interfaces;
using Bogus;
using Domain.User;
using Moq;
using Xunit;
using Application.Users;
using System.Threading.Tasks;
using Application.Users.DTOs;
using Domain.ValueObjects;

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
    }
}