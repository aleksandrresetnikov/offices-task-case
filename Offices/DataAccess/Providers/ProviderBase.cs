using Microsoft.EntityFrameworkCore;
using Offices.Models.Entities;
using Offices.Utils;

namespace Offices.DataAccess.Providers;

/// <summary>
/// Базовый провайдер для работы с сущностями БД, реализующий стандартные CRUD операции.
/// </summary>
public abstract class ProviderBase<TEntity> where TEntity : TypeBase
{
    protected readonly DellinDictionaryDbContext Context;
    
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="context">Контекст данных.</param>
    protected ProviderBase(DellinDictionaryDbContext context)
    {
        Context = context;
    }
    
    /// <summary>
    /// Верет DbSet для сущности данного провайдера
    /// </summary>
    /// <param name="contextBase">Инстанция соединения с бд</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected DbSet<TEntity> GetDbSet()
    {
        return Context.Set<TEntity>() 
               ?? throw new InvalidOperationException(
                   $"Не удалось получить DbSet для сущности '{typeof(TEntity).Name}'. " +
                   $"Проверьте, что тип зарегистрирован в вашем DbContext (через DbSet<T> или OnModelCreating).");
    }

    /// <summary>
    /// Получает все сущности типа <typeparamref name="TEntity"/> из БД.
    /// </summary>
    /// <returns>Коллекция сущностей.</returns>
    public virtual IEnumerable<TEntity> ListAll()
    {
        return GetDbSet()
            .AsNoTracking() // в EF на отслеживание объектов уходит много ресов
            .ToList();
    }
    
    /// <summary>
    /// Асинхронно получает все сущности типа <typeparamref name="TEntity"/> из БД без отслеживания изменений.
    /// </summary>
    /// <param name="ct">Токен отмены операции.</param>
    /// <returns>Коллекция сущностей.</returns>
    public virtual async Task<IEnumerable<TEntity>> ListAllAsync(CancellationToken ct = default)
    {
        return await GetDbSet()
            .AsNoTracking() // в EF на отслеживание объектов уходит много ресов
            .ToListAsync(cancellationToken: ct);
    }

    
    /// <summary>
    /// Проверяет существование сущности с указанным ID.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>True, если сущность существует, иначе False.</returns>
    public virtual bool Has(int id)
    {
        return GetDbSet()
            .Any(i => i.Id == id);
    }
    
    /// <summary>
    /// Асинхронно проверяет существование сущности с указанным ID.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="ct">Токен отмены операции.</param>
    /// <returns>True, если сущность существует, иначе False.</returns>
    public virtual async Task<bool> HasAsync(int id, CancellationToken ct = default)
    {
        return await GetDbSet()
            .AnyAsync(i => i.Id == id, cancellationToken: ct);
    }

    
    /// <summary>
    /// Получает сущность по ID.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>Найденная сущность или null.</returns>
    public virtual TEntity? Get(int id)
    {
        return GetDbSet()
            .FirstOrDefault(item => item.Id == id);
    }
    
