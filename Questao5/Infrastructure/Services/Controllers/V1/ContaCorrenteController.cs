using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Configuration;
using Questao5.Domain.Validations;
using Questao5.Infrastructure.Services.Controllers.Base;

namespace Questao5.Infrastructure.Services.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/contacorrente")]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public class ContaCorrenteController : BaseController
    {
        public ContaCorrenteController(IMediator commandHandler) : base(commandHandler) { }

        /// <summary>
        /// Realiza a movimentação de valor de uma conta corrente
        /// </summary>
        /// <param name="command">Objeto de movimentação.</param>
        /// <returns>Identificador de Movimentação.</returns>
        /// <remarks>
        /// Sample request:
        ///
        /// ```
        /// POST /api/v1/contacorrente
        /// {
        ///     "requestId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///     "contaCorrenteId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///     "valor": 2450.00,
        ///     "tipoMovimento": "C"
        /// }
        /// ```
        /// </remarks>
        /// <response code="200">Identificador de Movimentação.</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(MovimentarContaResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> MovimentarContaCorrente(MovimentarContaCommand command)
        {
            var result = await _commandHandler.Send(command);
            return Ok(result);
        }

        [HttpGet("saldo")]
        [ProducesResponseType(typeof(SaldoContaResponse), 200)]
        public async Task<IActionResult> GetSaldo([FromQuery] string id)
        {
            var response = await _commandHandler.Send(new GetSaldoContaQuery(id));
            return Ok(response);
        }
    }
}
