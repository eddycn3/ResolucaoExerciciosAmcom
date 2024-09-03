using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Questao2.Dtos;
using Questao2.Services;
using Questao2.Services.Interfaces;
using System.Globalization;

public class Program
{
    public async static Task Main()
    {
        Console.Write("-- Questao 2 - Quantidade de Gols Por Time em um Ano --" + Environment.NewLine);

        var services = new ServiceCollection();
        ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();

        var service = serviceProvider.GetService<IServiceInfosFootballMatches>();

        bool repetirPrograma;
        do
        {
            try
            {

                Console.Write("Entre com o Ano:");
                if (int.TryParse(Console.ReadLine(), out int ano) && ano < 1863)
                    throw new ArgumentException("Ano inválido");

                Console.Write("Entre com o Nome do time:");
                string time = Console.ReadLine();

                var matches = await service.GetFootballMatches(ano, time);

                int totalGoals = GetTotalScoredGoals(matches);

                Console.WriteLine("Team " + time + " scored " + totalGoals.ToString() + " goals in " + ano + Environment.NewLine);

                Console.WriteLine("Deseja pesquisar outro time? (s/n)");
                _ = char.TryParse(Console.ReadLine(), out char tentarNovamente);
                repetirPrograma = tentarNovamente == 's' || tentarNovamente == 'S';

                Console.WriteLine();

            }
            catch (Exception e)
            {
                Console.WriteLine($"Ocorreu uma falha: {e.Message}");
                Console.WriteLine("Deseja tentar novamente? (s/n)");
                _ = char.TryParse(Console.ReadLine(), out char tentarNovamente);
                repetirPrograma = tentarNovamente == 's' || tentarNovamente == 'S';
            }
        } while (repetirPrograma);



        /*
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals =  getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals =  getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);
        */

    }

    private static int GetTotalScoredGoals(IEnumerable<FootballMatchDTO> matches)
    {
        int totalGoals = 0;
        foreach (FootballMatchDTO match in matches)
        {
            if (int.TryParse(match.Team1Goals, out int teamGoal))
                totalGoals += teamGoal;
        }

        return totalGoals;
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient<IServiceInfosFootballMatches, ServiceInfosFootballMatches>();
    }

}