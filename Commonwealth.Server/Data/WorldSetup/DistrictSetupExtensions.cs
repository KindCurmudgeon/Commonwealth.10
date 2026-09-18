using Commonwealth.Shared.Common;

namespace Commonwealth.Server.Data;
public partial class DistrictSetup
{
        public bool HasFeature(Feature? feature)
    {
        if (feature is null || feature == Feature.NONE) return true;
        Feature? found = Features?.Find(f => f == feature);
        return found is not null;
    }
}