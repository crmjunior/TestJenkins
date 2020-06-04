using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation.Results;

namespace Shared.Entities
{
    public class ViewModelValidation
    {
        [NotMapped]
        public ValidationResult ValidationResult { get; set; }
    }
}