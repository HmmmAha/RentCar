namespace RentCar.WebClient.Models.Cars
{
    public enum CarValidationStatus
    {
        Valid,
        MissingDates,
        InvalidDateRange,
        PastDate
    }
}
