using System.Collections.Generic;
using System.Linq;
using Moq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.MockOperations
{
    public static class MockCellRepository
    {
        public static void MockCellRepositorySaveCell(
            this Mock<ICellRepository> repository, IEnumerable<Cell> cells)
        {
            repository.Setup(x => x.Insert(It.IsAny<Cell>())).Callback<Cell>(
                e =>
                {
                    repository.Setup(x => x.GetAll()).Returns(
                        cells.Concat(new List<Cell> {e}).AsQueryable());
                    repository.Setup(x => x.Count()).Returns(
                        repository.Object.GetAll().Count());
                    repository.Setup(x => x.GetAllList()).Returns(repository.Object.GetAll().ToList());
                });
            repository.Setup(x => x.AddCells(It.IsAny<IEnumerable<Cell>>())).Callback<IEnumerable<Cell>>(
                l =>
                {
                    repository.Setup(x => x.GetAll()).Returns(
                        cells.Concat(l).AsQueryable());
                    repository.Setup(x => x.Count()).Returns(
                        repository.Object.GetAll().Count());
                    repository.Setup(x => x.GetAllList()).Returns(repository.Object.GetAll().ToList());
                });
        }

        public static void MockCellRepositorySaveCell(
            this Mock<ICellRepository> repository)
        {
            repository.Setup(x => x.Insert(It.IsAny<Cell>())).Callback<Cell>(
                e =>
                {
                    IEnumerable<Cell> cells = repository.Object.GetAll();
                    repository.Setup(x => x.GetAll()).Returns(
                        cells.Concat(new List<Cell> { e }).AsQueryable());
                    repository.Setup(x => x.Count()).Returns(
                        repository.Object.GetAll().Count());
                    repository.Setup(x => x.GetAllList()).Returns(repository.Object.GetAll().ToList());
                });
            repository.Setup(x => x.AddCells(It.IsAny<IEnumerable<Cell>>())).Callback<IEnumerable<Cell>>(
                l =>
                {
                    IEnumerable<Cell> cells = repository.Object.GetAll();
                    repository.Setup(x => x.GetAll()).Returns(
                        cells.Concat(l).AsQueryable());
                    repository.Setup(x => x.Count()).Returns(
                        repository.Object.GetAll().Count());
                    repository.Setup(x => x.GetAllList()).Returns(repository.Object.GetAll().ToList());
                });
        }

        public static void MockCellRepositoryDeleteCell(
            this Mock<ICellRepository> repository, IEnumerable<Cell> cells)
        {
            repository.Setup(x => x.Delete(It.Is<Cell>(e => e != null
                && cells.FirstOrDefault(y => y == e) != null))
                ).Callback<Cell>(
                e =>
                {
                    repository.Setup(x => x.GetAll()).Returns(
                        cells.Except(new List<Cell> { e }).AsQueryable());
                    repository.Setup(x => x.Count()).Returns(
                        repository.Object.GetAll().Count());
                    repository.Setup(x => x.GetAllList()).Returns(repository.Object.GetAll().ToList());
                });
        }

        public static void MockCellRepositoryDeleteCell(
            this Mock<ICellRepository> repository)
        {
            if (repository.Object != null)
            {
                IEnumerable<Cell> cells = repository.Object.GetAll();
                repository.Setup(x => x.Delete(It.Is<Cell>(e => e != null
                    && cells.FirstOrDefault(y => y == e) != null))
                    ).Callback<Cell>(
                    e =>
                    {
                        repository.Setup(x => x.GetAll()).Returns(
                            cells.Except(new List<Cell> { e }).AsQueryable());
                        repository.Setup(x => x.Count()).Returns(
                            repository.Object.GetAll().Count());
                        repository.Setup(x => x.GetAllList()).Returns(repository.Object.GetAll().ToList());
                    });
            }
        }
    }

    public static class MockCdmaCellRepository
    {
        public static void MockCdmaCellRepositorySaveCell(
            this Mock<ICdmaCellRepository> repository, IEnumerable<CdmaCell> cells)
        {
            repository.Setup(x => x.Insert(It.IsAny<CdmaCell>())).Callback<CdmaCell>(
                e =>
                {
                    repository.Setup(x => x.GetAll()).Returns(
                        cells.Concat(new List<CdmaCell> {e}).AsQueryable());
                    repository.Setup(x => x.GetAllList()).Returns(repository.Object.GetAll().ToList());
                    repository.Setup(x => x.Count()).Returns(repository.Object.GetAll().Count());
                });
        }

        public static void MockCdmaCellRepositorySaveCell(
            this Mock<ICdmaCellRepository> repository)
        {
            repository.Setup(x => x.Insert(It.IsAny<CdmaCell>())).Callback<CdmaCell>(
                e =>
                {
                    IEnumerable<CdmaCell> cells = repository.Object.GetAll();
                    repository.Setup(x => x.GetAll()).Returns(
                        cells.Concat(new List<CdmaCell> { e }).AsQueryable());
                    repository.Setup(x => x.GetAllList()).Returns(repository.Object.GetAll().ToList());
                    repository.Setup(x => x.Count()).Returns(repository.Object.GetAll().Count());
                });
        }

        public static void MockCdmaCellRepositoryDeleteCell(
            this Mock<ICdmaCellRepository> repository, IEnumerable<CdmaCell> cells)
        {
            repository.Setup(x => x.Delete(It.Is<CdmaCell>(e => e != null
                && cells.FirstOrDefault(y => y == e) != null))
                ).Callback<CdmaCell>(
                e =>
                {
                    repository.Setup(x => x.GetAll()).Returns(
                        cells.Except(new List<CdmaCell> {e}).AsQueryable());
                    repository.Setup(x => x.GetAllList()).Returns(repository.Object.GetAll().ToList());
                    repository.Setup(x => x.Count()).Returns(repository.Object.GetAll().Count());
                });
        }

        public static void MockCdmaCellRepositoryDeleteCell(
            this Mock<ICdmaCellRepository> repository)
        {
            if (repository.Object != null)
            {
                IEnumerable<CdmaCell> cells = repository.Object.GetAll();
                repository.Setup(x => x.Delete(It.Is<CdmaCell>(e => e != null
                    && cells.FirstOrDefault(y => y == e) != null))
                    ).Callback<CdmaCell>(
                    e =>
                    {
                        repository.Setup(x => x.GetAll()).Returns(
                            cells.Except(new List<CdmaCell> {e}).AsQueryable());
                        repository.Setup(x => x.GetAllList()).Returns(repository.Object.GetAll().ToList());
                        repository.Setup(x => x.Count()).Returns(repository.Object.GetAll().Count());
                    });
            }
        }
    }
}
