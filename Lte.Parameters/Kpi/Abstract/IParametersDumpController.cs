using Lte.Parameters.Abstract;

namespace Lte.Parameters.Kpi.Abstract
{
    public interface IParametersDumpController
    {
        IBtsRepository BtsRepository { get; }

        ICdmaCellRepository CdmaCellRepository { get; }

        ICellRepository CellRepository { get; }

        IENodebRepository ENodebRepository { get; }

        ITownRepository TownRepository { get; }
    }

    public interface IParametersDumpConfig
    {
        bool ImportBts { get; set; }

        bool UpdateBts { get; set; }

        bool ImportCdmaCell { get; set; }

        bool UpdateCdmaCell { get; set; }

        bool ImportENodeb { get; set; }

        bool UpdateENodeb { get; set; }

        bool ImportLteCell { get; set; }

        bool UpdateLteCell { get; set; }

        bool UpdatePci { get; set; }
    }

    public interface IBtsDumpRepository<TBts>
        where TBts : class, IValueImportable, new()
    {
        void InvokeAction(IExcelBtsImportRepository<TBts> importRepository);

        bool ImportBts { get; set; }

        bool UpdateBts { get; set; }
    }

    public interface ICellDumpRepository<TCell>
        where TCell : class, IValueImportable, new()
    {
        void InvokeAction(IExcelCellImportRepository<TCell> importRepository);

        bool ImportCell { get; set; }

        bool UpdateCell { get; set; }
    }
}
