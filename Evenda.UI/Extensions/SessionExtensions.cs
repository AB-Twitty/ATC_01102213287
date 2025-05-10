using Evenda.UI.Dtos.Auth;

namespace Evenda.UI.Extensions
{
    public static class SessionExtensions
    {
        public static void SetUserSession(this ISession session, AuthDto authDto)
        {
            session.SetString("access-token", authDto.AccessToken);
            session.SetString("refresh-token", authDto.RefreshToken);
            session.SetString("user-fname", authDto.FirstName);
            session.SetString("user-lname", authDto.LastName);
            session.SetString("user-id", authDto.Id.ToString());
            session.SetString("user-email", authDto.Email);
            session.SetString("user-roles", string.Join(",", authDto.Roles));
        }

        public static AuthDto GetUserSession(this ISession session)
        {
            var id = session.GetString("user-id");

            if (!Guid.TryParse(id, out Guid gid))
                return new AuthDto();

            return new AuthDto
            {
                Id = gid,
                FirstName = session.GetString("user-fname"),
                LastName = session.GetString("user-lname"),
                Email = session.GetString("user-email"),
                AccessToken = session.GetString("access-token"),
                RefreshToken = session.GetString("refresh-token"),
                Roles = session.GetString("user-roles")?.Split(',').ToList() ?? new List<string>(),
            };
        }
    }
}
