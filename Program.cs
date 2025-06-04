using System.Globalization;
using CsvHelper;

class Jogador
{
    public string Nome { get; set; } = null!;
    public int Pontuacao { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        string caminho = "tabuada_scores.csv";
        List<Jogador> jogadores = new List<Jogador>();

        // Carrega pontuações anteriores
        if (File.Exists(caminho))
        {
            using (var leitor = new StreamReader(caminho))
            using (var csv = new CsvReader(leitor, CultureInfo.InvariantCulture))
            {
                jogadores = csv.GetRecords<Jogador>().ToList();
            }
        }

        Console.Write("Digite seu nome: ");
        string nome = Console.ReadLine()!;

        Console.WriteLine("\n DESAFIO DE TABUADA - 5 PERGUNTAS");
        Console.WriteLine("Responda corretamente para ganhar pontos!\n");

        Random rand = new Random();
        int pontuacao = 0;

        for (int i = 0; i <= 5; i++)
        {
            int a = rand.Next(2, 11);
            int b = rand.Next(2, 11);
            int respostaCerta = a * b;

            Console.Write($"[{i}/5] Quanto é {a} x {b}? ");

            string entrada = Console.ReadLine()!;
            if (int.TryParse(entrada, out int respotaJogador))
            {
                if (respotaJogador == respostaCerta)
                {
                    Console.WriteLine("Correto!\n");
                    pontuacao += 20;
                }
                else
                {
                    Console.WriteLine($"Errado! A resposta certa era {respostaCerta}.\n");
                }
            }
            else
            {
                Console.WriteLine("Entrada inválida. Nenhum ponto ganho. \n");
            }
        }

        Console.WriteLine($"Fim de jogo! você fez {pontuacao} pontos.");

        var jogadorExistente = jogadores.FirstOrDefault(j => j.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

        if (jogadorExistente != null)
        {
            if (pontuacao > jogadorExistente.Pontuacao)
            {
                jogadorExistente.Pontuacao = pontuacao;
                Console.WriteLine("Você bateu sua pontuação anterior!");
            }
            else
            {
                Console.WriteLine($"Sua pontuação anterior ({jogadorExistente.Pontuacao}) foi melhor ou igual.");
            }
        }
        else
        {
            jogadores.Add(new Jogador { Nome = nome, Pontuacao = pontuacao });
            Console.WriteLine("Novo jogador registrado!");
        }

        // Salvar placar
        using (var escritor = new StreamWriter(caminho))
        using (var csv = new CsvWriter(escritor, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(jogadores);
        }

        Console.WriteLine("Pontuação salva em 'tabuada_scores.csv'");
    }
}
