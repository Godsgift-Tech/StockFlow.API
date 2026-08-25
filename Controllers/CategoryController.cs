using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockFlow.Application.Features.AppCategory.Command.CreateCommand;
using StockFlow.Application.Features.AppCategory.Command.DeleteComand;
using StockFlow.Application.Features.AppCategory.Command.UpdateCommand;
using StockFlow.Application.Features.AppCategory.DTO;
using StockFlow.Application.Features.AppCategory.Queries.GetAllCategoies;
using StockFlow.Application.Features.AppCategory.Queries.GetCategoryById;

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
                Name = dto.Name,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl
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
                Name = dto.Name,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl



            };

            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        // GET BY ID
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetCategoryByIdQuery(id);

            var response = await _mediator.Send(query);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        // GET PAGED
        [HttpGet("Multiples")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5)
        {
            var query = new GetPagedCategoriesQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var response = await _mediator.Send(query);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        //  DEACTIVATE CATEGORY
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteCategoryCommand(id);

            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}