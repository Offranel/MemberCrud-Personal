using Microsoft.VisualStudio.TestTools.UnitTesting;
using MemberCrud.Models;

namespace MemberCrud.UnitTests
{
    [TestClass]
    public class MemberTests
    {
        [TestMethod]
        public void ToString_AllPartsPresent_ReturnsExpected()
        {
            // Arrange
            var member = new Member
            {
                FirstName = "John",
                LastName = "Doe",
                MembershipStatus = "Active",
                Phone = "000",
                Email = "a@b.com",
                Street = "x",
                City = "y",
                PostalCode = "z",
                DateOfBirth = new DateOnly(1990,1,1),
                CreateAt = System.DateTime.UtcNow
            };

            // Act
            var result = member.ToString();

            // Assert
            Assert.AreEqual("John Doe - Active", result);
        }
    }
}
