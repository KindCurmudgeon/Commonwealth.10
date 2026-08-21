
using Commonwealth.Server.Data;
using Commonwealth.Server.Endpoints.ExceptionHandling;
using Commonwealth.Server.Utilities;
using Commonwealth.Shared.EconomicMgrs;
using Commonwealth.Shared.EndpointDTOs;

namespace Commonwealth.Server.Endpoints;

public static partial class OrdersEndpoints
{
    public static async Task UpdateOrdersAsync(OrdersRequest request, Nation nation, BlobService blobService, OrdersResponse response)
    {
        Game game = await Game.RetrieveAsync(nation.Identity.GameName, blobService);
        if (game.GameDate.IsSame(nation.SeasonCount) is false) throw new AppException(ExceptionType.Endpoint, EndpointFailType.SeasonUpdated, (string?)null);
        UpdateVillageOrders();
        UpdateDistrictOrders();
        UpdateSpyOrders();
        UpdateTradeOrders();
        nation.OrdersState = request.SubmissionState ?? OrdersState.None;

        await nation.SaveAsync(blobService);
        response.AddMessage(nation.OrdersState == OrdersState.OrdersSubmitted ?
            "Orders Saved & Authorized" : "Orders Saved");
        void UpdateVillageOrders()
        {
            foreach (VillageMgr mgr in request.VillageMgrs ?? [])
            {
                Village? village = nation.Villages?.Find(v => v.Identity.Equals(mgr.Identity));
                if (village is null)
                {
                    (nation.Villages ?? []).Add(new Village(mgr));
                }
                else village.Order = mgr.Order;
            }
        }
        void UpdateDistrictOrders()
        {
            // foreach (DistrictOrder order in request.DistrictOrders ?? [])
            // {
            //     DistrictOrder? match = nation.DistrictOrders?.Find(d => d.Name == order.Name);
            //     if (match is null)
            //     {
            //         (nation.DistrictOrders ?? []).Add(order);
            //     }
            //     else match = order;
            // }
        }
        void UpdateSpyOrders()
        {
            foreach (SpyMgr mgr in request.UpdatedSpyMgrs ?? [])
            {
                switch (mgr.Order.Status)
                {
                    case SpyState.Added:
                        Spy spy = new Spy(mgr) { Order = mgr.Order };
                        spy.Order.Status = SpyState.Registered;
                        (nation.Spies ??= []).Add(spy);
                        break;
                    default:
                        Spy? spy1 = nation.Spies?.Find(s => s.Id == mgr.Id);
                        spy1?.Order = mgr.Order;
                        break;
                }
            }
        }
        void UpdateTradeOrders()
        {
            foreach (TradeMgr mgr in request.TradeMgrs ?? [])
            {
                Trade? match = nation.Trades?.Find(t => t.Id == mgr.Id);
                if (match is null)
                {
                    (nation.Trades ??= []).Add(new Trade(mgr));
                }
                else match.Order = mgr.Order;
            }
        }
    }
}

