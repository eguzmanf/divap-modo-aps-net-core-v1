using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.Helpers.Rut.V2
{
    public interface IVerificaRutHelpersV2
    {
        bool ValidaRut(string rut);
        bool ValidaRut(string rut, string dv);
        string Digito(int rut);
    }
}
