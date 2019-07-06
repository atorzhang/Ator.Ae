using Ator.Entity;
using Ator.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysSettingRepository : RepositoryBase<SysSetting>
    {
        public SysSettingRepository(AeDbContext context) : base(context)
        {

        }
    }
}
