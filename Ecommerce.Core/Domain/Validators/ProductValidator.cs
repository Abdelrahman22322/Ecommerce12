using Ecommerce.Core.Domain.Entities;
using FluentValidation;

namespace Ecommerce.Core.Domain.Validators;

public class ProductValidator : AbstractValidator<Product>
{
    
    public ProductValidator()
    {
        RuleFor(x => x.ProductName).NotEmpty().WithMessage("Product name is required");

        RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("Product price must be greater than 0");
        RuleFor(x => x.UnitsInStock).GreaterThanOrEqualTo(0).WithMessage("Product stock must be greater than or equal to 0");
        RuleFor(x => x.UnitsOnOrder).GreaterThanOrEqualTo(0).WithMessage("Product order must be greater than or equal to 0");
        RuleFor(x => x.ReorderLevel).GreaterThanOrEqualTo(0).WithMessage("Product reorder level must be greater than or equal to 0");

        RuleFor(x => x.SupplierId).NotEmpty().WithMessage("Supplier ID is required");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category ID is required");
        RuleFor(x => x.BrandId).NotEmpty().WithMessage("Brand ID is required");

        //RuleFor(x => x.ProductName).Must(BeUniqueProductName).WithMessage("Product name must be unique");

        //RuleFor(x => x.UnitPrice).Must(BeValidPrice).WithMessage("Product price must be a valid value");

        //RuleFor(x => x.UnitsInStock).Must(BeValidStock).WithMessage("Product stock must be a valid value");

        //RuleFor(x => x.UnitsOnOrder).Must(BeValidOrder).WithMessage("Product order must be a valid value");

        //RuleFor(x => x.ReorderLevel).Must(BeValidReorderLevel).WithMessage("Product reorder level must be a valid value");

        RuleFor(x => x.Supplier).NotNull().WithMessage("Supplier must be provided");
        RuleFor(x => x.Category).NotNull().WithMessage("Category must be provided");
        RuleFor(x => x.Brand).NotNull().WithMessage("Brand must be provided");
    }

    

   

    //private bool BeValidStock(int unitsInStock)
    //{
    //    // Check if the units in stock is a valid value
    //    // Implement the logic here
    //    throw new NotImplementedException();
    //}

    //private bool BeValidOrder(int unitsOnOrder)
    //{
    //    // Check if the units on order is a valid value
    //    // Implement the logic here
    //    throw new NotImplementedException();
    //}

    //private bool BeValidReorderLevel(int reorderLevel)
    //{
    //    // Check if the reorder level is a valid value
    //    // Implement the logic here
    //    throw new NotImplementedException();
    //}

    //// Additional validation methods

    //private bool BeValidWeight(decimal weight)
    //{
    //    // Check if the weight is a valid value
    //    // Implement the logic here
    //    throw new NotImplementedException();
    //}

    //private bool BeValidDimensions(decimal length, decimal width, decimal height)
    //{
    //    // Check if the dimensions are valid values
    //    // Implement the logic here
    //    throw new NotImplementedException();
    //}

    //private bool BeValidExpirationDate(DateTime expirationDate)
    //{
    //    // Check if the expiration date is a valid value
    //    // Implement the logic here
    //    throw new NotImplementedException();
    //}

    //// Additional exception methods

    //private void ThrowInvalidWeightException()
    //{
    //    throw new ArgumentException("Invalid weight value");
    //}

    //private void ThrowInvalidDimensionsException()
    //{
    //    throw new ArgumentException("Invalid dimensions values");
    //}

    //private void ThrowInvalidExpirationDateException()
    //{
    //    throw new ArgumentException("Invalid expiration date value");
    //}
}
