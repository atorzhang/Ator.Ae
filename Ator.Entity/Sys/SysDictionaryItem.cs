﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ator.Entity.Sys
{
    [Table("Sys_Dictionary_Item")]
    public class SysDictionaryItem : EntityBase
    {
        [Key]
        [StringLength(32)]
        public string SysDictionaryItemId { get; set; } 

        [Display(Name = "字典ID")]
        [StringLength(32)]
        public string SysDictionaryId { get; set; }

        [Display(Name = "字典项值")]
        [StringLength(255)]
        public string SysDictionaryItemValue { get; set; }

        [Display(Name = "字典项名称")]
        [StringLength(32)]
        public string SysDictionaryItemName { get; set; }

        [ForeignKey("SysDictionaryId")]
        public virtual SysDictionary SysDictionary { get; set; }
    }
}
