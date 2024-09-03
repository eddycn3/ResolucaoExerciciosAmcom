using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Questao5.Infrastructure.Services.Controllers.Base
{
    [ApiController]
    public abstract class BaseController : Controller
    {
        protected readonly IMediator _commandHandler;
        public BaseController(IMediator commandHandler) => _commandHandler = commandHandler;
    }
}
