using System.Reflection;
using Newtonsoft.Json;
using WebApplication1.ORM.Wrapper;

namespace WebApplication1.Attribute;

public class Repository : System.Attribute
{
   public class UsesRepository : System.Attribute {}
   
   public static void SetRepositories()
   {
      var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
         .Where(p => p.GetCustomAttributes(false).Any(o => o.GetType() == typeof(UsesRepository)))
         .ToList();
      
      
      types.ForEach(type =>
      {
         var properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(prop => prop.IsDefined(typeof(Repository), false));
         
         
         var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Static)
            .Where(prop => prop.IsDefined(typeof(Repository), false));

         foreach (FieldInfo fieldInfo in fields)
         {
            Console.WriteLine(RepositoryHolder.GetRepostitory(fieldInfo.FieldType));
            fieldInfo.SetValue(null, RepositoryHolder.GetRepostitory(fieldInfo.FieldType));
         }
         
         Console.WriteLine(JsonConvert.SerializeObject(fields.Select(info => info.FieldType.Name), Formatting.Indented));
      });
   }
}