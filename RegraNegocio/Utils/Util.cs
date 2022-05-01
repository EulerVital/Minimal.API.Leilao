using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegraNegocio.Utils
{
    public class Util
    {
        public static string RetornaNomeStringAleatorio(string valueInicio = "", int qtdCaracteres = 8)
        {
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var Charsarr = new char[qtdCaracteres];
            var random = new Random();

            for (int i = 0; i < Charsarr.Length; i++)
            {
                Charsarr[i] = characters[random.Next(characters.Length)];
            }

            return valueInicio + new string(Charsarr);
        }
            
    }
}
