using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Domain.Enums;

namespace InsuranceApp.Application.DTOs.Report;

public class ReportGenericResponse<T>
{
    public string ReportName { get; set; } = string.Empty;
    public GroupingInfo GroupedBy { get; set; } = new GroupingInfo();
    public DateTime GeneratedAt { get; set; }
    public ReportFilterRequest FilteredBy { get; set; } = new ReportFilterRequest();
    public PageResponse<T> Reports { get; set; } = default!;
}