using Ator.Entity;
using Ator.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysCmsColumnRepository : RepositoryBase<SysCmsColumn>
    {
        public SysCmsColumnRepository(AeDbContext context) : base(context)
        {

        }
    
    }
}
