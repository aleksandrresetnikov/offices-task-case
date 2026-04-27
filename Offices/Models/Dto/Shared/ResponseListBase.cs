namespace Offices.Models.Dto.Shared;

public class ResponseListBase<T>
{
    public List<T> Data { get; set; }
    public int Page { get; set; }
    public int Count { get; set; }
    public int TotalCount { get; set; }
}