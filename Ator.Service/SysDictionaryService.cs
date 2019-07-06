using Ator.Entity.Sys;
using Ator.IService;
using Ator.Model;
using Ator.Repository;
using Ator.Utility.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Ator.Service
{
    public class SysDictionaryService : ISysDictionaryService
    {
        UnitOfWork _unitOfWork;
        public SysDictionaryService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<KeyValuePair<string, string>> GetDictionaryList()
        {
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();
            var all = _unitOfWork.SysDictionaryRepository.Load().OrderBy(o => o.Sort).ToList();
            foreach (var item in all)
            {
                data.Add(new KeyValuePair<string, string>(item.SysDictionaryId, item.SysDictionaryName));
            }
            return data;
        }
    }
}
