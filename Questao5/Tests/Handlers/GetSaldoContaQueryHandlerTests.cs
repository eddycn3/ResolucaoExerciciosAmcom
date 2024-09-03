using NSubstitute;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Validations;
using Questao5.Infrastructure.Database.Repositories.Interfaces;
using Xunit;

namespace Questao5.Tests.Handlers
{
    public class GetSaldoContaQueryHandlerTests
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly GetSaldoContaQueryHandler _handler;

        public GetSaldoContaQueryHandlerTests()
        {
            _contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
            _movimentoRepository = Substitute.For<IMovimentoRepository>();
            _handler = new GetSaldoContaQueryHandler(_contaCorrenteRepository, _movimentoRepository);
        }

        [Fact]
        public async Task Handle_DeveRetornarSaldo_QuandoContaEhValida()
        {
            // Arrange
            var contaCorrenteId = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
            var query = new GetSaldoContaQuery(contaCorrenteId);
            var contaCorrente = new ContaCorrente(123, "Eduardo C. Neto", true);

            _contaCorrenteRepository.GetByIdAsync(contaCorrenteId).Returns(Task.FromResult(contaCorrente));
            _movimentoRepository.GetSumByTipoAsync(contaCorrenteId, TipoMovimento.CREDITO).Returns(Task.FromResult(1000m));
            _movimentoRepository.GetSumByTipoAsync(contaCorrenteId, TipoMovimento.DEBITO).Returns(Task.FromResult(500m));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(123, result.Numero);
            Assert.Equal("Eduardo C. Neto", result.Nome);
            Assert.Equal(500m, result.Saldo);
        }

        [Fact]
        public async Task Handle_DeveLancarException_QuandoContaEstaInativa()
        {
            // Arrange
            var contaCorrenteId = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
            var query = new GetSaldoContaQuery(contaCorrenteId);
            var contaCorrente = new ContaCorrente(123, "Eduardo C. Neto", false);

            _contaCorrenteRepository.GetByIdAsync(contaCorrenteId).Returns(Task.FromResult(contaCorrente));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Equal("Conta corrente inativa.", exception.Mensagem);
        }

        [Fact]
        public async Task Handle_DeveRetornarSaldoZero_QuandoNaoExistirMovimentacoes()
        {
            // Arrange
            var contaCorrenteId = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
            var query = new GetSaldoContaQuery(contaCorrenteId);
            var contaCorrente = new ContaCorrente(123, "Eduardo C. Neto", true);

            _contaCorrenteRepository.GetByIdAsync(contaCorrenteId).Returns(Task.FromResult(contaCorrente));
            _movimentoRepository.GetSumByTipoAsync(contaCorrenteId, TipoMovimento.CREDITO).Returns(Task.FromResult(0m));
            _movimentoRepository.GetSumByTipoAsync(contaCorrenteId, TipoMovimento.DEBITO).Returns(Task.FromResult(0m));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(123, result.Numero);
            Assert.Equal("Eduardo C. Neto", result.Nome);
            Assert.Equal(0m, result.Saldo);
        }
    }

}
