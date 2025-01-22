using DomainDDD.Entities.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainDDD.Entities.SubProduct;

namespace DomainDDD.Entities.Product
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public List<SubProduct.SubProduct> subProducts { get; private set; }
        public ProductState state { get; private set; }
    }
}
