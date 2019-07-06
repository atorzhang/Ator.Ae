using Ator.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ator.Model.ViewModel.Sys
{
    public class SysDictionarySearchViewModel : PagingViewModel
    {
        [Display(Name = "字典名称")]
        [StringLength(32)]
        public string SysDictionaryName { get; set; }

        [Display(Name = "字典分组")]
        [StringLength(32)]
        public string SysDictionaryGroup { get; set; }

        public List<SysDictionarySearchDto> ReturnData { get; set; }
    }

    public class SysDictionarySearchDto : EntityBase
    {
        [Key]
        [StringLength(32)]
        public string SysDictionaryId { get; set; }

        [Display(Name = "字典名称")]
        [StringLength(32)]
        public string SysDictionaryName { get; set; }

        [Display(Name = "字典分组")]
        [StringLength(32)]
        public string SysDictionaryGroup { get; set; }
    }
}
