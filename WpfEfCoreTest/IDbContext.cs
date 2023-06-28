using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfEfCoreTest
{
    public interface IDbContext
    {
        public TestContext DbContext { get; }
    }
}
