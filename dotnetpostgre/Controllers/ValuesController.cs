using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using dotnetpostgre.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver.V1;
using Newtonsoft.Json;

namespace dotnetpostgre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public readonly IDriver driver;
        private readonly IDistributedCache _cache;

        public ValuesController(IDriver driver, IDistributedCache cache)
        {
            this.driver = driver;
            _cache = cache;
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var movies = new List<Movie>();
            //string contact = null;

            // Get the requested ETag
            string requestETag = "";
            bool haveCachedContact = false;
            if (Request.Headers.ContainsKey("If-None-Match"))
            {
                requestETag = Request.Headers["If-None-Match"].First();

                if (!string.IsNullOrEmpty(requestETag))
                {
                    //The client has supplied an ETag, so, get this version of the contact from our cache


                    //Construct the key for the cache which includes the entity type(i.e. "contact"), the contact id and the version of the contact record(i.e.the ETag value)
                    string oldCacheKey = $"movies-{requestETag}";

                    // Get the cached item
                    string cachedContactJson = _cache.GetString(oldCacheKey);

                    // If there was a cached item then deserialise this into our contact object
                    if (!string.IsNullOrEmpty(cachedContactJson))
                    {
                        movies = JsonConvert.DeserializeObject<List<Movie>>(cachedContactJson);
                        haveCachedContact = (movies != null);
                    }
                }
            }
            //If no data was found, then return a 404
            if (movies == null)
            {
                return NotFound();
            }

            //Construct the new ETag
            string responseETag = "dsfdsc"; //Convert.ToBase64String();

            // Add the movies to the cache for 30 mins if not already in the cache
            if (!haveCachedContact)
            {
                using (var session = driver.Session())
                {
                    var res = session.Run("Match(m:Movie) return m");
                    foreach (var record in res)
                    {
                        var node = record["m"].As<INode>();
                        //var relationship = record["r"].As<IRelationship>();
                        //var no = record["p"].As<INode>();
                        movies.Add(new Movie
                        {
                            released = node["released"].As<int>(),
                            tagline = node["tagline"].As<string>(),
                            title = node["title"].As<string>(),

                            //Relationship = relationship[].As<>()
                        });
                    }
                }

                string cacheKey = $"movies-{responseETag}";
                //_cache.SetString(cacheKey, JsonConvert.SerializeObject(movies), new DistributedCacheEntryOptions() { AbsoluteExpiration = DateTime.Now.AddMinutes(30) });
            }
            return Ok(movies.Select(m => new { movie = m }));
        }

        // GET api/values/5
        [HttpGet("{title}")]
        public IActionResult Get(string title)
        {
            var movies = new List<Movie>();
            //string contact = null;

            // Get the requested ETag
            string requestETag = "";
            bool haveCachedContact = false;
            if (Request.Headers.ContainsKey("If-None-Match"))
            {
                requestETag = Request.Headers["If-None-Match"].First();

                if (!string.IsNullOrEmpty(requestETag))
                {
                    //Construct the key for the cache which includes the entity type(i.e. "contact"), the contact id and the version of the contact record(i.e.the ETag value)
                     string oldCacheKey = $"movies-{requestETag}";

                    // Get the cached item
                    string cachedJson = _cache.GetString(oldCacheKey);

                    // If there was a cached item then deserialise this into our contact object
                    if (!string.IsNullOrEmpty(cachedJson))
                    {
                        movies = JsonConvert.DeserializeObject<List<Movie>>(cachedJson);
                        haveCachedContact = (movies != null);
                    }
                }
            }

            //If no data was found, then return a 404
            //if (movies == null)
            //{
            //    return NotFound();
            //}

            //Construct the new ETag
            string responseETag = "dsfdsc"; //Convert.ToBase64String();

            // Add the contact to the cache for 30 mins if not already in the cache
            if (!haveCachedContact)
            {
                using (var session = driver.Session())
                {
                    //var res = session.Run("Match (mv:Movie) Where mv.title=$title Return mv", new { title });
                    var res = session.Run("Match (Movie:Movie)-[Relationship:ACTED_IN]-(Person:Person) where toLower(Person.name) contains toLower($title) Return Movie, Relationship, Person", new { title });
                    foreach (var record in res)
                    {
                        var Movie = record["Movie"].As<INode>();
                        var Relationship = record["Relationship"].As<IRelationship>();
                        var Person = record["Person"].As<INode>();
                        movies.Add(new Movie
                        {
                            released = Movie["released"].As<int>(),
                            tagline = Movie["tagline"].As<string>(),
                            title = Movie["title"].As<string>()
                        });
                    }
                }

                string cacheKey = $"movies-{responseETag}";
               // _cache.SetString(cacheKey, JsonConvert.SerializeObject(movies), new DistributedCacheEntryOptions() { AbsoluteExpiration = DateTime.Now.AddMinutes(30) });
            }

            // Return a 304 if the ETag of the current record matches the ETag in the "If-None-Match" HTTP header
            if (Request.Headers.ContainsKey("If-None-Match") && responseETag == requestETag)
            {
                return StatusCode((int)HttpStatusCode.NotModified);
            }

            // Add the current ETag to the HTTP header
            Response.Headers.Add("ETag", responseETag);

            return Ok(movies.Select(m => new { movie = m }));

        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
