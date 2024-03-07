using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTOs;

namespace StocksApp.Filters.ActionFilters
{
    public class RedirectOnModelErrorActionFilter : IActionFilter
    {
        private readonly ILogger<RedirectOnModelErrorActionFilter> _logger;

        public RedirectOnModelErrorActionFilter(ILogger<RedirectOnModelErrorActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("On Executed");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("On Executing");
            if (!context.ModelState.IsValid)
            {
                var stockSymbol = "";
                if (context.ActionArguments.TryGetValue("buyOrderRequest", out var retrievedBuyOrderRequest)) 
                {
                    var buyOrderRequest = retrievedBuyOrderRequest as BuyOrderRequest;
                    stockSymbol = buyOrderRequest?.StockSymbol;
                }
                else if (context.ActionArguments.TryGetValue("sellOrderRequest", out var retrievedSellOrderRequest))
                {
                    var sellOrderRequest = retrievedSellOrderRequest as SellOrderRequest;
                    stockSymbol = sellOrderRequest?.StockSymbol;
                }

                context.Result = new RedirectToActionResult("Index", "Trade", new { stockSymbol });
            }
        }
    }
}
