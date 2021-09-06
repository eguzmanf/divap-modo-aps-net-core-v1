using FiftyOne.Foundation.Mobile.Detection.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.CustomValidation
{
    public class CustomTest : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var fieldUserName = value.ToString();
            
            return fieldUserName == "Hola";
        }
    }
}
