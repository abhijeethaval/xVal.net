using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XVal.Core.Tests.TestData
{
    public class Employee
    {
        public int? Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public Address Address { get; set; }

        public IEnumerable<PhoneNumber> ContactNumbers { get; set; }
    }

    public class PhoneNumber
    {
        public int? Number { get; set; }

        public PhoneType? Type { get; set; }
    }

    public enum PhoneType
    {
        Mobile,
        Home,
        Office,
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
    }
}
