using refactor_me.Models;
using System;
using System.Linq;

namespace refactor_me.Tests.Repository
{
    public class MockProductDbSet : MockDbSet<Product>
    {
        public override Product Find(params object[] keyValues)
        {
            return this.SingleOrDefault(product => product.Id == (Guid)keyValues.Single());
        }
    }
}
