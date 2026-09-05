namespace IdentityProvider.EndpointDTOs;

public static class Conversions
{
     public static Guid StringToGuid(string? id)
     {
          if (!Guid.TryParse(id, out Guid guid)) return Guid.Empty;
          return guid;
     }
}