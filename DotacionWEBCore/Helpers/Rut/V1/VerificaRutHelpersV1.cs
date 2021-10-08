using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.Helpers.Rut.V1
{
    public class VerificaRutHelpersV1 : IVerificaRutHelpersV1
    {
        // Sample String
        // string stringwithspecialcharacters = "Th1i3s i4s a3 s@3mple str0ing wi25th !@#$%^&*()_-+<speical characters *&^&^k";
        // Remove everything from string except number, kK and -
        // string formatedProductName = Regex.Replace(stringwithspecialcharacters, @"[^0-9kK-]+", "");
        // Remove everything from string except number
        // string formatedProductName = Regex.Replace(stringwithspecialcharacters, @"[^0-9]+", "");
        // Console.WriteLine("{0}", formatedProductName);

        public VerificaRutHelpersV1() { 
        }

        public bool verificaRut(string rut)
        {
            var rutDvArray = rut.Split("-");
            var rutSinDv = int.Parse(rutDvArray[0]);
            var dv = rutDvArray[1];

            return verificaRut(rutSinDv, dv);
        }

        public bool verificaRut(int rut, string dv)
        {
            int Digito;
            int Contador;
            int Multiplo;
            int Acumulador;
            string RutDigito;

            Contador = 2;
            Acumulador = 0;

            while (rut != 0)
            {
                Multiplo = (rut % 10) * Contador;
                Acumulador = Acumulador + Multiplo;
                rut = rut / 10;
                Contador = Contador + 1;

                if (Contador == 8)
                {
                    Contador = 2;
                }
            }

            Digito = 11 - (Acumulador % 11);
            RutDigito = Digito.ToString().Trim().ToUpper();
            if (Digito == 10)
            {
                RutDigito = "K";
            }
            if (Digito == 11)
            {
                RutDigito = "0";
            }

            if (RutDigito.ToString() == dv.ToString().Trim().ToUpper())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
