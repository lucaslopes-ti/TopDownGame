using System;
using System.Collections.Generic;

namespace BFSDemo
{
    class Program
    {
        // Matriz de adjacência - representa conexões entre cidades
        // Cada valor é a distância em km, -1 significa sem conexão
        static int[,] grafo = new int[5, 5]
        {
            { 0, 40, 10, 20, 45},  // Uberlândia
            {40,  0, 40, 30, 10},  // Uberaba
            {10, 40,  0,  8, -1},  // Araguari
            {20, 30,  8,  0, 35},  // Ituiutaba
            {45, 10, -1, 35,  0}   // Patos de Minas
        };

        // Nomes das cidades
        static string[] cidades = { "Uberlândia", "Uberaba", "Araguari", "Ituiutaba", "Patos de Minas" };

        static void Main(string[] args)
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║        BFS - BUSCA EM LARGURA (Breadth-First Search)         ║");
            Console.WriteLine("║              Demo com Matriz de Adjacência                   ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // Mostra a matriz de adjacência
            MostrarMatriz();

            Console.WriteLine("\n══════════════════════════════════════════════════════════════");
            Console.WriteLine("Testando BFS de diferentes origens:");
            Console.WriteLine("══════════════════════════════════════════════════════════════\n");

            // Teste 1: Uberlândia → Patos de Minas
            Console.WriteLine("🔍 Busca 1: Uberlândia → Patos de Minas");
            BFS(0, 4);
            Console.WriteLine();

            // Teste 2: Araguari → Uberaba
            Console.WriteLine("🔍 Busca 2: Araguari → Uberaba");
            BFS(2, 1);
            Console.WriteLine();

            // Teste 3: Ituiutaba → Uberlândia
            Console.WriteLine("🔍 Busca 3: Ituiutaba → Uberlândia");
            BFS(3, 0);
            Console.WriteLine();

            Console.WriteLine("══════════════════════════════════════════════════════════════");
            Console.WriteLine("Demonstração de BFS passo a passo:");
            Console.WriteLine("══════════════════════════════════════════════════════════════\n");
            
            BFSPassoAPasso(0, 4);

            Console.WriteLine("\n══════════════════════════════════════════════════════════════");
            Console.WriteLine("                      FIM DA DEMONSTRAÇÃO                      ");
            Console.WriteLine("══════════════════════════════════════════════════════════════");
        }

