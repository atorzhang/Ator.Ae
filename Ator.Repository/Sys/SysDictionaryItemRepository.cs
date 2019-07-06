using Ator.Entity;
using Ator.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysDictionaryItemRepository : RepositoryBase<SysDictionaryItem>
    {
        public SysDictionaryItemRepository(AeDbContext context) : base(context)
        {

        }
    }
}
