using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Attributes
{
    public class StartsWithAttribute : ValidationAttribute
    {
        private readonly string _keyword;

        public StartsWithAttribute(string keyword)
        {
            _keyword = keyword;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string str && str.StartsWith(_keyword))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("ID must start with 'PL'");
        }
    }
}
