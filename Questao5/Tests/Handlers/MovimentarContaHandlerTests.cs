using Newtonsoft.Json;
using NSubstitute;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Extensions;
using Questao5.Domain.Validations;
using Questao5.Infrastructure.Database.Repositories.Interfaces;
using Xunit;

namespace Questao5.Tests.Handlers
{
    public class MovimentarContaHandlerTests
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;
        private readonly MovimentarContaHandler _handler;

        public MovimentarContaHandlerTests()
        {
            _contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
            _movimentoRepository = Substitute.For<IMovimentoRepository>();
            _idempotenciaRepository = Substitute.For<IIdempotenciaRepository>();
            _handler = new MovimentarContaHandler(_contaCorrenteRepository, _movimentoRepository, _idempotenciaRepository);
        }

        [Fact]
        public async Task Handle_DeveRetornarResultadoExistente_SeRequisicaoEhIdempotente()
        {
            // Arrange
            var request = new MovimentarContaCommand
            {
                RequestId = Guid.NewGuid(),
                ContaCorrenteId = Guid.NewGuid(),
                Valor = 100m,
                TipoMovimento = TipoMovimento.CREDITO.ToCode(),
            };
            var idempotencia = new Idempotencia(request.RequestId, JsonConvert.SerializeObject(request), "997e2776-b8ec-4d65-9f0c-3a2becc4558d");

            _idempotenciaRepository.GetByChaveAsync(request.RequestId).Returns(Task.FromResult(idempotencia));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("997e2776-b8ec-4d65-9f0c-3a2becc4558d", result.IdMovimentacaoGerada);
        }

        [Fact]
        public async Task Handle_DeveLancarException_SeContaEhInvalidaOuInativa()
        {
            // Arrange
            var request = new MovimentarContaCommand
            {
                RequestId = Guid.NewGuid(),
                ContaCorrenteId = Guid.NewGuid(),
                Valor = 100m,
                TipoMovimento = TipoMovimento.CREDITO.ToCode()
            };

            _contaCorrenteRepository.GetByIdAsync(request.ContaCorrenteId.ToString()).Returns(Task.FromResult<ContaCorrente>(null));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Conta corrente inválida ou inativa", exception.Mensagem);
        }

        [Fact]
        public async Task Handle_DeveLancarException_SeValorEhInvalido()
        {
            // Arrange
            var request = new MovimentarContaCommand
            {
                RequestId = Guid.NewGuid(),
                ContaCorrenteId = Guid.NewGuid(),
                Valor = -10m,
                TipoMovimento = TipoMovimento.CREDITO.ToCode()
            };
            var contaCorrente = new ContaCorrente(123, "John Doe", true);

            _contaCorrenteRepository.GetByIdAsync(request.ContaCorrenteId.ToString()).Returns(Task.FromResult(contaCorrente));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Valor inválido", exception.Mensagem);
        }

        [Fact]
        public async Task Handle_DeveLancarException_SeTipoMovimentacaoEhInvalido()
        {
            // Arrange
            var request = new MovimentarContaCommand
            {
                RequestId = Guid.NewGuid(),
                ContaCorrenteId = Guid.NewGuid(),
                Valor = 100m,
                TipoMovimento = "OAISNDSAO" // Invalid TipoMovimento
            };
            var contaCorrente = new ContaCorrente(123, "John Doe", true);

            _contaCorrenteRepository.GetByIdAsync(request.ContaCorrenteId.ToString()).Returns(Task.FromResult(contaCorrente));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Tipo movimento inválido.", exception.Mensagem);
        }

        [Fact]
        public async Task Handle_DeveAdicionarMovimentacaoEIdempotencia_QuandoValido()
        {
            // Arrange
            var request = new MovimentarContaCommand
            {
                RequestId = Guid.NewGuid(),
                ContaCorrenteId = Guid.NewGuid(),
                Valor = 100m,
                TipoMovimento = TipoMovimento.CREDITO.ToCode()
            };
            var contaCorrente = new ContaCorrente(123, "John Doe", true);
            var movimentoId = Guid.NewGuid().ToString();

            _contaCorrenteRepository.GetByIdAsync(request.ContaCorrenteId.ToString()).Returns(Task.FromResult(contaCorrente));
            _movimentoRepository.AddAsync(Arg.Any<Movimento>()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            await _movimentoRepository.Received(1).AddAsync(Arg.Is<Movimento>(m => m.Valor == request.Valor));
            await _idempotenciaRepository.Received(1).AddAsync(Arg.Is<Idempotencia>(i => i.ChaveIdempotencia == request.RequestId));
            Assert.NotNull(result);
            Assert.NotEmpty(result.IdMovimentacaoGerada);
        }

    }

}
