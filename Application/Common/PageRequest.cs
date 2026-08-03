using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DCOM_API.Common;

public class PageRequest
{
    private const int MaxPageSize = 100;
    private int _pageNumber = 1;
    private int _pageSize = 10;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 10 : (value > MaxPageSize ? MaxPageSize : value);
    }

    [BindNever]
    public int Skip => (PageNumber - 1) * PageSize;

    [BindNever]
    public int Take => PageSize;
}