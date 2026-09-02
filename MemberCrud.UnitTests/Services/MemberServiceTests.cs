using Microsoft.VisualStudio.TestTools.UnitTesting;
using MemberCrud.Services;
using MemberCrud.Data;
using MemberCrud.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace MemberCrud.UnitTests.Services
{
    [TestClass]
    public class MemberServiceTests
    {
        [TestMethod]
        public void AddMember_AddsAndGetAllMembersReturnsIt()
        {
            string dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<MemberCrudDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            var service = new MemberService(() => new MemberCrudDbContext(options));

            var member = new Member
            {
                FirstName = "John",
                LastName = "Doe",
                Phone = "555-0100",
                Email = "john@example.com",
                MembershipStatus = "Active",
                Street = "1 Test St",
                City = "Testville",
                PostalCode = "12345",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-30)),
                CreateAt = DateTime.UtcNow
            };

            service.AddMember(member);

            var all = service.GetAllMembers();
            Assert.AreEqual(1, all.Count);
            Assert.AreEqual("John", all.First().FirstName);
        }

        [TestMethod]
        public void UpdateMember_ChangesPersist()
        {
            string dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<MemberCrudDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            var service = new MemberService(() => new MemberCrudDbContext(options));

            var member = new Member
            {
                FirstName = "Jane",
                LastName = "Original",
                Phone = "555-0200",
                Email = "jane@example.com",
                MembershipStatus = "Active",
                Street = "2 Test Ave",
                City = "Testville",
                PostalCode = "54321",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-25)),
                CreateAt = DateTime.UtcNow
            };

            service.AddMember(member);

            // Update the member's last name
            member.LastName = "Updated";
            service.UpdateMember(member);

            var all = service.GetAllMembers();
            Assert.AreEqual(1, all.Count);
            Assert.AreEqual("Updated", all.First().LastName);
        }

        [TestMethod]
        public void DeleteMember_RemovesRecord()
        {
            string dbName = Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<MemberCrudDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            var service = new MemberService(() => new MemberCrudDbContext(options));

            var member = new Member
            {
                FirstName = "Delete",
                LastName = "Me",
                Phone = "555-0300",
                Email = "delete@example.com",
                MembershipStatus = "Active",
                Street = "3 Test Rd",
                City = "Testville",
                PostalCode = "00000",
                DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-40)),
                CreateAt = DateTime.UtcNow
            };

            service.AddMember(member);
            service.DeleteMember(member);

            var all = service.GetAllMembers();
            Assert.AreEqual(0, all.Count);
        }
        [TestMethod]
        public void TestRunner_Works()
        {
            // Sanity check so test runner reports results. This test does not exercise
            // MemberService directly because the methods instantiate MemberCrudDbContext
            // internally and cannot be substituted with a test double.
            Assert.IsTrue(DateTime.UtcNow != default);
        }

        [TestMethod]
        public void AddMember_Null_ThrowsArgumentNullException()
        {
            var service = new MemberService(() => new MemberCrudDbContext(new DbContextOptionsBuilder<MemberCrudDbContext>().UseInMemoryDatabase("test").Options));
            try
            {
                service.AddMember(null!);
                Assert.Fail("Expected ArgumentNullException");
            }
            catch (ArgumentNullException)
            {
                // Expected
            }
        }

        [TestMethod]
        public void UpdateMember_Null_ThrowsArgumentNullException()
        {
            var service = new MemberService(() => new MemberCrudDbContext(new DbContextOptionsBuilder<MemberCrudDbContext>().UseInMemoryDatabase("test").Options));
            try
            {
                service.UpdateMember(null!);
                Assert.Fail("Expected ArgumentNullException");
            }
            catch (ArgumentNullException)
            {
                // Expected
            }
        }

        [TestMethod]
        public void DeleteMember_Null_ThrowsArgumentNullException()
        {
            var service = new MemberService(() => new MemberCrudDbContext(new DbContextOptionsBuilder<MemberCrudDbContext>().UseInMemoryDatabase("test").Options));
            try
            {
                service.DeleteMember(null!);
                Assert.Fail("Expected ArgumentNullException");
            }
            catch (ArgumentNullException)
            {
                // Expected
            }
        }
    }
}
