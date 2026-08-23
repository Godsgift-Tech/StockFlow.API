using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Features.AppCategory.Command.CreateCommand;
using StockFlow.Application.Features.AppCategory.Command.UpdateCommand;
using StockFlow.Application.Features.AppCategory.DTO;

namespace StockFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
        CreateCategoryDto dto)
        {
            var command = new CreateCategoryCommand
            {
                Name = dto.Name
            };

            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }




        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
      Guid id,
      UpdateCategoryDto dto)
        {
            var command = new UpdateCategoryCommand
            {
                Id = id,
                Name = dto.Name
            };

            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}