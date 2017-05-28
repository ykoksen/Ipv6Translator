using System;
using System.Runtime.Serialization;

namespace IpAddressTranslator
{
  [DataContract]
  public class Authentication
  {
    [DataMember]
    public string Username { get; set; }

    [DataMember]
    public DateTime PasswordTime { get; set; }

    [DataMember]
    public string Password { get; set; }    
  }
}