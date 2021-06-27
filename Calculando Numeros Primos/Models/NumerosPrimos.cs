using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculando_Numeros_Primos.Models
{
    public class NumerosPrimos
    {
        public NumerosPrimos()
        {
            numeros = new Collection<NumerosPrimos>();


        }
        
        public long Numero { get; set; }
        public int QuantidadeDivisores { get; set; }
        public bool EPrimo { get; set; }
        public TimeSpan tempoCalculo { get; set; }

        ICollection<NumerosPrimos> numeros { get; set;}

    }
}
