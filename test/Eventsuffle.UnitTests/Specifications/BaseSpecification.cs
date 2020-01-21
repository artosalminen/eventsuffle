using Eventsuffle.Core.Specifications;
using System;
using System.Linq;
using Xunit;

namespace Eventsuffle.UnitTests.Specifications
{
    public class BaseSpecification
    {
        class TestObject
        {
            public TestObject(Guid id, string name)
            {
                Id = id;
                Name = name;
            }

            public Guid Id { get; }
            public string Name { get; }
        }

        class TestSpecification: BaseSpecification<TestObject>
        {
            public TestSpecification(Guid id) : base(obj => obj.Id == id)
            {
            }

            public TestSpecification(string name) : base(obj => obj.Name == name)
            {
            }
        }

        [Fact]
        public void MatchesWithId()
        {
            // Arrange
            var testObjects = new[]
            {
                new TestObject(Guid.NewGuid(), "foo"),
                new TestObject(Guid.NewGuid(), "bar"),
                new TestObject(Guid.NewGuid(), "baz"),
            };
            var idToQueryFor = testObjects[0].Id;
            var specification = new TestSpecification(idToQueryFor);

            // Act
            TestObject result = testObjects.AsQueryable().FirstOrDefault(specification.Criteria);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Id, idToQueryFor);
        }

        [Fact]
        public void DoesNotMatchWithIdNotPresent()
        {
            // Arrange
            var testObjects = new[]
            {
                new TestObject(Guid.NewGuid(), "foo"),
                new TestObject(Guid.NewGuid(), "foo"),
                new TestObject(Guid.NewGuid(), "baz"),
            };
            var idToQueryFor = Guid.NewGuid();
            var specification = new TestSpecification(idToQueryFor);

            // Act
            TestObject result = testObjects.AsQueryable().FirstOrDefault(specification.Criteria);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void MatchesWithName()
        {
            // Arrange
            var testObjects = new[]
            {
                new TestObject(Guid.NewGuid(), "foo"),
                new TestObject(Guid.NewGuid(), "foo"),
                new TestObject(Guid.NewGuid(), "bar"),
            };
            var nameToQueryFor = testObjects[0].Name;
            var specification = new TestSpecification(nameToQueryFor);

            // Act
            TestObject[] result = testObjects.AsQueryable().Where(specification.Criteria).ToArray();

            // Assert
            Assert.NotEmpty(result);
            Assert.All(result, i => Assert.Equal(nameToQueryFor, i.Name));
            Assert.Equal(2, result.Count());
        }
    }
}
