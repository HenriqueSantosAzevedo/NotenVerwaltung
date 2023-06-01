namespace NotenAppConsoleSchueler.ORM.Wrapper;

public class RepositoryHolder
{
    private static readonly Dictionary<Type, IBaseRepository> _repositories = new();

    public static void AddRepository<T, IDT>(BaseRepository<T, IDT> baseRepository)
        where T : NotenAppConsoleSchueler.ORM.Wrapper.Entity
        where IDT : new()
    {
        Type? baseType = baseRepository.GetType().BaseType;
        if (baseType == null)
            throw new Exception();
        if (!baseType.IsGenericType)
            throw new Exception();
        Type type = baseType.GenericTypeArguments.First();
        _repositories.Add(type, baseRepository);
    }

    public static IBaseRepository? GetRepostitoryByType(Type type)
    {
        return _repositories.TryGetValue(type, out var value) ? value : null;
    }
    
    public static IBaseRepository? GetRepostitory(Type repository)
    {
        return _repositories.Values.FirstOrDefault(baseRepository => baseRepository?.GetType() == repository, null);
    }

    public static int load()
    {
        var type = typeof(IBaseRepository);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p))
            .Where(p => p.IsClass)
            .Where(p => !p.IsGenericType)
            .ToList();

        types.ForEach(type1 =>
        {
            Console.WriteLine($"Loading repository {type1.Name}");
            Activator.CreateInstance(type1);
        });
        
        return types.Count();
    }
}