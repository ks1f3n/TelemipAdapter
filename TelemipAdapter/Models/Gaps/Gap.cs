using System.ComponentModel.DataAnnotations;
using TelemipAdapter.Models.Base;

namespace TelemipAdapter.Models.Gaps
{
    public class Gap : ModelIntIdentifier
    {
        [Display(Name = "Коррекция нуля")]
        public int InitValue { get; set; }
        public int Value { get; set; }
        [Display(Name = "Период отправки данных, с")]
        public int Period { get; set; }
    }
}
