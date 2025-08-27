namespace tourism_api.Domain
{
    public class TourStats
    {
        public List<Tour> tourMaxRes { get; set; } = new List<Tour>();
        public List<Tour> tourMinRes { get; set; } = new List<Tour>();
        public List<Tour> tourMaxPercentage { get; set; } = new List<Tour>();
        public List<Tour> tourMinPercentage { get; set; } = new List<Tour>();
        public int totalTours { get; set; } = 0;
    }
}
