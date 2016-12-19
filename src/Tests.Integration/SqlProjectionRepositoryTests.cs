using System;
using Example;
using Library;
using NUnit.Framework;
using System.Data.SqlClient;

namespace Tests.Integration
{
    [TestFixture]
    public class SqlProjectionRepositoryTests
    {
        private const string ConnectionString = "Data Source=<FixIt!>; Initial Catalog=Projections; Integrated Security=True";

        [Test]
        public void CreateTable_WhenTableDoesNotExist_TableCreatedCorrectly()
        {
            // Arrange
            var tableName = Guid.NewGuid().ToString();
            var projectionRepository = new SqlServerViewRepository(ConnectionString, tableName);

            // Act
            projectionRepository.CreateProjectionsTable();

            // Assert
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                var sql = $@"SELECT name FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{tableName}]') AND type in (N'U')";
                using (var command = new SqlCommand(sql, sqlConnection))
                {
                    string name = command.ExecuteScalar() as string;
                    Assert.That(name, Is.EqualTo(tableName));
                }
            }
        }

        [Test]
        public void Commit_WhenItemDoesNotExistInDatabase_ItemCreated()
        {
            // Arrange
            var tableName = Guid.NewGuid().ToString();
            const string id = "SomeId";
            const string name = "SomeName";
            const string category = "SomeCategory";

            var companyView = new CompanyView
            {
                Id = id,
                Category = category,
                Name = name
            };

            var projectionRepository = new SqlServerViewRepository(ConnectionString, tableName);
            projectionRepository.CreateProjectionsTable();

            // Act
            projectionRepository.Commit(id, companyView);

            // Assert
            var companyViewFromDb = projectionRepository.Read<CompanyView>(id);

            Assert.That(companyViewFromDb.Id, Is.EqualTo(id));
            Assert.That(companyViewFromDb.Name, Is.EqualTo(name));
            Assert.That(companyViewFromDb.Category, Is.EqualTo(category));
        }

        [Test]
        public void Commit_WhenItemExistsInDatabase_ItemUpdated()
        {
            // Arrange
            var tableName = Guid.NewGuid().ToString();
            const string id = "SomeId";
            const string theNewName = "TheNewName";
            const string theNewCategory = "TheNewCategory";

            var companyView = new CompanyView
            {
                Id = id,
                Name = "AName",
                Category = "ACategory"
            };

            var projectionRepository = new SqlServerViewRepository(ConnectionString, tableName);
            projectionRepository.CreateProjectionsTable();

            projectionRepository.Commit(id, companyView);

            // Act
            var updatedCompanyView = new CompanyView
            {
                Id = id,
                Name = theNewName,
                Category = theNewCategory
            };

            projectionRepository.Commit(id, updatedCompanyView);

            // Assert
            var companyViewFromDb = projectionRepository.Read<CompanyView>(id);

            Assert.That(companyViewFromDb.Id, Is.EqualTo(id));
            Assert.That(companyViewFromDb.Name, Is.EqualTo(theNewName));
            Assert.That(companyViewFromDb.Category, Is.EqualTo(theNewCategory));
        }
    }
}
