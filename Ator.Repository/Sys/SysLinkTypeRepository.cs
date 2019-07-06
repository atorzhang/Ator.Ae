using Ator.Entity;
using Ator.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysLinkTypeRepository : RepositoryBase<SysLinkType>
    {
        public SysLinkTypeRepository(AeDbContext context) : base(context)
        {

        }
    }
}
