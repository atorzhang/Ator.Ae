using Ator.Entity;
using Ator.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysDictionaryRepository : RepositoryBase<SysDictionary>
    {
        public SysDictionaryRepository(AeDbContext context) : base(context)
        {

        }
    }
}
