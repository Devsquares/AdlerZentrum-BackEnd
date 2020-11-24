using System.Collections;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Address : AuditableBaseEntity
    {
      public string   Street {get;set;}
        public int HouseNumber {get;set;}
        public string City {get;set;}
        public int PostalCode {get;set;}

    }
}
