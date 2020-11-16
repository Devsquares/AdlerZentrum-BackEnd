using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Address.Queries.GetAllAddresses
{
    public class GetAllAddressesViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }

    }
}
