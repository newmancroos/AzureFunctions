using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions
{
    public static class OnPaymentReceived
    {
        [FunctionName("OnPaymentReceived")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Queue("order")] IAsyncCollector<Order> orderQueue,
            ILogger log)
        {
            

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic order = JsonConvert.DeserializeObject<Order>(requestBody);
            await orderQueue.AddAsync(order);
            log.LogInformation($"Thank you for your paymant. Order Recevied from {order.Email}");
            return new OkObjectResult($"Thank you for your putrchase");
        }
    }


public class Order
{
    public string OrderId { get; set; } 
    public string ProductId { get; set;}
    public string Email { get; set; }
    public decimal Price { get; set; }  
}
}