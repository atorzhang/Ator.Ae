using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.Entity.Sys
{
    [Table("Sys_UserRole")]
    public class SysUserRole:EntityBase
    {
        [Key]
        [StringLength(32)]
        public string SysUserRoleId { get; set; }
        [StringLength(32)]
        public string SysUserId { get; set; }
        [StringLength(32)]
        public string SysRoleId { get; set; }

        [ForeignKey("SysRoleId")]
        public virtual SysRole SysRole { get; set; }
        [ForeignKey("SysUserId")]
        public virtual SysUser SysUser { get; set; }
    }
}
