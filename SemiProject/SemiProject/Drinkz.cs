using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace SemiProject
{
    public class Drinkz
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Company Companies { get; set; }
        public Ingredient[] Ingredients { get; set; }
        public double Price { get; set; }
        public string Quality { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Company
    {
        public string Name { get; set; }
        public Address Addresses { get; set; }
        public string Description { get; set; }
    }

    public class Ingredient
    {
        public string Name { get; set; }
        public int Percent { get; set; }
        public string Description { get; set; }
    }

    public class Address
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string PostalCode { get; set; }
    }
}
