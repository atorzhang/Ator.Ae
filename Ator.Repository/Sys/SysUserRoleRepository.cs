using Ator.Entity;
using Ator.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysUserRoleRepository : RepositoryBase<SysUserRole>
    {
        public SysUserRoleRepository(AeDbContext context) : base(context)
        {

        }
    
    }
}
