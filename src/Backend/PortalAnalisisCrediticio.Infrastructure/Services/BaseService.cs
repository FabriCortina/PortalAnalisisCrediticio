using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public abstract class BaseService<TEntity, TDto> where TEntity : class where TDto : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly IMapper _mapper;
    protected readonly DbSet<TEntity> _dbSet;

    protected BaseService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TDto>> GetAllAsync()
    {
        var entities = await _dbSet.ToListAsync();
        return _mapper.Map<IEnumerable<TDto>>(entities);
    }

    public virtual async Task<TDto> GetByIdAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task<TDto> CreateAsync(TDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task<TDto> UpdateAsync(int id, TDto dto)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity == null)
            return null;

        _mapper.Map(dto, entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
} 