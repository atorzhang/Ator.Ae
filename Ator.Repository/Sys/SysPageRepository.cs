using Ator.Entity;
using Ator.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysPageRepository : RepositoryBase<SysPage>
    {
        public SysPageRepository(AeDbContext context) : base(context)
        {

        }
    }
}