    /// <summary>
    /// Асинхронно получает сущность по ID.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="ct">Токен отмены операции.</param>
    /// <returns>Найденная сущность или null.</returns>
    public virtual async Task<TEntity?> GetAsync(int id, CancellationToken ct = default)
    {
        return await GetDbSet()
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken: ct);
    }

    
    /// <summary>
    /// Получает список сущностей по массиву идентификаторов.
    /// </summary>
    /// <param name="ids">Массив идентификаторов.</param>
    /// <returns>Коллекция найденных сущностей.</returns>
    public virtual IEnumerable<TEntity> GetByIds(int[] ids)
    {
        return GetDbSet()
            .Where(i => ids.Contains(i.Id))
            .ToList();
    }
    
    /// <summary>
    /// Асинхронно получает список сущностей по массиву идентификаторов.
    /// </summary>
    /// <param name="ids">Массив идентификаторов.</param>
    /// <param name="ct">Токен отмены операции.</param>
    /// <returns>Коллекция найденных сущностей.</returns>
    public virtual async Task<IEnumerable<TEntity>> GetByIdsAsync(int[] ids, CancellationToken ct = default)
    {
        return await GetDbSet()
            .Where(i => ids.Contains(i.Id))
            .ToListAsync(cancellationToken: ct);
    }


    /// <summary>
    /// Добавляет новую сущность в БД.
    /// </summary>
    /// <param name="item">Добавляемая сущность.</param>
    /// <returns>Добавленная сущность (с заполненным ID).</returns>
    public virtual TEntity? Add(TEntity item)
    {
        var dbSet = GetDbSet();
        var newEntity = dbSet.Add(item).Entity;

        Context.SaveChanges();

        return newEntity;
    }
    
    /// <summary>
    /// Асинхронно добавляет новую сущность в БД.
    /// </summary>
    /// <param name="item">Добавляемая сущность.</param>
    /// <param name="ct">Токен отмены операции.</param>
    /// <returns>Добавленная сущность с присвоенным идентификатором.</returns>
    public virtual async Task<TEntity?> AddAsync(TEntity item, CancellationToken ct = default)
    {
        var result = await GetDbSet()
            .AddAsync(item, cancellationToken: ct);
        
        var newEntity = result.Entity;
        await Context.SaveChangesAsync(cancellationToken: ct);

        return newEntity;
    }
    
    
    /// <summary>
    /// Массово добавляет сущности в БД.
    /// </summary>
    /// <param name="items">Коллекция сущностей для добавления.</param>
    public virtual void AddMany(IEnumerable<TEntity> items)
    {
        if (items == null || !items.Any()) return;

        GetDbSet().AddRange(items);
        Context.SaveChanges();
    }
    
    /// <summary>
    /// Асинхронно добавляет коллекцию сущностей в БД.
    /// </summary>
    /// <param name="items">Коллекция сущностей для добавления.</param>
    /// <param name="ct">Токен отмены операции.</param>
    public virtual async Task AddManyAsync(IEnumerable<TEntity> items, CancellationToken ct = default)
    {
        if (items == null || !items.Any()) return;

        await GetDbSet()
            .AddRangeAsync(items, cancellationToken: ct);

        await Context.SaveChangesAsync(cancellationToken: ct);
    }

    /// <summary>
    /// Добавляет коллекцию сущностей и возвращает их уже с заполненными ID.
    /// </summary>
    /// <param name="items">Коллекция сущностей для добавления.</param>
    /// <returns>Коллекция добавленных сущностей.</returns>
    public virtual IEnumerable<TEntity> AddManyWithGet(IEnumerable<TEntity> items)
    {
        List<TEntity> outputValues = new List<TEntity>();
        var dbSet = GetDbSet();

        items.Foreach(item =>
        {
            var result = dbSet.Add(item);
            outputValues.Add(result.Entity);
        });

        Context.SaveChanges();
        return outputValues;
    }
    
    /// <summary>
    /// Асинхронно добавляет коллекцию сущностей и возвращает их уже с заполненными ID.
    /// </summary>
    /// <param name="items">Коллекция сущностей для добавления.</param>
    /// <param name="ct">Токен отмены операции.</param>
    /// <returns>Коллекция добавленных сущностей.</returns>
    public virtual async Task<IEnumerable<TEntity>> AddManyWithGetAsync(IEnumerable<TEntity> items, CancellationToken ct = default)
    {
        List<TEntity> outputValues = new List<TEntity>();
        var dbSet = GetDbSet();

        await items.ForeachAsync(async item =>
        {
            var result = await dbSet
                .AddAsync(item, cancellationToken: ct);
            
            outputValues.Add(result.Entity);
        }, cancellationToken: ct);

        await Context.SaveChangesAsync(cancellationToken: ct);
        return outputValues;
    }

    
    /// <summary>
    /// Удаляет сущность по ID.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>True, если удаление прошло успешно, иначе False.</returns>
    public virtual bool Delete(int id)
    {
        var dbSet = GetDbSet();
        
        return dbSet
            .Where(i => i.Id == id)
            .ExecuteDelete() > 0;
    }
    
    /// <summary>
    /// Асинхронно удаляет сущность по ID.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <returns>True, если удаление прошло успешно.</returns>
    public virtual async Task<bool> DeleteAsync(int id)
    {
        var dbSet = GetDbSet();
        
        return await dbSet
            .Where(i => i.Id == id)
            .ExecuteDeleteAsync() > 0;
    }
    
    
    /// <summary>
    /// Удаляет несколько сущностей по массиву ID.
    /// </summary>
    /// <param name="ids">Массив идентификаторов.</param>
    /// <returns>Количество удаленных записей.</returns>
    public int DeleteMany(int[] ids)
    {
        var dbSet = GetDbSet();

        return dbSet.Where(i => ids.Contains(i.Id))
            .ExecuteDelete();
    }

    /// <summary>
    /// Асинхронно удаляет несколько сущностей по массиву ID.
    /// </summary>
    /// <param name="ids">Массив идентификаторов.</param>
    /// <returns>Количество удаленных записей.</returns>
    public async Task<int> DeleteManyAsync(int[] ids)
    {
        var dbSet = GetDbSet();

        return await dbSet.Where(i => ids.Contains(i.Id))
            .ExecuteDeleteAsync();
    }

    
    /// <summary>
    /// Обновляет данные существующей сущности.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="updateStatement">Объект с новыми данными.</param>
    /// <returns>True, если обновление успешно, иначе False.</returns>
    public virtual bool Update(int id, TEntity updateStatement)
    {
        var dbSet = GetDbSet();
        var exitingEntity = dbSet.FirstOrDefault(i => i.Id == id);
        if (exitingEntity == null) return false;
        
        updateStatement.Id = id;
        updateStatement.UpdateDate = DateTime.UtcNow;
        
        Context.Entry(exitingEntity).CurrentValues.SetValues(updateStatement);
        Context.SaveChanges();
        return true;
    }
    
    /// <summary>
    /// Асинхронно обновляет данные существующей сущности.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    /// <param name="updateStatement">Объект с новыми данными.</param>
    /// <returns>True, если обновление успешно, иначе False.</returns>
    public virtual async Task<bool> UpdateAsync(int id, TEntity updateStatement)
    {
        var dbSet = GetDbSet();
        var exitingEntity = await dbSet.FirstOrDefaultAsync(i => i.Id == id);
        if (exitingEntity == null) return false;
        
        updateStatement.Id = id;
        updateStatement.UpdateDate = DateTime.UtcNow;
        
        Context.Entry(exitingEntity).CurrentValues.SetValues(updateStatement);
        await Context.SaveChangesAsync();
        return true;
    }
}