        static void MostrarMatriz()
        {
            Console.WriteLine("Matriz de Adjacência (distâncias em km):");
            Console.WriteLine("(-1 = sem conexão direta)\n");

            // Cabeçalho
            Console.Write("                 ");
            for (int i = 0; i < 5; i++)
            {
                Console.Write($"{cidades[i].Substring(0, Math.Min(8, cidades[i].Length)),8} ");
            }
            Console.WriteLine();
            Console.WriteLine("              +" + new string('-', 45) + "+");

            // Linhas da matriz
            for (int i = 0; i < 5; i++)
            {
                Console.Write($"{cidades[i],-14} | ");
                for (int j = 0; j < 5; j++)
                {
                    if (grafo[i, j] == -1)
                        Console.Write($"{"---",8} ");
                    else if (grafo[i, j] == 0)
                        Console.Write($"{"   -",8} ");
                    else
                        Console.Write($"{grafo[i, j],8} ");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("              +" + new string('-', 45) + "+");
        }

        static void BFS(int origem, int destino)
        {
            // Array para marcar nós visitados
            int[] visitados = new int[5] { 0, 0, 0, 0, 0 };
            
            // Array para guardar predecessores (de onde viemos)
            int[] predecessores = new int[5] { -1, -1, -1, -1, -1 };
            
            // Fila para BFS
            Queue<int> fila = new Queue<int>();
            
            // String para mostrar ordem de visita
            string ordemVisita = "";

            // Inicializa: coloca origem na fila
            fila.Enqueue(origem);
            visitados[origem] = 1;

            while (fila.Count > 0)
            {
                int atual = fila.Dequeue();
                ordemVisita += cidades[atual] + " → ";

                // Chegou no destino?
                if (atual == destino)
                {
                    break;
                }

                // Para cada possível vizinho
                for (int i = 0; i < 5; i++)
                {
                    // Se existe conexão (valor > 0) E não foi visitado
                    if (grafo[atual, i] > 0 && visitados[i] == 0)
                    {
                        fila.Enqueue(i);
                        visitados[i] = 1;
                        predecessores[i] = atual;
                    }
                }
            }

            // Mostra ordem de visita
            ordemVisita = ordemVisita.TrimEnd(' ', '→', ' ');
            Console.WriteLine($"   Ordem de exploração: {ordemVisita}");
            
            // Reconstrói e mostra o caminho
            MostrarCaminho(predecessores, origem, destino);
        }

        static void MostrarCaminho(int[] predecessores, int origem, int destino)
        {
            List<string> caminho = new List<string>();
            int atual = destino;

            while (atual != -1)
            {
                caminho.Insert(0, cidades[atual]);
                atual = predecessores[atual];
            }

            Console.WriteLine($"   Caminho encontrado:  {string.Join(" → ", caminho)}");
            Console.WriteLine($"   Número de saltos:    {caminho.Count - 1}");
        }

        static void BFSPassoAPasso(int origem, int destino)
        {
            Console.WriteLine($"Buscando caminho de {cidades[origem]} até {cidades[destino]}...\n");

            int[] visitados = new int[5] { 0, 0, 0, 0, 0 };
            int[] distancias = new int[5] { -1, -1, -1, -1, -1 };
            int[] predecessores = new int[5] { -1, -1, -1, -1, -1 };
            Queue<int> fila = new Queue<int>();

            // Passo 0: Inicialização
            Console.WriteLine("PASSO 0 - Inicialização:");
            fila.Enqueue(origem);
            visitados[origem] = 1;
            distancias[origem] = 0;
            MostrarEstado(fila, visitados, distancias);

            int passo = 1;
            while (fila.Count > 0)
            {
                int atual = fila.Dequeue();
                
                Console.WriteLine($"\nPASSO {passo} - Processando: {cidades[atual]}");
                Console.WriteLine($"   Vizinhos de {cidades[atual]}:");

                for (int i = 0; i < 5; i++)
                {
                    if (grafo[atual, i] > 0)
                    {
                        if (visitados[i] == 0)
                        {
                            Console.WriteLine($"      ✅ {cidades[i]} - NOVO! Adicionando à fila");
                            fila.Enqueue(i);
                            visitados[i] = 1;
                            distancias[i] = distancias[atual] + 1;
                            predecessores[i] = atual;
                        }
                        else
                        {
                            Console.WriteLine($"      ⏭️  {cidades[i]} - já visitado, pulando");
                        }
                    }
                }

                MostrarEstado(fila, visitados, distancias);

                if (visitados[destino] == 1)
                {
                    Console.WriteLine($"\n🎯 DESTINO ENCONTRADO: {cidades[destino]}!");
                    break;
                }

                passo++;
            }

            // Mostra resultado final
            Console.WriteLine("\n════════════════════════════════════════");
            Console.WriteLine("RESULTADO FINAL:");
            Console.WriteLine("════════════════════════════════════════");
            
            Console.WriteLine("\nDistâncias (número de saltos) a partir de " + cidades[origem] + ":");
            for (int i = 0; i < 5; i++)
            {
                string distancia = distancias[i] >= 0 ? distancias[i].ToString() : "∞";
                Console.WriteLine($"   {cidades[i],-15}: {distancia} saltos");
            }

            Console.WriteLine("\n🛤️  Caminho reconstruído:");
            MostrarCaminho(predecessores, origem, destino);
        }

        static void MostrarEstado(Queue<int> fila, int[] visitados, int[] distancias)
        {
            Console.Write("   Fila atual: [");
            Queue<int> filaTemp = new Queue<int>(fila);
            List<string> itens = new List<string>();
            while (filaTemp.Count > 0)
            {
                itens.Add(cidades[filaTemp.Dequeue()]);
            }
            Console.WriteLine(string.Join(", ", itens) + "]");

            Console.Write("   Visitados:  [");
            List<string> vis = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                if (visitados[i] == 1)
                    vis.Add(cidades[i]);
            }
            Console.WriteLine(string.Join(", ", vis) + "]");
        }
    }
}
