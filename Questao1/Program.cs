using System;
using System.Globalization;
using Questao1.Models;

namespace Questao1
{
    class Program
    {
        static void Main(string[] args)
        {

            bool repetirPrograma;
            do
            {
                repetirPrograma = false;

                try
                {
                    ContaBancaria conta = null;

                    Console.Write("Entre o número da conta: ");
                    int numero = int.Parse(Console.ReadLine());
                    Console.Write("Entre o titular da conta: ");
                    string titular = Console.ReadLine();
                    Console.Write("Haverá depósito inicial (s/n)? ");
                    _ = char.TryParse(Console.ReadLine(), out char respVal);

                    if (respVal == 's' || respVal == 'S')
                    {
                        Console.Write("Entre com o valor de depósito inicial: ");
                        if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal valorDepositoInicial))
                            throw new ArgumentException("Valor depósito inválido");

                        conta = new ContaBancaria(numero, titular, valorDepositoInicial);
                    }
                    else
                    {
                        conta = new ContaBancaria(numero, titular);
                    }

                    Console.WriteLine();
                    Console.WriteLine("Dados da conta:");
                    Console.WriteLine(conta);

                    Console.WriteLine();
                    Console.Write("Entre um valor para depósito: ");
                    if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal depositoVal))
                        throw new ArgumentException("Valor depósito inválido");

                    conta.Deposito(depositoVal);
                    Console.WriteLine("Dados da conta atualizados:");
                    Console.WriteLine(conta);

                    Console.WriteLine();
                    Console.Write("Entre um valor para saque: ");
                    if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal saqueVal))
                        throw new ArgumentException("Valor saque inválido");

                    conta.Saque(saqueVal);

                    Console.WriteLine("Dados da conta atualizados:");
                    Console.WriteLine(conta);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro: {e.Message}");

                    Console.WriteLine("Deseja tentar novamente? (s/n)");
                    char.TryParse(Console.ReadLine(), out char tentarNovamente);
                    repetirPrograma = tentarNovamente == 's' || tentarNovamente == 'S';
                }
            } while (repetirPrograma);

            /* Output expected:
            Exemplo 1:

            Entre o número da conta: 5447
            Entre o titular da conta: Milton Gonçalves
            Haverá depósito inicial(s / n) ? s
            Entre o valor de depósito inicial: 350.00

            Dados da conta:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 350.00

            Entre um valor para depósito: 200
            Dados da conta atualizados:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 550.00

            Entre um valor para saque: 199
            Dados da conta atualizados:
            Conta 5447, Titular: Milton Gonçalves, Saldo: $ 347.50

            Exemplo 2:
            Entre o número da conta: 5139
            Entre o titular da conta: Elza Soares
            Haverá depósito inicial(s / n) ? n

            Dados da conta:
            Conta 5139, Titular: Elza Soares, Saldo: $ 0.00

            Entre um valor para depósito: 300.00
            Dados da conta atualizados:
            Conta 5139, Titular: Elza Soares, Saldo: $ 300.00

            Entre um valor para saque: 298.00
            Dados da conta atualizados:
            Conta 5139, Titular: Elza Soares, Saldo: $ -1.50
            */
        }
    }
}
