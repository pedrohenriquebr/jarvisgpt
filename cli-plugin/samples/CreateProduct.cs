
using MediatR;
using FluentValidation;

namespace MyCrud.Application.Behavior.CreateProduct
{
    public enum ProductCategory : byte
    {
        ELETRONICS = 1,
        HOME,
        FASHION
    }

    public class CreateProductCommand : IRequest
    {
        public string Name { get; }
        public Money Price { get; }
        public ProductCategory Category { get; }
        public DateTime ExpireDate { get; }
    }

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .Length(3, 20);

            RuleFor(x => x.Price)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.ExpireDate)
                .NotNull();
        }
    }

    public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Implementation here
        }
    }
}
