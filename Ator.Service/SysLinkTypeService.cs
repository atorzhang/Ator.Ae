using Ator.IService;
using Ator.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ator.Service
{
    public class SysLinkTypeService : ISysLinkTypeService
    {
        UnitOfWork _unitOfWork;
        public SysLinkTypeService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<KeyValuePair<string, string>> GetLinkTypeList()
        {
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();
            var all = _unitOfWork.SysLinkTypeRepository.Load().OrderBy(o => o.Sort).ToList();
            foreach (var item in all)
            {
                data.Add(new KeyValuePair<string, string>(item.SysLinkTypeId, item.SysLinkTypeName));
            }
            return data;
        }
    }
}
