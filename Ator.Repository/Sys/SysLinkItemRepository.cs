﻿using Ator.Entity;
using Ator.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Repository.Sys
{
    public class SysLinkItemRepository : RepositoryBase<SysLinkItem>
    {
        public SysLinkItemRepository(AeDbContext context) : base(context)
        {

        }
    }
}
