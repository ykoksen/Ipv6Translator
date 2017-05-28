using System.ServiceModel;
using System.Threading.Tasks;

namespace IpAddressTranslator
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IIpAddressTranslator
    {
        [OperationContract]
        Task<string> GetIp(string name);

        [OperationContract]
        Task<string> GenerateName(Authentication auth, string ipAddress);

        [OperationContract]
        Task<bool> RevokeName(Authentication auth, string ipAddress);

        [OperationContract]
        Task<bool> TryCreateName(Authentication auth, string ipAddress, string requestedName);
    }
}
