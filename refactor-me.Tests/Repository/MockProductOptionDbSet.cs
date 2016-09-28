using refactor_me.Models;
using System;
using System.Linq;

namespace refactor_me.Tests.Repository
{
    public class MockProductOptionDbSet : MockDbSet<ProductOption>
    {
        public override ProductOption Find(params object[] keyValues)
        {
            return this.SingleOrDefault(productOption => productOption.Id == (Guid)keyValues.Single());
        }
    }
}
