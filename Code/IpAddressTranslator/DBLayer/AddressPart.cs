using System;
using System.Linq;
using System.Net;

namespace DBLayer
{
  public partial class Address
  {
    protected Address()
    { }

    public Address(string name, IPAddress address)
    {
      Name = name;
      IP = new Guid(address.GetAddressBytes());
    }

    private IPAddress _ipAddress;

    public IPAddress IpAddress
    {
      get
      {
        if (_ipAddress == null)
        {
          if (IPv6)
          {
            _ipAddress = new IPAddress(IP.ToByteArray());
          }
          else
          {
            _ipAddress = new IPAddress(IP.ToByteArray().Take(4).ToArray());
          }          
        }

        return _ipAddress;
      }      
    }

    internal Address(string key, byte[] ip, bool isIPv6)
    {
      Name = key;
      SetIpValue(ip, isIPv6);
    }

    internal Address(string name, string ip)
    {
      Name = name;
      _ipAddress = IPAddress.Parse(ip);
      byte[] bytes = _ipAddress.GetAddressBytes();
      bool isIPv6 = _ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6;

      SetIpValue(bytes, isIPv6);
    }

    private void SetIpValue(byte[] bytes, bool isIPv6)
    {
      if (isIPv6)
      {
        IP = new Guid(bytes);
      }
      else
      {
        byte[] bytesIp = new byte[16];
        bytes.CopyTo(bytesIp, 0);
        IP = new Guid(bytesIp);
      }
    }
  }
}
