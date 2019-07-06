using Ator.Entity;
using Ator.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysRolePageRepository : RepositoryBase<SysRolePage>
    {
        public SysRolePageRepository(AeDbContext context) : base(context)
        {

        }
    }
}
