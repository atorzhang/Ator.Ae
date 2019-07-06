using Ator.Entity;
using Ator.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysCmsInfoCommentRepository : RepositoryBase<SysCmsInfoComment>
    {
        public SysCmsInfoCommentRepository(AeDbContext context) : base(context)
        {

        }
    }
}
