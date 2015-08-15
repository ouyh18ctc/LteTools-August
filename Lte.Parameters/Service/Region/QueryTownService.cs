using System;
using System.Collections.Generic;
using System.Linq;
using Lte.Domain.Geo.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Service.Region
{
    public static class QueryTownService
    {
        public static Town Query(this IEnumerable<Town> towns, string district, string town)
        {
            return (towns == null) ? null : towns.FirstOrDefault(x =>
                x.DistrictName.Trim() == district.Trim()
                && x.TownName.Trim() == town.Trim());
        }

        public static Town Query(this IEnumerable<Town> towns, string city, string district, string town)
        {
            return (towns == null) ? null : towns.FirstOrDefault(x =>
                x.CityName.Trim() == city.Trim()
                && x.DistrictName.Trim() == district.Trim()
                && x.TownName.Trim() == town.Trim());
        }

        public static Town Query(this IEnumerable<Town> towns, ITown town)
        {
            return towns.Query(town.CityName, town.DistrictName, town.TownName);
        }

        public static IEnumerable<Town> QueryTowns(this IEnumerable<Town> towns, string district, string town)
        {
            district = district ?? "不限定";
            town = town ?? "不限定";
            return towns.ToList().Where(x =>
                (x.TownName == town || town.IndexOf("不限定", StringComparison.Ordinal) >= 0)
                && (x.DistrictName == district || district.IndexOf("不限定", StringComparison.Ordinal) >= 0));
        }

        public static IEnumerable<Town> QueryTowns(this IEnumerable<Town> towns, string city, string district, string town)
        {
            city = city ?? "不限定";
            district = district ?? "不限定";
            town = town ?? "不限定";
            return towns.ToList().Where(x =>
                (x.TownName == town || town.IndexOf("不限定", StringComparison.Ordinal) >= 0)
                && (x.DistrictName == district || district.IndexOf("不限定", StringComparison.Ordinal) >= 0)
                && (x.CityName == city || city.IndexOf("不限定", StringComparison.Ordinal) >= 0));
        }

        public static IEnumerable<Town> QueryTowns(this IEnumerable<Town> towns, ITown town)
        {
            return towns.QueryTowns(town.CityName, town.DistrictName, town.TownName);
        }

        public static int QueryId(this IEnumerable<Town> towns, string district, string townName)
        {
            Town town = towns.Query(district, townName);
            return (town == null) ? -1 : town.Id;
        }

        public static int QueryId(this IEnumerable<Town> towns, string city, string district, string townName)
        {
            Town town = towns.Query(city, district, townName);
            return (town == null) ? -1 : town.Id;
        }

        public static int QueryId(this IEnumerable<Town> towns, ITown town)
        {
            return towns.QueryId(town.CityName, town.DistrictName, town.TownName);
        }
    }
}
