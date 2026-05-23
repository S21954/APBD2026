using Microsoft.AspNetCore.Mvc;
using Zad05.DTOs;
using Zad05.Services;

namespace Zad05.Controllers
{
    [Route("api/pcs")]
    [ApiController]
    public class PcsController : ControllerBase
    {
        private readonly IPcsService _pcsService;

        public PcsController(IPcsService pcsService)
        {
            _pcsService = pcsService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _pcsService.GetAll());
        }

        [HttpGet("{id}/components")]
        public async Task<IActionResult> GetComponents(int id)
        {
            var components = await _pcsService.GetComponents(id);

            if (components == null)
            {
                return NotFound();
            }

            return Ok(components);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePcDto dto)
        {
            var pc = await _pcsService.Create(dto);

            return CreatedAtAction(nameof(GetComponents), new { id = pc.Id }, pc);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePcDto dto)
        {
            var result = await _pcsService.Update(id, dto);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _pcsService.Delete(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
