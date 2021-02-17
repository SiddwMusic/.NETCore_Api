using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using dotnetpostgre.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace dotnetpostgre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        // GET: api/Blog
        [HttpGet]
        public IEnumerable<blog> Get()
        {
            DataLayer dl = new DataLayer();
            return dl.getblogs();
        }

        // GET: api/Blog/5
        [HttpGet("{id}", Name = "Get")]
        public blog Get(int id)
        {
            DataLayer dl = new DataLayer();
            return dl.getblog(id);
        }

        // POST: api/Blog
        [HttpPost]
        public void Post([FromBody] blog value)
        {
            DataLayer dl = new DataLayer();
            dl.saveBlog(value);
        }

        // PUT: api/Blog/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] blog value)
        {
            DataLayer dl = new DataLayer();
            dl.saveBlog(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            DataLayer dl = new DataLayer();
            //dl.deleteBlog(id);
        }
    }
}
