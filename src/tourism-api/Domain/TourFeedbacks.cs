namespace tourism_api.Domain
{
    public class TourFeedbacks
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public int UserId { get; set; }
        public int? UserRating { get; set; }
        public string? UserComment { get; set; }
        public DateTime PostedOn { get; set; }
        public bool IsValid()
        {
            return TourId > 0 && UserId > 0;//Ubaciti proveru za postedOn
        }
    }
}
