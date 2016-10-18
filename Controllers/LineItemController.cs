using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bangazon.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Bangazon.Models;

namespace Bangazon.Controllers
{
    [ProducesAttribute("application/json")]
    [Route("[controller]")]
    public class LineItemController : Controller
    {
        private BangazonContext context;

        public LineItemController(BangazonContext ctx)
        {
            context = ctx;
        }
        // GET api/values
        [HttpGet]
         public IActionResult Get()
        {
            IQueryable<object> lineItems = from lineItem in context.LineItem select lineItem;

            if (lineItems == null)
            {
                return NotFound();
            }

            return Ok(lineItems);

        }
        [HttpGet("{id}", Name = "GetLineItem")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                LineItem lineItem = context.LineItem.Single(m => m.LineItemId == id);

                if (lineItem == null)
                {
                    return NotFound();
                }
                
                return Ok(lineItem);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound();
            }


        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Models.LineItem lineItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.LineItem.Add(lineItem);
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (LineItemExists(lineItem.LineItemId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetLineItem", new { id = lineItem.LineItemId }, lineItem);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] LineItem lineItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lineItem.LineItemId )
            {
                return BadRequest();
            } 
            if (ModelState.IsValid)
            {
                context.Entry(lineItem).State = EntityState.Modified;
            }
           
            try 
            {
              context.SaveChanges();               
            } 
            catch (DbUpdateConcurrencyException)
            {
                if (!LineItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            } 
            return Ok(lineItem);
        }  
                                                                       

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            LineItem lineItem = context.LineItem.Single(m => m.LineItemId == id);

            if (lineItem == null)
            {
                return NotFound();
            }
            context.LineItem.Remove (lineItem);
            context.SaveChanges();
            return Ok(lineItem);
        }
           
           
         private bool LineItemExists(int id)
        {
            return context.LineItem.Count(e => e.LineItemId == id) > 0;
        }
    }
}
