using System;
using System.Threading.Tasks;
using BusinessLayer;
using DBLayer;

namespace IpAddressTranslator
{
  public class IpAddressTranslator : IIpAddressTranslator
  {
    public async Task<string> GetIp(string name)
    {
      Address addr = await AddressHandler.GetAddress(name);

      return addr?.IpAddress.ToString();
    }

    public async Task<string> GenerateName(Authentication auth, string ipAddress)
    {
      await Authenticate(auth);

      return await AddressHandler.GetOrCreateName(ipAddress);
    }

    public async Task<bool> RevokeName(Authentication auth, string ipAddress)
    {
			await Authenticate(auth);

      throw new NotImplementedException();
    }

    public async Task<bool> TryCreateName(Authentication auth, string ipAddress, string requestedName)
    {
			await Authenticate(auth);

      throw new NotImplementedException();
    }

    private async Task Authenticate(Authentication auth)
    {
			await UserHandler.Logon(auth.Username, auth.Password, auth.PasswordTime);
    }
  }
}
