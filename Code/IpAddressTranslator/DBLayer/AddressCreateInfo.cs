//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class AddressCreateInfo
    {
        public int Id { get; private set; }
        public System.DateTime CreationTime { get; set; }
    
        public virtual User User { get; set; }
        public virtual Address AddressLookup { get; set; }
    }
}
