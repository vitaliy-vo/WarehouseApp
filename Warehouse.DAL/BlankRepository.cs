using Microsoft.EntityFrameworkCore;
using Warehouse.Core;
using Warehouse.Core.Dtos;
using Warehouse.Core.IRepositories;

namespace Warehouse.DAL
{
    public class BlankRepository : IBlankRepository
    {
        private DataContext _dataContext;

        public BlankRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<BlankDto> GetAll()
        {
            var result = _dataContext.Blanks.OrderBy(x => x.NameValue).ToList();
            return result;
        }

        public List<BlankDto> GetBlanksByIdWarehouseId(int warehouseId)
        {
            throw new NotImplementedException();
        }

        public BlankDto AddBlank(BlankDto blank)
        {
            _dataContext.Blanks.Add(blank);
            _dataContext.SaveChanges();
            return blank;
        }

        public async Task<BlankDto> GetByIdAsync(int id)
        {
            return await _dataContext.Blanks.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateAsync(BlankDto blank)
        {
            _dataContext.Blanks.Update(blank);
        }

        public async Task SaveChangesAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
