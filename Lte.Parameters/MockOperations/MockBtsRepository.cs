using System.Collections.Generic;
using System.Linq;
using Moq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.MockOperations
{
    public static class MockBtsRepository
    {
        public static void MockBtsRepositorySaveBts(
            this Mock<IBtsRepository> repository, IEnumerable<CdmaBts> btss)
        {
            repository.Setup(x => x.Insert(It.IsAny<CdmaBts>())).Callback<CdmaBts>(
                e =>
                {
                    repository.Setup(x => x.GetAll()).Returns(
                        btss.Concat(new List<CdmaBts> {e}).AsQueryable());
                    repository.Setup(x => x.Count()).Returns(
                        repository.Object.GetAll().Count());
                });
        }

        public static void MockBtsRepositorySaveBts(
            this Mock<IBtsRepository> repository)
        {
            repository.Setup(x => x.Insert(It.IsAny<CdmaBts>())).Callback<CdmaBts>(
                e =>
                {
                    IEnumerable<CdmaBts> btss = repository.Object.GetAll();
                    repository.Setup(x => x.GetAll()).Returns(
                    btss.Concat(new List<CdmaBts> { e }).AsQueryable());
                    repository.Setup(x => x.Count()).Returns(
                        repository.Object.GetAll().Count());
                });
        }

        public static void MockBtsRepositoryDeleteBts(
            this Mock<IBtsRepository> repository, IEnumerable<CdmaBts> btss)
        {
            repository.Setup(x => x.Delete(It.Is<CdmaBts>(e => e != null
                && btss.FirstOrDefault(y => y == e) != null))
                ).Callback<CdmaBts>(
                e =>
                {
                    repository.Setup(x => x.GetAll()).Returns(
                        btss.Except(new List<CdmaBts> {e}).AsQueryable());
                    repository.Setup(x => x.Count()).Returns(
                        repository.Object.GetAll().Count());
                });
        }

        public static void MockBtsRepositoryDeleteBts(
            this Mock<IBtsRepository> repository)
        {
            if (repository.Object != null)
            {
                IEnumerable<CdmaBts> btss = repository.Object.GetAll();
                repository.Setup(x => x.Delete(It.Is<CdmaBts>(e => e != null
                    && btss.FirstOrDefault(y => y == e) != null))
                    ).Callback<CdmaBts>(
                    e =>
                    {
                        repository.Setup(x => x.GetAll()).Returns(
                            btss.Except(new List<CdmaBts> { e }).AsQueryable());
                        repository.Setup(x => x.Count()).Returns(
                            repository.Object.GetAll().Count());
                    });
            }
        }
    }

    public static class MockENodebRepository
    {
        public static void MockENodebRepositorySaveENodeb(
            this Mock<IENodebRepository> repository, IEnumerable<ENodeb> eNodebs)
        {
            repository.Setup(x => x.Insert(It.IsAny<ENodeb>())).Callback<ENodeb>(
                e => repository.SetupGet(x => x.GetAll()).Returns(
                    eNodebs.Concat(new List<ENodeb> { e }).AsQueryable()));
        }

        public static void MockENodebRepositorySaveENodeb(
            this Mock<IENodebRepository> repository)
        {
            repository.Setup(x => x.Insert(It.IsAny<ENodeb>())).Callback<ENodeb>(
                e =>
                {
                    IEnumerable<ENodeb> eNodebs = repository.Object.GetAll();
                    repository.Setup(x => x.GetAll()).Returns(
                    eNodebs.Concat(new List<ENodeb> { e }).AsQueryable());
                    repository.Setup(x => x.Count()).Returns(repository.Object.GetAll().Count());
                });
        }

        public static void MockENodebRepositoryDeleteENodeb(
            this Mock<IENodebRepository> repository, IEnumerable<ENodeb> eNodebs)
        {
            repository.Setup(x => x.Delete(It.Is<ENodeb>(e => e != null
                && eNodebs.FirstOrDefault(y => y == e) != null))
                ).Callback<ENodeb>(
                e =>
                {
                    repository.Setup(x => x.GetAll()).Returns(
                        eNodebs.Except(new List<ENodeb> { e }).AsQueryable());
                    repository.Setup(x => x.Count()).Returns(repository.Object.GetAll().Count());
                });
        }

        public static void MockENodebRepositoryDeleteENodeb(
            this Mock<IENodebRepository> repository)
        {
            if (repository.Object != null)
            {
                IEnumerable<ENodeb> eNodebs = repository.Object.GetAll();
                repository.Setup(x => x.Delete(It.Is<ENodeb>(e => e != null
                    && eNodebs.FirstOrDefault(y => y == e) != null))
                    ).Callback<ENodeb>(
                    e =>
                    {
                        repository.Setup(x => x.GetAll()).Returns(
                            eNodebs.Except(new List<ENodeb> {e}).AsQueryable());
                        repository.Setup(x => x.Count()).Returns(repository.Object.GetAll().Count());
                    });
            }
        }
    }
}
