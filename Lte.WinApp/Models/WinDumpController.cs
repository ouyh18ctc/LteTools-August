using Lte.Parameters.Abstract;
using Lte.Parameters.Concrete;
using Lte.Parameters.Kpi.Abstract;

namespace Lte.WinApp.Models
{
    public class WinDumpController : IParametersDumpController
    {
        public IBtsRepository BtsRepository { get; private set; }

        public ICdmaCellRepository CdmaCellRepository { get; private set; }

        public ICellRepository CellRepository { get; private set; }

        public IENodebRepository ENodebRepository { get; private set; }

        public ITownRepository TownRepository { get; private set; }

        public WinDumpController()
        {
            BtsRepository = new EFBtsRepository();
            CdmaCellRepository = new EFCdmaCellRepository();
            CellRepository = new EFCellRepository();
            ENodebRepository = new EFENodebRepository();
            TownRepository = new EFTownRepository();
        }
    }
}
