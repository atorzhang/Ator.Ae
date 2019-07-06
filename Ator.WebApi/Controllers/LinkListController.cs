using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ator.Entity.Sys;
using Ator.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ator.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/ListLink")]
    public class LinkListController : BaseApiController
    {
        private UnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        public LinkListController(UnitOfWork unitOfWork, ILogger<LinkListController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/ListLink
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ListLink/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(string id,int page=1,int limit = 10)
        {
            var pageData = _unitOfWork.SysLinkItemRepository.GetPage(o => o.SysLinkTypeId == id && o.Status == 1, $"{nameof(SysLinkItem.Sort)},-{nameof(SysLinkItem.CreateTime)}", page, limit);
            return SuccessRes(pageData.Rows, pageData.Totals);
        }
        
        // POST: api/ListLink
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/ListLink/5
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
