namespace RentCar.WebClient.Models.Cars
{
    public enum CarValidationStatus
    {
        Valid = 1,
        MissingDates = -1,
        InvalidDateRange = -2
    }
}
