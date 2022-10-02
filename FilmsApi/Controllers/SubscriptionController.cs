using FastestDeliveryApi.database;
using FilmsApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private EfModel _efModel;

        public SubscriptionController(EfModel model)
        {
            _efModel = model;
        }

        [HttpGet]
        public async Task<ActionResult<List<Subscription>>> GetSubscription()
        {
            return await _efModel.Subscriptions.ToListAsync();
        }

        [HttpGet("Main")]
        public async Task<ActionResult<Subscription>> GetSubscriptionMain()
        {
            List<Subscription> subscription = await _efModel.Subscriptions
                .Where(u => u.MainSubscription == true)
                .ToListAsync();

            return subscription.Last();
        }
    }
}
