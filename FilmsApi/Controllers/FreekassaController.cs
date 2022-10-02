using FastestDeliveryApi.database;
using FilmsApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreekassaController : ControllerBase
    {
        private EfModel _efModel;

        public FreekassaController(EfModel model)
        {
            _efModel = model;
        }

        [Authorize(Roles = "ADMIN_USER")]
        [HttpGet]
        public async Task<ActionResult<List<Freekassa>>> GetFreekassa()
        {
            return await _efModel.Freekassas.ToListAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Freekassa>> GetFreekassaById(int id)
        {
            return await _efModel.Freekassas.FindAsync(id);
        }

        [Authorize]
        [HttpGet("ShopId")]
        public async Task<ActionResult<Freekassa>> GetFreekassaByShopId(int shopId)
        {
            return await _efModel.Freekassas.FirstOrDefaultAsync(u => u.ShopId == shopId);
        }

        [Authorize(Roles = "ADMIN_USER")]
        [HttpPost]
        public async Task<ActionResult> PostFreekassa(Freekassa freekassa)
        {
            _efModel.Freekassas.Add(freekassa);
            await _efModel.SaveChangesAsync();

            return Ok();
        }
    }
}
