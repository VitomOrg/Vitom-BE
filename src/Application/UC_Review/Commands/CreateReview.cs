using Application.Caches.Events;
using Application.Contracts;
using Application.Mappers.ReviewMappers;
using Application.Responses.ReviewResponses;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Review.Commands;

public class CreateReview
{
    public record Command
    (
        Guid ProductId,
        int Rating,
        string Content
    ) : IRequest<Result<CreateReviewResponse>>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser) : IRequestHandler<Command, Result<CreateReviewResponse>>
    {
        public async Task<Result<CreateReviewResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            // check if user is not logged in
            if (currentUser.User == null) return Result.Forbidden();
            // check if product is existed
            Product? product = await context.Products
                .Where(p => p.DeletedAt == null)
                .SingleOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);
            if (product == null) return Result.NotFound($"Product with id {request.ProductId} is not existed");
            // init new review object
            Review newReview = new()
            {
                ProductId = request.ProductId,
                UserId = currentUser.User.Id,
                Rating = request.Rating,
                Content = request.Content
            };
            //add new review and save changes to database
            context.Reviews.Add(newReview);
            newReview.AddDomainEvent(new EntityCreated.Event("review"));
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(newReview.MapToCreateReviewResponse(), "Review created successfully");
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Rating).InclusiveBetween(1, 5);
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}