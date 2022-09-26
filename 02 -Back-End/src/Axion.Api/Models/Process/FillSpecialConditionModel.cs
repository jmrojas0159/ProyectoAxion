#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace Axion.Api.Models
{
    public class FillSpecialConditionModel
    {
        [Required] public int IdSpecialCondition { get; set; }

        [Required] public int IdForm { get; set; }

        [Required] public bool PositiveValue { get; set; }

        [Required] public bool NegativeValue { get; set; }
    }
}