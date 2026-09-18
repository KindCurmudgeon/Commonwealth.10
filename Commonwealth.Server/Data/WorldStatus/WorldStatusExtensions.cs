using Commonwealth.Server.Endpoints.ExceptionHandling;

namespace Commonwealth.Server.Data;
public partial class WorldStatus
{
     public List<DistrictStatus> GetOwnedDistricts(int nationCode)
     {
          return Districts.Where(d => d.Owner == nationCode).ToList();
     }
     public DistrictStatus GetDistrict(string? districtName)
     {
          DistrictStatus? district =  Districts.Find(d => d.Name == districtName);
          if (district == null) throw new AppException(ExceptionType.Blob, BlobFailType.NotFound, $"District: {districtName ?? "null"}");
          return district;
     }
}