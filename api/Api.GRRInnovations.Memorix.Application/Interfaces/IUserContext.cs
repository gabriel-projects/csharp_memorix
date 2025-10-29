namespace Api.GRRInnovations.Memorix.Application.Interfaces
{
    public interface IUserContext
    {
        Guid? UserId { get; }
        string? Email { get; }
        string? Name { get; }
        bool IsAuthenticated { get; }

        Guid RequireUserId();
    }
}
