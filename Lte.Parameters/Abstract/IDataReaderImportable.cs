using System.Collections.Generic;
using System.Data;

namespace Lte.Parameters.Abstract
{
    public interface IDataReaderImportable
    {
        void Import(IDataReader tableReader);
    }

    public interface IValueImportable
    {
        void Import();
    }

    public interface IExcelImportable<in TExcel>
    {
        void Import(TExcel element, bool updateInfo);
    }

    public interface IMmlImportRepository<TBts, TCell, TBtsExcel, TCellExcel>
        where TBts : class, IExcelImportable<TBtsExcel>, new()
        where TCell : class, IExcelImportable<TCellExcel>, new()
    {
        List<TBts> CdmaBtsList { get; }

        List<TCell> CdmaCellList { get; }
    }

    public interface IMmlDumpRepository<TBts, TCell, TBtsExcel, TCellExcel>
        where TBts : class, IExcelImportable<TBtsExcel>, new()
        where TCell : class, IExcelImportable<TCellExcel>, new()
    {
        void InvokeAction(IMmlImportRepository<TBts, TCell, TBtsExcel, TCellExcel> mmlRepository);
    }
}
