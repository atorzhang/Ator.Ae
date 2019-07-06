using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ator.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ator.Entity.Sys;

namespace Ator.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class InfoListController : BaseApiController
    {
        private UnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        public InfoListController(UnitOfWork unitOfWork, ILogger<InfoListController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        // GET: api/ListInfo
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ListInfo/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(string id,int page,int limit)
        {
            var pageData = _unitOfWork.SysCmsInfoRepository.GetPage(o=>o.SysCmsColumnId == id && o.Status == 1,$"-{nameof(SysCmsInfo.InfoTop)},{nameof(SysCmsInfo.Sort)},-{nameof(SysCmsInfo.CreateTime)}",page,limit);
            return SuccessRes(pageData.Rows, pageData.Totals);
        }
        
        // POST: api/ListInfo
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/ListInfo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
