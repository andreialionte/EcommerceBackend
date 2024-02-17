using Ecommerce.Models;
using Ecommerce2.Data;
using Ecommerce2.Dtos;
using Ecommerce2.Models;
using Ecommerce2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdsController : ControllerBase
    {
        private readonly DataContextEf _ef;
        private readonly ILogger<AdsController> _logger;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;

        public AdsController(IConfiguration configuration, ILogger<AdsController> logger, IStorageService storageService)
        {
            _config = configuration;
            _ef = new DataContextEf(configuration);
            _logger = logger;
            _storageService = storageService;
        }

        [HttpGet("GetAds")]
        public async Task<IActionResult> GetAds()
        {
            try
            {
                IEnumerable<Ad> adsDb = await _ef.ads.ToListAsync<Ad>();
                return Ok(adsDb);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetSingleAd/{Id}")]
        public async Task<IActionResult> GetSingleAd(int Id)
        {
            try
            {
                Ad? ad = await _ef.ads.Where(k => k.AnuntId == Id).FirstOrDefaultAsync();
                return Ok(ad);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("AddAd/{Id}")]
        public async Task<IActionResult> PostAd(AdDto adDto, int Id, [Required] int productid)
        {
            try
            {
                Ad? existingAd = await _ef.ads.FirstOrDefaultAsync(ad => ad.AnuntId == Id);
                if (existingAd != null)
                {
                    throw new Exception($"{existingAd.AnuntId} already exists!");
                }

                Product? productDb = await _ef.products
                    .Include(p => p.Ads)
                    .FirstOrDefaultAsync(p => p.ProductId == productid);

                if (_storageService == null)
                {
                    throw new Exception("_storageService is not initialized");
                }

                if (productDb != null && productDb.Ads == null)
                {
                    //Make to add multiple content-types and multiple photos for 1 ad/anunt
                    var contentType = "image/png";
                    var photoFileName = _storageService.Upload(adDto.PhotoUrl, contentType);

                    Ad newAd = new Ad()
                    {
                        AnuntId = Id,
                        Description = adDto.Description,
                        Title = adDto.Title,
                        PhotoUrl = _storageService.GetImageUrl(photoFileName),
                        ProductId = productid,
                        Product = productDb,
                    };

                    _ef.ads.Add(newAd);
                    await _ef.SaveChangesAsync();

                    return Ok(newAd);
                }
                else if (productDb == null)
                {
                    throw new Exception($"Product with Id {productid} not found!");
                }
                else
                {
                    throw new Exception($"{productDb.Name} already has an ad!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing PostAd.");
                return BadRequest(ex.Message);
            }
        }



        [HttpPut("UpdateAd/{Id}")]
        public async Task<IActionResult> UpdateAd(int Id, int productId, AdDto adDto)
        {
            try
            {
                Product? product = await _ef.products.Where(k => k.ProductId == productId).FirstOrDefaultAsync();
                Ad? adDb = await _ef.ads.Where(k => k.AnuntId == Id).FirstOrDefaultAsync();

                if (adDb != null)
                {
                    if (product.ProductId == null)
                    {
                        throw new Exception($"{product.ProductId} dosent exists");
                    }

                    adDb.Title = adDto.Title;
                    adDb.Description = adDto.Description;
                    adDb.ProductId = productId;

                    await _ef.SaveChangesAsync();

                    return Ok(adDb);
                }
                else
                {
                    return NotFound("Ad not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteAd/{Id}")]
        public async Task<IActionResult> DeleteAd(int Id)
        {
            try
            {
                Ad? adDb = await _ef.ads.Where(k => k.AnuntId == Id).FirstOrDefaultAsync();

                if (adDb != null)
                {
                    _ef.ads.Remove(adDb);
                    await _ef.SaveChangesAsync();
                    return Ok(adDb);
                }
                else
                {
                    return NotFound("Ad not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

