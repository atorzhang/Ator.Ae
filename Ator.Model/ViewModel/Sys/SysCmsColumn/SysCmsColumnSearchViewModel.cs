using Ator.Entity;
using Ator.Entity.Sys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ator.Model.ViewModel.Sys
{
    public class SysCmsColumnSearchViewModel : PagingViewModel
    {
        [Key]
        [StringLength(32)]
        public string SysCmsColumnId { get; set; }

        /// <summary>
        /// 栏目名称
        /// </summary>
        [Display(Name = "栏目名称")]
        [StringLength(50)]
        public string ColumnName { get; set; }

        public List<SysCmsColumn> ReturnData { get; set; }
    }

}
