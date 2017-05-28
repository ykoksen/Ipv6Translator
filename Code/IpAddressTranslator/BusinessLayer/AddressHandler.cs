using System;
using System.Net;
using System.Threading.Tasks;
using DBLayer;

namespace BusinessLayer
{
	public class AddressHandler
	{
		public static Task<Address> GetAddress(string name)
		{
			return AddressDb.Get(name);
		}

		public static async Task<string> GetOrCreateName(string ipAddress)
		{
			UserHandler.HasPermission(ActionPermission.CreateName);

			IPAddress newIp = IPAddress.Parse(ipAddress);

			Address address = await AddressDb.GetByIp(newIp);

			if (address != null)
			{
				return address.Name;
			}

			throw new NotImplementedException("Not done with method");
		}
	}
}
