using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class MovieController : ApiController
    {
        // GET api/movies?criteria=title&value=The
        // search criteria: title, genre, year
        public IHttpActionResult Get()
        {
            var criteria = Request.GetQueryNameValuePairs().Where(nv => nv.Key.ToLower() == "criteria").Select(nv => nv.Value).FirstOrDefault();
            var value = Request.GetQueryNameValuePairs().Where(nv => nv.Key.ToLower() == "value").Select(nv => nv.Value).FirstOrDefault();

            if (criteria != null && criteria != string.Empty && value != null && value != string.Empty)
            {
                criteria = criteria.ToLower();
                value = value.ToLower();
                if (!criteria.Equals("title") && !criteria.Equals("genre") && !criteria.Equals("year"))
                    return BadRequest();
            }
            else
                return BadRequest();
        
            var movies = Data.MovieDb.GetMoviePresentations(criteria, value).AsEnumerable();

            if (movies.Count() == 0)
            {
                return NotFound();
            }

            return Ok(movies);
        }        
    }
}
