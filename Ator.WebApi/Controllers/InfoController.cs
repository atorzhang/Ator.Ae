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
    public class InfoController : BaseApiController
    {
        private UnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        public InfoController(UnitOfWork unitOfWork, ILogger<InfoController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/Info/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(string id)
        {
            var data = _unitOfWork.SysCmsInfoRepository.Get(id);
            if(data == null || data.Status != 1)
            {
                return ErrRes("无数据");
            }
            return SuccessRes(data);
        }
        
        // POST: api/Info
        [HttpPost]
        public void Post([FromBody]string value)
        {

        }
        
        // PUT: api/Info/5
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
