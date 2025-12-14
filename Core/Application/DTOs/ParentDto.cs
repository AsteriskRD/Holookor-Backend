namespace HolookorBackend.Core.Application.DTOs
{
    namespace HolookorBackend.Core.Application.DTOs
    {
        public record ParentDto(
            string Id,
            string UserProfileId,
            IReadOnlyCollection<StudentDto> Children
        );

        public record CreateParentRequest(
            string UserProfileId,
            string[] ChildrenIds
        );

        public record UpdateParentRequest(
            string[]? ChildrenIds = null
        );
    }

}
