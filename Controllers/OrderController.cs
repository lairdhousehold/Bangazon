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
    public class OrdersController : Controller
    {
        private BangazonContext context;

        public OrdersController(BangazonContext ctx)
        {
            context = ctx;
        }
        // GET api/values
        [HttpGet]
         public IActionResult Get()
        {
            IQueryable<object> orders = from order in context.Order select order;

            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);

        }
        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Order order = context.Order.Single(m => m.OrderId == id);

                if (order == null)
                {
                    return NotFound();
                }
                
                return Ok(order);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound();
            }


        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Models.Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Order.Add(order);
            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrderExists(order.OrderId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetOrder", new { id = order.OrderId }, order);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.OrderId )
            {
                return BadRequest();
            } 
            if (ModelState.IsValid)
            {
                context.Entry(order).State = EntityState.Modified;
            }
           
            try 
            {
              context.SaveChanges();               
            } 
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            } 
            return Ok(order);
        }  
                                 

        // DELETE api/values/5
        [HttpDelete("{id}")]
       public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Order order = context.Order.Single(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }
            context.Order.Remove(order);
            context.SaveChanges();
            return Ok(order);
        }
         private bool OrderExists(int id)
        {
            return context.Order.Count(e => e.OrderId == id) > 0;
        }
    }
}
