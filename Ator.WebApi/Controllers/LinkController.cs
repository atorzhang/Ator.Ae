using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ator.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ator.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Link")]
    public class LinkController : BaseApiController
    {
        private UnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        public LinkController(UnitOfWork unitOfWork, ILogger<LinkController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        // GET: api/Link
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Link/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(string id)
        {
            var data = _unitOfWork.SysLinkItemRepository.Get(id);
            if(data == null || data.Status != 1)
            {
                return ErrRes("");
            }
            return SuccessRes(data);
        }
        
        // POST: api/Link
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Link/5
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
