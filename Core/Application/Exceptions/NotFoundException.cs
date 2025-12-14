namespace HolookorBackend.Core.Application.Exceptions
{
    namespace HolookorBackend.Core.Application.Exceptions
    {
        public class NotFoundException : Exception
        {
            public NotFoundException(string message) : base(message) { }
        }

        public class ValidationException : Exception
        {
            public ValidationException(string message) : base(message) { }
        }

        public class DomainException : Exception
        {
            public DomainException(string message) : base(message) { }
        }
        public class ConflictException : Exception
        {
            public ConflictException(string message) : base(message) { }
        }
    }

}
