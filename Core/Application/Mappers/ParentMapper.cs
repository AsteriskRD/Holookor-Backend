using HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Application.DTOs.HolookorBackend.Core.Application.DTOs;
using HolookorBackend.Core.Domain.Entities;

namespace HolookorBackend.Core.Application.Mappers
{
    public static class ParentMapper
    {
        public static ParentDto Map(Parent parent, IEnumerable<Student> children)
        {
            return new ParentDto(
                parent.Id,
                parent.UserProfileId,
                children.Select(StudentMapper.Map).ToList()
            );
        }
    }
}
