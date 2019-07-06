using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ator.Entity.Sys.Map
{
    public class SysUserMap : IEntityTypeConfiguration<SysUser>
    {
        public void Configure(EntityTypeBuilder<SysUser> builder)
        {        
            // 最大为32个字符        
            //builder.Property(o => o.SysUserId).HasMaxLength(32);
            // 设置实体AspNetUsers的表名为Users        
            //builder.ToTable("SysUser");
            
        }
    }
    
}
