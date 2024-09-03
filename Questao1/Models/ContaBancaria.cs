using Questao1.Validations;
using System.Globalization;

namespace Questao1.Models
{
    public class ContaBancaria
    {
        private readonly decimal TaxaSaqueInstituicao = 3.5M;

        public int NumeroConta { get; private set; }
        public string NomeTitular { get; private set; }
        public decimal ValorDepositoInicial { get; private set; }
        public decimal SaldoContaCorrente { get; private set; }

        public ContaBancaria(int numeroConta, string titular)
        {
            ValidarNumeroConta(numeroConta);

            NumeroConta = numeroConta;
            NomeTitular = titular;
        }

        public ContaBancaria(int numero, string titular, decimal depositoInicial)
        {
            ValidarNumeroConta(numero);

            NumeroConta = numero;
            NomeTitular = titular;
            Deposito(depositoInicial);
        }

        public void AlterarNomeTitular(string nome)
        {
            NomeTitular = nome;
        }

        public void Deposito(decimal quantiaDeposito)
        {
            DomainExceptionValidation
               .When(quantiaDeposito < 0, "A quantia de depósito não pode ser negativa.\n");

            SaldoContaCorrente += quantiaDeposito;
        }

        public void Saque(decimal quantia)
        {
            SaldoContaCorrente -= (quantia + TaxaSaqueInstituicao);
        }

        private void ValidarNumeroConta(int numeroConta)
        {
            DomainExceptionValidation
                .When(numeroConta < 0, "Número da conta inválido!\n");
        }

        public override string ToString()
        {
            return $"Conta {NumeroConta}, Titular: {NomeTitular}, Saldo:{SaldoContaCorrente.ToString("C", new CultureInfo("pt-BR"))}";
        }
    }
}
