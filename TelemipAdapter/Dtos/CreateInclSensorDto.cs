namespace TelemipAdapter.Dtos
{
    public class CreateInclSensorDto
    {
        public int UID { get; set; }
        public int ST { get; set; }
        public int PER { get; set; }
        public int VOLT { get; set; }
        public int CSQ { get; set; }
        public int[] X { get; set; }
        public int[] Y { get; set; }
        public int[] T { get; set; }
        public long[] TS { get; set; }
    }
}
