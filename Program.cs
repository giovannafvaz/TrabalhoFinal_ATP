using System;
using System.IO;
class Program
{
    static string[] Produtos; //armazena o nome dos produtos da loja
    static int[] QuantidadeEstoque; //armazena a quantidade de cada produto em estoque
    static int[,] VendasDia; //armazena a quantidade diária de vendas durante um mês
    static int MesAtual; // Variável para armazenar o número do mês atual

    static void Main()
    {
        bool rodar = true; //Enquanto "rodar" for verdadeiro, o programa continurá rodando. 

        while (rodar)
        {
            Console.WriteLine("MENU PRINCIPAL");
            Console.WriteLine("1 – Importar arquivo de produtos");
            Console.WriteLine("2 – Registrar venda");
            Console.WriteLine("3 – Relatório de vendas");
            Console.WriteLine("4 – Relatório de estoque");
            Console.WriteLine("5 – Criar arquivo de vendas");
            Console.WriteLine("6 - Sair");

            Console.Write("Escolha uma opção: ");
            int opcao = int.Parse(Console.ReadLine());
            Console.WriteLine();

            switch (opcao)
            {
                case 1:
                    ImportarProdutos();
                    break;
                case 2:
                    RegistrarVendas();
                    break;
                case 3:
                    RelatorioVendas();
                    break;
                case 4:
                    RelatorioEstoque();
                    break;
                case 5:
                    GerarVendas();
                    break;
                case 6:
                    rodar = false;
                    //se o usuário digitar 6 (sair), a variável bool rodar será falsa, logo o sistema irá parar de rodar
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void ImportarProdutos()
    {
        //Usuário informa o caminho do argquivo program.txt
        Console.Write("Digite o caminho do arquivo de produtos (escreva 'produtos.txt'): ");
        string caminhoArquivo = Console.ReadLine();
        try
        {
            //ReadAllLines: lê cada linha do arquivo fornecido para armazená-lo em um array de string
            string[] linhas = File.ReadAllLines(caminhoArquivo);
            int numProdutos = linhas.Length / 2;
            /*Cada produto ocupa 2 linhas no array (nome do produto e quantidade em estoque), por isso, 
            e divide por 2 para pegar metade (produtos)*/
            Produtos = new string[numProdutos];// será armazenado os nomes dos produtos

            QuantidadeEstoque = new int[numProdutos];// será armazenado a quantidade de produtos

            for (int i = 0; i < numProdutos; i++)
            {
                Produtos[i] = linhas[i * 2];
                QuantidadeEstoque[i] = int.Parse(linhas[i * 2 + 1]);
            }

            // utilizado o for para percorrer as linhas do arquivo importado e armazenar nas variáveis correspondentes

            VendasDia = new int[numProdutos, 30]; // Matriz com o número de produtos e 30 dias (considerando um mês de 30 dias)
            MesAtual = 0;
            Console.WriteLine("Arquivo de produtos importado com sucesso.");

        }
        catch (Exception ex) // tratamento de exceção
        {
            Console.WriteLine("Erro ao importar arquivo de produtos: " + ex.Message);
        }
    }

    static void RegistrarVendas()
    {
        //retorna a menssagem de que não há produtos importados se o array for nulo
        if (Produtos == null)
        {
            Console.WriteLine("Não há produtos importados. Importe um arquivo de produtos antes de registrar vendas.");
            return;
        }

        //solicita ao usuário o número do produto, o dia do mês e a quantidade vendida
        Console.Write("Digite o número do produto (1 a {0}): ", Produtos.Length);

        int numProduto;
        //Verifica se o número do produto digitado é válido
        if (!int.TryParse(Console.ReadLine(), out numProduto) || numProduto < 1 || numProduto > Produtos.Length)
        {
            //Se não for válido, aparece a mensagem abaixo
            Console.WriteLine("Número do produto inválido.");
            return;
        }

        //Verifica se o dia inserido é válido
        Console.Write("Agora, insira o dia de venda desejado: ");
        int dia;
        if (!int.TryParse(Console.ReadLine(), out dia) || dia < 1 || dia > 30)
        {
            //Se não for válido, aparece a mensagem abaixo
            Console.WriteLine("Dia inválido.");
            return;
        }

        Console.Write("Por fim, digite a quantidade de produtos vendidos no dia selecionado: ");
        int quantidade;
        //Verifica se a quantidade vendida é maior do que a quantidade em estoque para o produto selecionado.
        if (!int.TryParse(Console.ReadLine(), out quantidade) || quantidade <= 0)
        {
            //Se não for válida, aparece a mensagem abaixo
            Console.WriteLine("Quantidade inválida.");
            return;
        }

        //Localiza a posição do produto selecionado dentro do array
        int indiceProduto = numProduto - 1;

        //Compara a quantidade vendida com a quantidade em estoque do produto selecionado.
        if (quantidade > QuantidadeEstoque[indiceProduto])
        {
            Console.WriteLine("Quantidade em estoque insuficiente.");
            return;
        }

        //Incrementa o número de vendas do dia à lista do produto selecionado
        VendasDia[indiceProduto, dia - 1] += quantidade;

        //Subtrai os produtos vendidos da lista de estoque
        QuantidadeEstoque[indiceProduto] -= quantidade;

        Console.WriteLine("Venda registrada com sucesso.");
    }

    static void RelatorioVendas()
    {
        if (Produtos == null)
        {
            Console.WriteLine("Não há produtos importados. Importe um arquivo de produtos antes de gerar o relatório de vendas.");
            return;
        }

        Console.WriteLine("Relatório de vendas:");

        //for é usado para exibir os números de dia de 1 a 30.
        Console.Write("{0, -12}", "Produto");

        for (int dia = 1; dia <= 30; dia++)
        {
            Console.Write("{0, 4}", dia);
        }
        Console.WriteLine();

        for (int i = 0; i < Produtos.Length; i++)
        {
            //exibe o nome do produto
            Console.Write("{0, -12}", Produtos[i]);

            for (int dia = 1; dia <= 30; dia++)
            {
                //exibe a quantidade vendida do produto no dia correspondente
                Console.Write("{0, 4}", VendasDia[i, dia - 1]);
            }

            Console.WriteLine();
        }
    }

    static void RelatorioEstoque()
    {
        if (Produtos == null)
        {
            Console.WriteLine("Não há produtos importados. Importe um arquivo de produtos antes de gerar o relatório de estoque.");
            return;
        }
        //O loop percorre cada produto no array produtos. Para cada produto,
        //a linha Console.WriteLine("{0}: {1}", produtos[i], estoque[i]); exibe o nome do produto seguido pelo valor do estoque correspondente.

        Console.WriteLine("Relatório de estoque:");
        for (int i = 0; i < Produtos.Length; i++)
        {
            Console.WriteLine("{0}: {1}", Produtos[i], QuantidadeEstoque[i]);
        }
    }

    static void GerarVendas()
    {
        if (Produtos == null)
        {
            Console.WriteLine("Não há produtos importados. Importe um arquivo de produtos antes de criar o arquivo de vendas.");
            return;
        }

        //lê a entrada do usuário e armazena o caminho do arquivo em uma variável.
        try
        {
            //criar o arquivo de vendas no caminho especificado.
            using (StreamWriter sw = new StreamWriter("vendas.txt"))
            {
                for (int i = 0; i < Produtos.Length; i++)
                {
                    sw.WriteLine(Produtos[i]);

                    for (int dia = 1; dia <= 30; dia++)
                    {
                        sw.WriteLine("Dia {0}: {1}", dia, VendasDia[i, dia - 1]);
                        //escreve no arquivo o número do dia seguido pela quantidade vendida do produto no dia correspondente.
                    }
                    sw.WriteLine();
                }
                Console.WriteLine("Arquivo de vendas criado com sucesso!");
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine("Erro ao criar arquivo de vendas: " + ex.Message);
        }
    }
}