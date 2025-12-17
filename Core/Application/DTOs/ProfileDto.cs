using HolookorBackend.Core.Application.DTOs.HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Domain.Entities;

namespace HolookorBackend.Core.Application.DTOs
{
    public record ProfileDto(
     string UserProfileId,
     string FirstName,
     string LastName,
     string PhoneNumber,
     string Email,
     string Role,

     StudentDto? Student,
     TutorDto? Tutor,
     ParentDto? Parent
 );

}
