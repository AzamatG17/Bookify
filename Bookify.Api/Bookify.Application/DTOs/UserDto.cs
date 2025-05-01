namespace Bookify.Application.DTOs;

public record UserDto(
    string FirstName,
    string LastName,
    string PhoneNumber,
    List<BookingWithIdsDto> BookingDtos,
    List<ETicketDto> ETicketDtos
    );
