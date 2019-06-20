using System.ComponentModel.DataAnnotations;
using TelemipAdapter.Models.Base;

namespace TelemipAdapter.Models.Incls
{
    public class Incl : ModelIntIdentifier
    {
        public int InitX { get; set; }
        public int X { get; set; }
        public int InitY { get; set; }
        public int Y { get; set; }
        [Display(Name = "Период отправки данных, с")]
        public int Period { get; set; }
    }
}
