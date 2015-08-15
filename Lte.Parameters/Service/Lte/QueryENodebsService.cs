using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;
using Lte.Parameters.Service.Region;

namespace Lte.Parameters.Service.Lte
{
    public static class QueryENodebService
    {
        public static IEnumerable<ENodeb> QueryWithNames(this IRepository<ENodeb> repository,
            ITownRepository townRepository, string city, string district, string town, string eNodebName,
            string address)
        {
            IEnumerable<Town> _townList = townRepository.GetAll().QueryTowns(city, district, town).ToList();
            return (!_townList.Any())
                ? null
                : repository.GetAllList().Where(x =>
                    _townList.FirstOrDefault(t => t.Id == x.TownId) != null
                    && (string.IsNullOrEmpty(eNodebName) || x.Name.IndexOf(eNodebName.Trim(),
                        StringComparison.Ordinal) >= 0)
                    && (string.IsNullOrEmpty(address) || x.Address.IndexOf(address.Trim(),
                        StringComparison.Ordinal) >= 0));
        }
    }
}
