using Calculando_Numeros_Primos.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Calculando_Numeros_Primos
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DateTime dtinicio = DateTime.Now;


            Console.WriteLine("Informe o Valor maximo a ser analisado");
            var linha = Console.ReadLine();
            try
            {
                var valor = !string.IsNullOrEmpty(linha) ? Convert.ToUInt64(linha) : long.MaxValue;
                long min = 5;
                long max = (long)valor;
                List<NumerosPrimos> retorno = await CalculaPrimo(min, max);
                ImprimePrimos(retorno, min, max);
            }
            catch (FormatException ex)
            {

                Console.WriteLine("Formato invalido: " + ex.Message);
            }

            DateTime dtFim = DateTime.Now;
            TimeSpan tempo_execucao = dtFim.Subtract(dtinicio);
            Console.WriteLine($"Iniciado em {dtinicio}\nfinalizado em: {dtFim} \nExecutado em {tempo_execucao}");

            Console.ReadKey();
        }

        static async Task<List<NumerosPrimos>> CalculaPrimo(long ini, long fim)
        {
            var tasks =
                Task.Factory.StartNew(async () =>
                {
                    List<NumerosPrimos> primos = new List<NumerosPrimos>();

                    primos.Add(new NumerosPrimos { Numero = 1, QuantidadeDivisores = 1, EPrimo = false, tempoCalculo = DateTime.Now.Subtract(DateTime.Now) });
                    primos.Add(new NumerosPrimos { Numero = 3, QuantidadeDivisores = 2, EPrimo = true, tempoCalculo = DateTime.Now.Subtract(DateTime.Now) });
                    primos.Add(new NumerosPrimos { Numero = 2, QuantidadeDivisores = 2, EPrimo = true, tempoCalculo = DateTime.Now.Subtract(DateTime.Now) });

                    //for (long i = ini; i <= fim; i++)
                    //{
                    //    NumerosPrimos numeros = new NumerosPrimos();
                    //    DateTime DtInicio = DateTime.Now;
                    //    numeros.Numero = i;
                    //    numeros.QuantidadeDivisores = await Divisores(i, primos);
                    //    numeros.EPrimo = numeros.QuantidadeDivisores > 2 ? false : true;
                    //    DateTime DtFim = DateTime.Now;
                    //    numeros.tempoCalculo = DtFim.Subtract(DtInicio);

                    //    primos.Add(numeros);
                    //}

                    Parallel.For(ini, fim, new ParallelOptions { MaxDegreeOfParallelism = 4 }, async Index => {
                        NumerosPrimos numeros = new NumerosPrimos();
                        DateTime DtInicio = DateTime.Now;
                        numeros.Numero = Index;
                        numeros.QuantidadeDivisores = await Divisores(Index, primos);
                        numeros.EPrimo = numeros.QuantidadeDivisores > 2 ? false : true;
                        DateTime DtFim = DateTime.Now;
                        numeros.tempoCalculo = DtFim.Subtract(DtInicio);

                        primos.Add(numeros);
                    });


                    return primos;
                });

            return await await tasks;
        }

        
        static async Task<int> Divisores(long num, List<NumerosPrimos> primos)
        {
            int divisores = 0;
            long aux = num;
            if (aux > 3)
            {
                var arrayPrimos = primos.AsParallel().Where(w => w.EPrimo).Select(s => s.Numero).OrderBy(o => o).ToList();
                arrayPrimos.Add(num);
                arrayPrimos.Add(1);

                Parallel.ForEach(arrayPrimos.OrderBy(o => o), new ParallelOptions { MaxDegreeOfParallelism = 4}, ( item, loopState) =>
                {
                    if (num % 2 == 0)
                    {
                        divisores += 3;
                        loopState.Stop();
                    }
                    if (num % item == 0)
                    {
                        divisores++;
                    }
                    if (divisores >= 3)
                    {
                        loopState.Stop();
                        //break;
                    }
                });

                //foreach (var item in arrayPrimos.AsParallel().OrderBy(o => o))
                //{
                //    if (num % 2 == 0)
                //    {
                //        divisores += 3;
                //        break;
                //    }
                //    if (num % item == 0)
                //    {
                //        divisores++;
                //    }
                //    if (divisores >= 3)
                //    {
                //        break;
                //    }
                //}
                //for (int i = 1; i <= num; i++)
                //{

                //}
                //aux--;
            }
            //else
            //{
            //    foreach (var item in primos.Where(w => w.EPrimo).Select(s => s.Numero).OrderBy(o => o))
            //    {
            //        if (aux % item == 0)
            //        {
            //            divisores++;
            //        }
            //        if (divisores > 3)
            //        {
            //            break;
            //        }
            //    }
            //}
            return divisores;
        }

        static void ImprimePrimos(List<NumerosPrimos> Imprime, long ini, long max)
        {
            ini = 2;
            Console.WriteLine($"Exitem {Imprime.AsParallel().Where(w => w.EPrimo).Count()} numeros primos entre {ini} e {max}");

            var quantidade = Imprime.AsParallel().Where(w => w.EPrimo).OrderBy(o => o.Numero);
            int indice = 0;
            //monta matriz
            Dictionary<int, string> matriz = new Dictionary<int, string>();
            var qtde = quantidade.Count();

            int raiz = (int)Math.Sqrt(qtde) + 1;

            string[] aux = new string[raiz]; ;

            int i = 0;
            foreach (var item in quantidade)
            {
                aux[i] = item.Numero.ToString();
                i++;

                if (i == aux.Length)
                {
                    string insert = string.Empty;

                    string ta = max.ToString();
                    for (int X = 0; X < aux.Length; X++)
                    {
                        insert = (X != 0 ? insert + " | " : "") + aux[X].Trim().PadLeft(ta.Length, ' ');
                    }
                    matriz.Add(indice, insert);
                    i = 0;
                    indice++;
                }
            }

            // var m = MontaMatriz(matriz);

            StreamWriter sw = new StreamWriter(@"C:\Temp\Primos.txt");
            foreach (var item in matriz.OrderBy(o => o.Key))
            {
                sw.WriteLine(item.Value);
                Console.WriteLine(
                                item.Value
                            );
                sw.Flush();
            }
            sw.Flush();
            sw.Dispose();
            sw.Close();
        }

        public static string[][] MontaMatriz(Dictionary<int, string> matriz)
        {
            var qtde = matriz.Count();
            int raiz = (int)Math.Sqrt(qtde);

            string[][] aux = new string[raiz][];

            return aux;
        }


    }
}
