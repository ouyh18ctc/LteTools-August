using System.Collections.Generic;
using Lte.Parameters.Entities;

namespace Lte.WebApp.Tests.ControllerParameters
{
    public class ParametersConfig
    {
        protected readonly IEnumerable<Town> towns = new List<Town>{
            new Town { CityName = "City1", DistrictName = "District1", TownName = "Town1", Id = 1 },
            new Town { CityName = "City1", DistrictName = "District1", TownName = "Town2", Id = 2 },
            new Town { CityName = "City1", DistrictName = "District1", TownName = "Town3", Id = 3 },
            new Town { CityName = "City1", DistrictName = "District2", TownName = "Town1", Id = 4 },
            new Town { CityName = "City1", DistrictName = "District2", TownName = "Town4", Id = 5 },
            new Town { CityName = "City2", DistrictName = "District1", TownName = "Town5", Id = 6 },
            new Town { CityName = "City2", DistrictName = "District3", TownName = "Town2", Id = 7 }
        };

        protected readonly IEnumerable<OptimizeRegion> regions = new List<OptimizeRegion>{
            new OptimizeRegion { City = "City1", District = "District1", Region = "Region1" },
            new OptimizeRegion { City = "City1", District = "District2", Region = "Region2" },
            new OptimizeRegion { City = "City2", District = "District1", Region = "Region3" },
            new OptimizeRegion { City = "City2", District = "District3", Region = "Region4" }
        };

        protected readonly IEnumerable<ENodeb> eNodebs = new List<ENodeb> {
            new ENodeb { TownId = 1, ENodebId = 1, Name = "ENodeb-1", Address = "Address-1" },
            new ENodeb { TownId = 1, ENodebId = 2, Name = "ENodeb-2", Address = "Address-2" },
            new ENodeb { TownId = 2, ENodebId = 3, Name = "ENodeb-3", Address = "Address-3" },
            new ENodeb { TownId = 3, ENodebId = 4, Name = "ENodeb-4", Address = "Address-4" },
            new ENodeb { TownId = 5, ENodebId = 5, Name = "ENodeb-5", Address = "Address-5" },
            new ENodeb { TownId = 5, ENodebId = 6, Name = "ENodeb-6", Address = "Address-6" },
            new ENodeb { TownId = 6, ENodebId = 7, Name = "ENodeb-7", Address = "Address-7" },
            new ENodeb { TownId = 7, ENodebId = 8, Name = "ENodeb-8", Address = "Address-8" },
            new ENodeb { TownId = 7, ENodebId = 9, Name = "ENodeb-9", Address = "Address-9" }
        };

        protected readonly IEnumerable<ENodeb> lotsOfENodebs = new List<ENodeb> {
            new ENodeb { TownId = 1, ENodebId = 1, Name = "ENodeb-1", Address = "Address-1" },
            new ENodeb { TownId = 1, ENodebId = 2, Name = "ENodeb-2", Address = "Address-2" },
            new ENodeb { TownId = 2, ENodebId = 3, Name = "ENodeb-3", Address = "Address-3" },
            new ENodeb { TownId = 3, ENodebId = 4, Name = "ENodeb-4", Address = "Address-4" },
            new ENodeb { TownId = 5, ENodebId = 5, Name = "ENodeb-5", Address = "Address-5" },
            new ENodeb { TownId = 5, ENodebId = 6, Name = "ENodeb-6", Address = "Address-6" },
            new ENodeb { TownId = 6, ENodebId = 7, Name = "ENodeb-7", Address = "Address-7" },
            new ENodeb { TownId = 7, ENodebId = 8, Name = "ENodeb-8", Address = "Address-8" },
            new ENodeb { TownId = 7, ENodebId = 9, Name = "ENodeb-9", Address = "Address-9" },
            new ENodeb { TownId = 1, ENodebId = 10, Name = "ENodeb-10", Address = "Address-10" },
            new ENodeb { TownId = 1, ENodebId = 11, Name = "ENodeb-11", Address = "Address-11" },
            new ENodeb { TownId = 2, ENodebId = 12, Name = "ENodeb-12", Address = "Address-12" },
            new ENodeb { TownId = 5, ENodebId = 13, Name = "ENodeb-13", Address = "Address-13" },
            new ENodeb { TownId = 5, ENodebId = 14, Name = "ENodeb-14", Address = "Address-14" },
            new ENodeb { TownId = 5, ENodebId = 15, Name = "ENodeb-15", Address = "Address-15" },
            new ENodeb { TownId = 5, ENodebId = 16, Name = "ENodeb-16", Address = "Address-16" },
            new ENodeb { TownId = 7, ENodebId = 17, Name = "ENodeb-17", Address = "Address-17" }
        };
    }
}
