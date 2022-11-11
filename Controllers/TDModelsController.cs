using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDM.DTOs;
using TDM.Interfaces;
using TDM.Models;

namespace TDM.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TDModelsController : ControllerBase
    {
        private readonly ITDModelRepository _tDModelRep;
        public TDModelsController(ITDModelRepository tDModelRep)
        {
            _tDModelRep = tDModelRep;
        }

        // GET: api/TDModels?userId=1
        public async Task<ActionResult<IEnumerable<TDModelDTO>>> GetTDModels(long userId)
        {
            try
            {
                var tDModels = await _tDModelRep.GetTDModels(userId);
                var tDModelsDTO = tDModels.Select(x => GetTDModelDTOForList(x));
                return Ok(tDModelsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //GET: api/TDModels/GetTDModel/5
        [Route("[action]")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TDModelDTO>> GetTDModel(long id)
        {
            try {
                var tDModel = await _tDModelRep.GetTDModel(id);
                
                if (tDModel == null)
                {
                    return NotFound();
                }

                var tDModelDTO = GetTDModelDTOForItem(tDModel);
            
                return Ok(tDModelDTO);
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/TDModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TDModelDTO>> PostTDModel([FromForm] TDModelDTOPost tDModelDTOPost)
        {
            try {
                if (tDModelDTOPost.ModelIFormFile != null) {
                    IFormFile modelIFormFile = tDModelDTOPost.ModelIFormFile;
                    string modelName = modelIFormFile.FileName;
                    TDModelDTO tDModel = new TDModelDTO() {
                        UserId = tDModelDTOPost.UserId,
                        Email = tDModelDTOPost.Email,
                        ModelIFormFile = modelIFormFile,
                        ModelName = modelName,
                        CreatedDate =  DateTime.Now,
                        UpdatedDate =  DateTime.Now,
                    };
                    TDModelDTO tDModelDTO = await _tDModelRep
                    .CreateTDModel(tDModel);
                    return CreatedAtAction (
                        nameof(PostTDModel),
                        new {id = tDModelDTO.Id},
                        tDModelDTO
                    );
                }
                else {
                    return BadRequest();
                }
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/TDModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTDModel(long id, [FromForm] TDModelDTO tDModelDTO)
        {
            if (id != tDModelDTO.Id) {
                return BadRequest();
            }

            try {
                if (!TDModelExists(id)) {
                    return NotFound();
                }
                await _tDModelRep.UpdateTDModel(id, tDModelDTO);
                return NoContent();
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/TDModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTDModel(long id)
        {
            try {
                
                if (!TDModelExists(id))
                {
                    return NotFound();
                }

                await _tDModelRep.DeleteTDModel(id);
                return NoContent();
            }
            catch (Exception ex) {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [NonAction]
        private bool TDModelExists(long id)
        {
            bool exists = (_tDModelRep.GetTDModel(id) != null);
            return exists;
        }

        [NonAction]
        private static TDModelDTO GetTDModelDTOForList
            (TDModel x) => new TDModelDTO() {
            Id = x.Id,
            Email = x.Email,
            ModelName = x.ModelName,
            CreatedDate = x.CreatedDate,
            UpdatedDate = x.UpdatedDate,
        };

        [NonAction]
        private static TDModelDTO GetTDModelDTOForItem
            (TDModel x) => new TDModelDTO() {
            Id = x.Id,
            ModelName = x.ModelName,
            Model = x.Model,
            ModelIFormFile = x.ModelIFormFile,
            CreatedDate = x.CreatedDate,
            UpdatedDate = x.UpdatedDate,
        };
    }
}