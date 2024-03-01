using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TaskManager
{
    public class PriorityValidationRule : ValidationRule
    {
        public double Min { get; set; }
        public double Max { get; set; }

        public PriorityValidationRule()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo ci)
        {
            double priority = 0;

            try
            {
                if (((string)value).Length > 0)
                    priority = int.Parse((string)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Illegal characters or {e.Message}");
            }

            if (priority < Min || priority > Max)
            {
                return new ValidationResult(false,
                    $"Priorytet przyjmuje wartości z przedziału {Min} - {Max}.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
