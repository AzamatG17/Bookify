namespace Bookify.Application.Requests.Services;

public record UpdateServiceGroupRequest(
    int Id,
    List<int> ServiceGroupIds
    );
