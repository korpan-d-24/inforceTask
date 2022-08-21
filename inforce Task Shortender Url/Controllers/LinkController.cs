using Bussines_logic.Interface;
using Domains.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;

namespace inforce_Task_Shortender_Url.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinksController : ControllerBase
    {
        public ILinkService _linkService { get; set; }
        public LinksController(ILinkService linkService)
        {
            _linkService = linkService;
        }
        [HttpGet]
        public async Task<IActionResult> GetLinks([FromQuery]LinkFilteringModel filteringModel)
        {
            var links = await _linkService.GetLinks(filteringModel);

            return Ok(links);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLink(int id)
        {
            var link = await _linkService.GetLink(id);
            if (link == null)
            {
                return BadRequest();
            }
            return Ok(link);
        }
        [HttpPost]
        public async Task<IActionResult> AddLink([FromBody] LinkDTO link)
        {
            if (link == null)
            {
                return BadRequest();
            }
            link.CurrentUrl = HttpContext.Request.GetEncodedUrl();
            link.Path = HttpContext.Request.Path.Value;
            var isSuccessfull = await _linkService.AddLink(link);
            if (!isSuccessfull)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isSuccessfull = await _linkService.DeleteLink(id);
            if (!isSuccessfull)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
