using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.CustomValidation
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string domain;

        public ValidEmailDomainAttribute(string Domain)
        {
            this.domain = Domain;
        }

        public override bool IsValid(object value)
        {
            string[] values = value.ToString().Split('@');

            if(values[1].ToLower() == this.domain.ToLower())
            {
                return true;
            } else
            {
                return false;
            }
            
        }
    }
}
