using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TaskManager
{
    public class TaskContentValidationRule : ValidationRule
    {

        public TaskContentValidationRule()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo ci)
        {
            
            if (string.IsNullOrWhiteSpace((string)value))
            {
                    return new ValidationResult(false, $"Zadanie musi posiadać treść");
            }
            return ValidationResult.ValidResult;
        }
    }
}
