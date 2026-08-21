
namespace Commonwealth.Shared.EconomicMgrs;

public interface IOrderMgr<T>
{
     T? Order { get; }
     void ChangeOrder(T order);
     bool HasChanged { get; set; }
     List<string> Adjustments { get; set; }

}