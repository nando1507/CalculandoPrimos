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

                    for (long i = ini; i <= fim; i++)
                    {
                        NumerosPrimos numeros = new NumerosPrimos();
                        DateTime DtInicio = DateTime.Now;
                        numeros.Numero = i;
                        numeros.QuantidadeDivisores = await Divisores(i, primos);
                        numeros.EPrimo = numeros.QuantidadeDivisores > 2 ? false : true;
                        DateTime DtFim = DateTime.Now;
                        numeros.tempoCalculo = DtFim.Subtract(DtInicio);

                        primos.Add(numeros);
                    }



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
                var arrayPrimos = primos.Where(w => w.EPrimo).Select(s => s.Numero).OrderBy(o => o).ToList();
                arrayPrimos.Add(num);
                arrayPrimos.Add(1);

                foreach (var item in arrayPrimos.OrderBy(o => o))
                {
                    if(num % 2 == 0)
                    {
                        divisores += 3;
                        break;
                    }
                    if (num % item == 0)
                    {
                        
                        divisores++;
                    }
                    if(divisores >= 3)
                    {
                        break;
                    }
                }
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
            Console.WriteLine($"Exitem {Imprime.Where(w => w.EPrimo).Count()} numeros primos entre {ini} e {max}");

            var quantidade = Imprime.Where(w => w.EPrimo).OrderBy(o => o.Numero);
            int indice = 0;
            //monta matriz
            Dictionary<int, string> matriz = new Dictionary<int, string>();
            string[] aux = new string[10]; ;

            int i = 0;
            foreach (var item in quantidade)
            {
                aux[i] = item.Numero.ToString();
                i++;

                if (i == aux.Length)
                {
                    string insert = string.Empty;

                    for (int X = 0; X < aux.Length; X++)
                    {
                        insert = (X != 0 ? insert + " | " : "") + aux[X].Trim().PadLeft(2, '0');

                    }
                    matriz.Add(indice, insert);
                    i = 0;
                    indice++;
                }
            }
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



    }
}
