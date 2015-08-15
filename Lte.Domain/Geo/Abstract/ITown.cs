using Moq;

namespace Lte.Domain.Geo.Abstract
{
    public interface ITown
    {
        string CityName { get; set; }

        string DistrictName { get; set; }

        string TownName { get; set; }
    }

    public static class ITownQueries
    {
        public static bool IsAddConditionsValid(this ITown addConditions)
        {
            return !(string.IsNullOrEmpty(addConditions.CityName) || string.IsNullOrEmpty(addConditions.DistrictName)
                || string.IsNullOrEmpty(addConditions.TownName));
        }

        public static string GetAddConditionsInfo(this ITown addConditions)
        {
            return addConditions.CityName + "-" + addConditions.DistrictName + "-" + addConditions.TownName;
        }
    }
}
