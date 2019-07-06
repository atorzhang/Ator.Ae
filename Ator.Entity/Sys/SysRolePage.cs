using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.Entity.Sys
{
    [Table("Sys_RolePage")]
    public class SysRolePage:EntityBase
    {
        [Key]
        [StringLength(32)]
        public string SysRolePageId { get; set; }

        [Display(Name = "页面ID")]
        [StringLength(32)]
        public string SysPageId { get; set; }

        [Display(Name = "角色ID")]
        [StringLength(32)]
        public string SysRoleId { get; set; }

        [ForeignKey("SysRoleId")]
        public virtual SysRole SysRole { get; set; }

        [ForeignKey("SysPageId")]
        public virtual SysPage SysPage { get; set; }

    }
}
