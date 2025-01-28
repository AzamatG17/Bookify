namespace Bookify.Application.Responses;

public class FreeTimeOnlinetResponse
{
    public int FreePlacesCount { get; set; }
    public int Hour { get; set; }
    public int Minute { get; set; }
    public int Second { get; set; }
    public string Time { get; set; }
    public int TotalPlacesCount { get; set; }
}
