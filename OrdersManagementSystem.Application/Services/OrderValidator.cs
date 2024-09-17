using FluentValidation;
using OrdersManagementSystem.Application.DTOs;

namespace OrdersManagementSystem.Application.Services
{
    public class OrderValidator : AbstractValidator<OrderDTO>
    {
        public OrderValidator()
        {
            RuleFor(order => order.OrderID)
            .NotEqual(Guid.Empty).WithMessage("OrderID must be a valid, non-default GUID.");

            RuleFor(order => order.CustomerID)
                .NotEmpty().WithMessage("CustomerID is required.");

            RuleFor(order => order.OrderDate)
                .NotEqual(default(DateTime)).WithMessage("OrderDate is required.");

            RuleFor(order => order.OrderItems)
                .NotEmpty().WithMessage("Order must have at least one item.");

            RuleForEach(order => order.OrderItems)
                .ChildRules(items =>
                {
                    items.RuleFor(item => item.ProductID).NotEmpty().WithMessage("ProductID is required.");
                    items.RuleFor(item => item.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
                    items.RuleFor(item => item.TotalPrice).GreaterThan(0).WithMessage("TotalPrice must be greater than zero.");
                });
            RuleFor(order => order.Status)
            .IsInEnum().WithMessage("Status must be a valid value.");
        }
    }
}
