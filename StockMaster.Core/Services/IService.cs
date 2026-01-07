using StockMaster.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace StockMaster.Core.Services
{
    public interface IService<Entity, Dto> where Entity : class where Dto : class
    {
        Task<CustomResponseDto<Dto>> GetByIdAsync(int id);
        Task<CustomResponseDto<IEnumerable<Dto>>> GetAllAsync();
        Task<CustomResponseDto<IEnumerable<Dto>>> Where(Expression<Func<Entity, bool>> expression);
        Task<CustomResponseDto<Dto>> AddAsync(Dto dto);
        Task<CustomResponseDto<NoContentDto>> UpdateAsync(Dto dto, int id); // NoContentDto henüz yok, aşağıda tanımlayacağız
        Task<CustomResponseDto<NoContentDto>> RemoveAsync(int id);
    }
}
