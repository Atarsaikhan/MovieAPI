using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class MovieRatingController : ApiController
    {
        int TopN = 5;
    
        // GET api/movierating
        // returns top 5 movies with highest user rating
        public IHttpActionResult Get()
        {
            var movies = Data.MovieDb.GetMoviePresentations(TopN, -1).AsEnumerable();

            if (movies.Count() == 0)
            {
                return NotFound();
            }

            return Ok(movies);
        }

        // GET api/movierating/7
        // returns top 5 movies of user 7
        public IHttpActionResult Get(int id)
        {
            if (id < 0) //checking criteria
                return BadRequest();

            var movies = Data.MovieDb.GetMoviePresentations(TopN, id).AsEnumerable();

            if (movies.Count() == 0)
            {
                return NotFound();
            }

            return Ok(movies);
        }
        
        // PUT api/movierating
        public IHttpActionResult Put()
        {
            //reading querystring
            var mid = Request.GetQueryNameValuePairs().Where(nv => nv.Key.ToLower() == "movieid").Select(nv => nv.Value).FirstOrDefault();
            var uid = Request.GetQueryNameValuePairs().Where(nv => nv.Key.ToLower() == "userid").Select(nv => nv.Value).FirstOrDefault();
            var rate = Request.GetQueryNameValuePairs().Where(nv => nv.Key.ToLower() == "rate").Select(nv => nv.Value).FirstOrDefault();

            //validating
            if (mid != null && mid != string.Empty && uid != null && uid != string.Empty && rate != null && rate != string.Empty)
            {
                try
                {
                    int m = Convert.ToInt32(mid);
                    int u = Convert.ToInt32(uid);
                    double r = Convert.ToDouble(rate);

                    Data.MovieDb.UpdateMovieRating(m, u, r);

                    return Ok("Success");
                }
                catch(Exception ex)
                {
                    return BadRequest();
                }                    
            }
            else
                return BadRequest();

        }
        
    }
}
