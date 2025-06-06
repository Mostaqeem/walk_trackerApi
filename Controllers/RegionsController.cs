using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Models.Domain;
using NZWalks.Models.DTOs;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbcontext dbContext;
        public RegionsController(NZWalksDbcontext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await dbContext.Regions.ToListAsync();

            //List of DTO
            var regionsDto = new List<RegionDTO>();

            foreach (var region in regions)
            {
                regionsDto.Add(new RegionDTO()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl

                });
            }
            return Ok(regionsDto);
        }
        //GET single region
        //GET: https://localhost:portnumber/api/region/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            //Does the samething
            //var region = dbContext.Regions.Find(id);
            var region = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (region == null)
            {
                return NotFound();
            }

            //New single item dto
            var regionsDto = new RegionDTO()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };
            return Ok(regionsDto);
        }

        //POST to Create new Region
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionDTO addRegionDTO)
        {
            //Map DTO to domain model
            var regionDomain = new Region()
            {
                Name = addRegionDTO.Name,
                Code = addRegionDTO.Code,
                RegionImageUrl = addRegionDTO.RegionImageUrl
            };

            await dbContext.Regions.AddAsync(regionDomain);
            await dbContext.SaveChangesAsync();


            //Domain model to dto
            var regionDto = new RegionDTO
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }


        //update region
        //PUT: https://localhost:portnumber/api/region/{id}

        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionDTO updateDto)
        {
            var toUpdate = dbContext.Regions.Find(id);

            if (toUpdate == null)
            {
                return NotFound();
            }

            toUpdate.Name = updateDto.Name;
            toUpdate.Code = updateDto.Code;
            toUpdate.RegionImageUrl = updateDto.RegionImageUrl;

            dbContext.SaveChanges();

            RegionDTO showUpdate = new RegionDTO
            {
                Id = toUpdate.Id,
                Code = toUpdate.Code,
                Name = toUpdate.Name,
                RegionImageUrl = toUpdate.RegionImageUrl

            };

            return Ok(showUpdate);

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var regionModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionModel == null)
            {
                return NotFound(); 
            }

            dbContext.Regions.Remove(regionModel);
            dbContext.SaveChanges();

            var regionDTO = new RegionDTO
            {
                Id = regionModel.Id,
                Name = regionModel.Name,
                Code = regionModel.Code,
                RegionImageUrl = regionModel.RegionImageUrl

            };
            return Ok(regionDTO);
        }
    }
}
