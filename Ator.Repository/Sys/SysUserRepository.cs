using Ator.Entity;
using Ator.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysUserRepository : RepositoryBase<SysUser>
    {
        public SysUserRepository(AeDbContext context) : base(context)
        {

        }
    }
}
