using Ator.Entity;
using Ator.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysRoleRepository : RepositoryBase<SysRole>
    {
        public SysRoleRepository(AeDbContext context) : base(context)
        {

        }
    }
}
