using InsuranceApp.Application.DTOs.Paging;

namespace InsuranceApp.Application.Tests.TestData;

public static class PagingValidatorTestData
{
    public static PageRequest ValidPageRequest(
        int page = 1,
        int pageSize = 20)
        => new()
        {
            Page = page,
            PageSize = pageSize
        };
}
