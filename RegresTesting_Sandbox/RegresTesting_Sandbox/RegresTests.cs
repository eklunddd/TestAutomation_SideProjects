using NUnit.Framework;
using System.Threading.Tasks;

namespace RegresTesting_Sandbox
{
    public class RegresTests
    {

        [Test]
        public async Task SetUpNewUser()
        {
            var utility = new Utility();
            //Arrange
            var userToSetup = await utility.CreateUserDtoObject();
            //Act
            var setupUser = await utility.CreateNewUserReturnResponseDto(userToSetup);
            var getAlreadySetupUserById = await utility.GetUserById(setupUser.Id);
            Assert.Multiple(() =>
            {
                Assert.That(getAlreadySetupUserById?.Id, Is.EqualTo(setupUser.Id));
                Assert.That(getAlreadySetupUserById?.Email, Is.EqualTo(setupUser.Email));
            });
        }
    }
}