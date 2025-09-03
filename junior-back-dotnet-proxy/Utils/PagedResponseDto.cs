// Api/Dtos/PagedResponseDto.cs
namespace junior_back_dotnet_proxy.Utils;
/// Generic DTO for paginated responses.
/// <typeparam name="T">Type of the items in the response.</typeparam>
public class PagedResponseDto<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
}
