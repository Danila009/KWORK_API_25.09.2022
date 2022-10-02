using FastestDeliveryApi.database;
using FilmsApi.DTOs.Advertising;
using FilmsApi.Model.Advertising;
using FilmsApi.Repository;
using FilmsApi.utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FilmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisingController : ControllerBase
    {
        private EfModel _efModel;

        private Random random = new Random();

        private ImageRepository imageRepository = new ImageRepository();

        public AdvertisingController(EfModel model)
        {
            _efModel = model;
        }

        [HttpGet]
        public async Task<ActionResult<List<Advertising>>> GetAdvertising()
        {
            return await _efModel.Advertisings.ToListAsync();
        }

        [HttpGet("Random")]
        public async Task<ActionResult<Advertising>> GetAdvertisingRandom()
        {
            List<Advertising> advertising = await _efModel.Advertisings.ToListAsync();

            if (advertising.Count == 0)
                return NoContent();

            return advertising[random.Next(0,advertising.Count - 1)];
        }

        [Authorize(Roles = "ADMIN_USER")]
        [HttpPost]
        public async Task<ActionResult<Advertising>> PostAdvertising(
            CreateAdvertisingDTO advertisingDTO
            )
        {
            Advertising advertising = new Advertising
            {
                Title = advertisingDTO.Title,
                WebUrl = advertisingDTO.WebUrl
            };

            _efModel.Advertisings.Add(advertising);
            await _efModel.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAdvertising), new { id = advertising.Id }, advertising);
        }

        [Authorize(Roles = "ADMIN_USER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAdvertising(int id)
        {
            Advertising advertising = await _efModel.Advertisings.FindAsync(id);

            if (advertising == null)
                return NotFound();

            _efModel.Advertisings.Remove(advertising);
            imageRepository.DeleteImage($"{Constants.IMAGE_PATCH}/{id}");
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "ADMIN_USER")]
        [HttpPost("{id}/Image")]
        public async Task<ActionResult> PostAdvertisingImage(int id, IFormFile file)
        {
            Advertising advertising = await _efModel.Advertisings.FindAsync(id);

            if (advertising == null)
                return NotFound();

            MemoryStream memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            advertising.ImageUrl = $"{Constants.BASE_URL}/api/Advertising/{id}/Image";

            imageRepository.SaveImage(
                memoryStream.ToArray(),Constants.IMAGE_PATCH,advertising.Id.ToString()
                );
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id}/Image")]
        public async Task<ActionResult> GetAdvertisingImage(int id)
        {
            byte[] file =  imageRepository.GetImage($"{Constants.IMAGE_PATCH}/{id}");

            if (file != null)
                return File(file, "image/jpeg");
            else
                return NotFound();
        }
    }
}
