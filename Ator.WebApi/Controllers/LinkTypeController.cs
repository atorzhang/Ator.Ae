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
    [Route("api/LinkType")]
    public class LinkTypeController : BaseApiController
    {
        private UnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        public LinkTypeController(UnitOfWork unitOfWork, ILogger<LinkTypeController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        // GET: api/LinkType
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/LinkType/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(string id)
        {
            var data = _unitOfWork.SysLinkTypeRepository.Get(id);
            if (data == null || data.Status != 1)
            {
                return ErrRes("");
            }
            return SuccessRes(data);
        }
        
        // POST: api/LinkType
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/LinkType/5
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
