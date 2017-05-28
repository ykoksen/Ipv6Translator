using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Utilities;
using Utilities.Caching;

namespace DBLayer
{
  public class AddressDb
  {
    public static void Test()
    {
      AddressCreateInfo info = new AddressCreateInfo();
      User user = new User();      
    }

    public static async Task<Address> GetByIp(IPAddress ip)
    {
      Address temp = new Address("temp", ip);
      using (var model = new AddressTranslatorModelContainer())
      {
        var result = await (from address in model.AddressSet
                     where address.IP == temp.IP
                     select address).FirstOrDefaultAsync();

        return result;
      }
    }

	  [ItemCanBeNull]
	  public static async Task<Address> Get([NotNull] string key)
    {
      NullItem<byte[]> result = Cache.GlobalCache.Get<byte[]>(key);

      if (result != null)
      {
	      if (!result.IsNull)
	      {
		      return new Address(key, result.Value, result.Value.Length == 16);
	      }
	      else
	      {
		      return null;
	      }
      }


      using (var model = new AddressTranslatorModelContainer())
      {
        Address addr = await (from address in model.AddressSet
                    where address.Name == key
                    select address).FirstOrDefaultAsync();

	      byte[] bytes = addr.IpAddress.GetAddressBytes();

        Cache.GlobalCache.Put(key, bytes);

        return addr;
      }
    }

    /// <summary>
    /// Returns false if already exists
    /// </summary>
    /// <param name="newAddress"></param>
    /// <returns></returns>
    public static async Task<bool> Create(Address newAddress)
    {
      using(Transaction scope = new Transaction())
      using (var model = new AddressTranslatorModelContainer())
      {
        var exists = from address in model.AddressSet
                     where address.Name == newAddress.Name
                     select address;

        if (exists.Count() != 0)
          return false;
                
        model.AddressSet.Add(newAddress);
        await model.SaveChangesAsync();
				Cache.GlobalCache.Put(newAddress.Name, newAddress.IpAddress.GetAddressBytes());

        scope.Complete();
      }

      return true;
    }

    public static async Task<bool> Delete([NotNull] Address address)
    {
      using (var model = new AddressTranslatorModelContainer())
      {
        model.AddressSet.Remove(address);
        await model.SaveChangesAsync();
        Cache.GlobalCache.Remove(address.Name);

        return true;
      }
    }
  }
}
