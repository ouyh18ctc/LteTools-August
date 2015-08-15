using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lte.Parameters.Entities;
using Lte.Parameters.Abstract;

namespace Lte.Parameters.Test.Process
{
    public class StubCellProcessRepository : ICellRepository
    {
        public int CurrentProgress { get; set; }

        public StubCellProcessRepository()
        {
            CurrentProgress = 0;
        }

        public void AddOneCell(Cell cell)
        {
            CurrentProgress += 5;
        }

        public void AddCells(IEnumerable<Cell> cells)
        {
            CurrentProgress += 10;
        }

        public bool SaveCell(CellExcel cellInfo, IENodebRepository eNodebRepository)
        {
            return true;
        }

        public int SaveCells(List<CellExcel> cellInfoList, IENodebRepository eNodebRepository)
        {
            if (CurrentProgress > 10) { CurrentProgress = 0; }
            return CurrentProgress++;
        }

        public bool RemoveOneCell(Cell cell)
        {
            return true;
        }

        public bool DeleteCell(int eNodebId, byte sectorId)
        {
            return true;
        }

        public IQueryable<Cell> GetAll()
        {
            return new List<Cell>(){
                    new Cell(){
                        ENodebId = 1,
                        SectorId = 2
                    }
                }.AsQueryable();
        }

        public List<Cell> GetAllList()
        {
            throw new NotImplementedException();
        }

        public Task<List<Cell>> GetAllListAsync()
        {
            throw new NotImplementedException();
        }

        public List<Cell> GetAllList(Expression<Func<Cell, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<List<Cell>> GetAllListAsync(Expression<Func<Cell, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public T Query<T>(Func<IQueryable<Cell>, T> queryMethod)
        {
            throw new NotImplementedException();
        }

        public Cell Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Cell> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Cell Single(Expression<Func<Cell, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Cell> SingleAsync(Expression<Func<Cell, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Cell FirstOrDefault(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Cell> FirstOrDefaultAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Cell FirstOrDefault(Expression<Func<Cell, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Cell> FirstOrDefaultAsync(Expression<Func<Cell, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Cell Load(int id)
        {
            throw new NotImplementedException();
        }

        public Cell Insert(Cell entity)
        {
            throw new NotImplementedException();
        }

        public Task<Cell> InsertAsync(Cell entity)
        {
            throw new NotImplementedException();
        }

        public int InsertAndGetId(Cell entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAndGetIdAsync(Cell entity)
        {
            throw new NotImplementedException();
        }

        public Cell InsertOrUpdate(Cell entity)
        {
            throw new NotImplementedException();
        }

        public Task<Cell> InsertOrUpdateAsync(Cell entity)
        {
            throw new NotImplementedException();
        }

        public int InsertOrUpdateAndGetId(Cell entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertOrUpdateAndGetIdAsync(Cell entity)
        {
            throw new NotImplementedException();
        }

        public Cell Update(Cell entity)
        {
            throw new NotImplementedException();
        }

        public Task<Cell> UpdateAsync(Cell entity)
        {
            throw new NotImplementedException();
        }

        public Cell Update(int id, Action<Cell> updateAction)
        {
            throw new NotImplementedException();
        }

        public Task<Cell> UpdateAsync(int id, Func<Cell, Task> updateAction)
        {
            throw new NotImplementedException();
        }

        public void Delete(Cell entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Cell entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<Cell, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Expression<Func<Cell, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return GetAll().Count();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public int Count(Expression<Func<Cell, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<Cell, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public long LongCount()
        {
            throw new NotImplementedException();
        }

        public Task<long> LongCountAsync()
        {
            throw new NotImplementedException();
        }

        public long LongCount(Expression<Func<Cell, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<long> LongCountAsync(Expression<Func<Cell, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
