namespace tourism_api.Domain
{
    public class TourReservations
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public int UserId { get; set; }
        public int NumberOfGuests { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsValid()
        {
            return TourId > 0 && UserId > 0 && NumberOfGuests > 0;
        }
    }
}
