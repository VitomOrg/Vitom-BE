using Application.Caches.Events;
using Application.Contracts;
using Application.Mappers.BlogMappers;
using Application.Responses.BlogResponses;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UC_Blog.Commands;

public class CreateBlog
{

    public record Command(
        string Title,
        string Content,
        string[] Images
    ) : IRequest<Result<CreateBlogResponses>>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser) : IRequestHandler<Command, Result<CreateBlogResponses>>
    {
        public async Task<Result<CreateBlogResponses>> Handle(Command request, CancellationToken cancellationToken)
        {
            // check if user is Organization
            if (!currentUser.User!.IsOrganization()) return Result.Forbidden();
            // init new blog object
            Blog newBlog = new()
            {
                Title = request.Title,
                Content = request.Content,
                UserId = currentUser.User.Id,
            };
            newBlog.Images = request.Images.Select(i => new BlogImage()
            {
                Url = i,
                BlogId = newBlog.Id
            }).ToList();
            context.Blogs.Add(newBlog);
            newBlog.AddDomainEvent(new EntityCreated.Event("blog"));
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(newBlog.MapToCreateBlogResponse(), "Create new blog successfully");
        }
    }
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required");
        }
    }
}