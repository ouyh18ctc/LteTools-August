using System.Collections.Generic;
using System.Linq;
using Moq;

namespace Lte.Parameters.Kpi.Abstract
{
    public interface ITopCellRepository<TStat>
    {
        IQueryable<TStat> Stats { get; }

        void AddOneStat(TStat stat);

        void SaveChanges();
    }

    public static class MockStatRepositoryOperations
    {
        public static void MockOperations<T>(this Mock<ITopCellRepository<T>> repository)
        {
            repository.SetupGet(x => x.Stats).Returns(
                new List<T>().AsQueryable());
            repository.Setup(x => x.AddOneStat(It.IsAny<T>())).Callback<T>(x =>
            {
                IEnumerable<T> originalStats = repository.Object.Stats;
                repository.SetupGet(r => r.Stats).Returns(originalStats.Concat(
                    new List<T> { x }).AsQueryable());
            });
        }
    }
}
