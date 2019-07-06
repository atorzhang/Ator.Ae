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
    [Route("api/[controller]")]
    public class ColumnListController : BaseApiController
    {
        private UnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        public ColumnListController(UnitOfWork unitOfWork, ILogger<ColumnListController> logger)
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
        public IActionResult Get(string id)
        {
            var list = _unitOfWork.SysCmsColumnRepository.GetList(o => o.ColumnParent == id && o.Status == 1).OrderBy(o => o.Sort).ToList();
            return SuccessRes(list);
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
