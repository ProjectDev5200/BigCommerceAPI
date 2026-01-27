using Avrhil.BigCommerce.API.Models;

namespace Avrhil.BigCommerce.API.Services;

public class OrdersService
{
    private readonly BigCommerceApiClient _apiClient;

    public OrdersService(BigCommerceApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<OrdersResponse> GetOrdersAsync(int page = 1, int limit = 50)
    {
        return await _apiClient.GetAsync<OrdersResponse>($"orders?page={page}&limit={limit}");
    }

    public async Task<OrderResponse> GetOrderByIdAsync(int orderId)
    {
        return await _apiClient.GetAsync<OrderResponse>($"orders/{orderId}");
    }

    public async Task<OrderResponse> UpdateOrderStatusAsync(int orderId, int statusId)
    {
        var payload = new { status_id = statusId };
        return await _apiClient.PutAsync<OrderResponse>($"orders/{orderId}", payload);
    }

    public async Task<OrderShipmentsResponse> GetOrderShipmentsAsync(int orderId)
    {
        return await _apiClient.GetAsync<OrderShipmentsResponse>($"orders/{orderId}/shipments");
    }

    public async Task<OrderShipmentResponse> CreateShipmentAsync(int orderId, OrderShipmentRequest shipment)
    {
        return await _apiClient.PostAsync<OrderShipmentResponse>($"orders/{orderId}/shipments", shipment);
    }

    public async Task<bool> DeleteShipmentAsync(int orderId, int shipmentId)
    {
        return await _apiClient.DeleteAsync($"orders/{orderId}/shipments/{shipmentId}");
    }
}
