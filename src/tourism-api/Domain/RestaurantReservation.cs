namespace tourism_api.Domain;

public class RestaurantReservation
{
    public int Id { get; set; }
    public int RestaurantId { get; set; }
    public int UserId { get; set; }
    public DateTime ReservationDate { get; set; }
    public string MealType { get; set; }
    public int NumberOfGuests { get; set; }
    public string Status { get; set; } = "pending";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public Restaurant? Restaurant { get; set; }
    public User? User { get; set; }

    public bool IsValid()
    {
        return RestaurantId > 0 && UserId > 0  && 
               !string.IsNullOrWhiteSpace(MealType) && 
               NumberOfGuests > 0;
    }
} 