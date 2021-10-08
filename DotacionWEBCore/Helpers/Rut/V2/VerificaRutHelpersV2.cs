using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotacionWEBCore.Helpers.Rut.V2
{
    public class VerificaRutHelpersV2 : IVerificaRutHelpersV2
    {
		/// <summary>
		/// Metodo de validación de rut con digito verificador
		/// dentro de la cadena
		/// </summary>
		/// <param name="rut">string</param>
		/// <returns>booleano</returns>

		// Sample String
		// string stringwithspecialcharacters = "Th1i3s i4s a3 s@3mple str0ing wi25th !@#$%^&*()_-+<speical characters *&^&^k";
		// Remove everything from string except number, kK and -
		// string formatedProductName = Regex.Replace(stringwithspecialcharacters, @"[^0-9kK-]+", "");
		// Remove everything from string except number
		// string formatedProductName = Regex.Replace(stringwithspecialcharacters, @"[^0-9]+", "");
		// Console.WriteLine("{0}", formatedProductName);

		public bool ValidaRut(string rut)
		{
			rut = rut.Replace(".", "").ToUpper();
			Regex expresion = new Regex("^([0-9]+-[0-9K])$");
			string dv = rut.Substring(rut.Length - 1, 1);
			if (!expresion.IsMatch(rut))
			{
				return false;
			}
			char[] charCorte = { '-' };
			string[] rutTemp = rut.Split(charCorte);
			if (dv != Digito(int.Parse(rutTemp[0])))
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Método que valida el rut con el digito verificador
		/// por separado
		/// </summary>
		/// <param name="rut">integer</param>
		/// <param name="dv">char</param>
		/// <returns>booleano</returns>
		public bool ValidaRut(string rut, string dv)
		{
			return ValidaRut(rut + "-" + dv);
		}

		/// <summary>
		/// método que calcula el digito verificador a partir
		/// de la mantisa del rut
		/// </summary>
		/// <param name="rut"></param>
		/// <returns></returns>
		public string Digito(int rut)
		{
			int suma = 0;
			int multiplicador = 1;
			while (rut != 0)
			{
				multiplicador++;
				if (multiplicador == 8)
					multiplicador = 2;
				suma += (rut % 10) * multiplicador;
				rut = rut / 10;
			}
			suma = 11 - (suma % 11);
			if (suma == 11)
			{
				return "0";
			}
			else if (suma == 10)
			{
				return "K";
			}
			else
			{
				return suma.ToString();
			}
		}
	}
}
