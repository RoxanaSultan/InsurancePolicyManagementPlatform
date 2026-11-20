using InsuranceApp.Application.Interfaces.Repositories.Policy;
using InsuranceApp.Domain.Entities.Policy;

namespace InsuranceApp.Infrastructure.Persistence.Repositories.Policy;

public class PolicyCancellationRepository(InsuranceDbContext context) 
    : GenericRepository<PolicyCancellation>(context), IPolicyCancellationRepository
{}