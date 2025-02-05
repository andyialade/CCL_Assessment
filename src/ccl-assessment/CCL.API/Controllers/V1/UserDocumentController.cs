using CCL.Application.UserDocuments.Queries.GetAllDocuments;
using CCL.Application.UserDocuments.Queries.GetDocumentById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCL.API.Controllers.V1
{
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class UserDocumentController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var userDocument = await mediator.Send(new GetDocumentByIdQuery(id));
            return Ok(userDocument);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] GetAllDocumentsQuery query)
        {
            var userDocuments = await mediator.Send(query);
            return Ok(userDocuments);
        }
    }
}
