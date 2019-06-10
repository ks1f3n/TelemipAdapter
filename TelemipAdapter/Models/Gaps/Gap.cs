using TelemipAdapter.Models.Base;

namespace TelemipAdapter.Models.Gaps
{
    public class Gap : ModelIntIdentifier
    {
        public int InitValue { get; set; }
        public int Value { get; set; }
        public int Period { get; set; }
    }
}
