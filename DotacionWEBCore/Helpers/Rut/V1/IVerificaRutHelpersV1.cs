using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.Helpers.Rut.V1
{
    public interface IVerificaRutHelpersV1
    {
        bool verificaRut(string rut);
        bool verificaRut(int rut, string dv);
    }
}
