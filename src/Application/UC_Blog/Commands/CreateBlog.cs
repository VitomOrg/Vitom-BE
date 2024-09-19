using Application.Contracts;
using Application.Mappers.BlogMappers;
using Application.Responses.BlogResponses;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UC_Blog.Commands;

public class CreateBlog
{

    public record Command(
        string Title,
        string Content,
        IFormFileCollection Images
    ) : IRequest<Result<CreateBlogResponses>>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser, IFirebaseService firebaseService) : IRequestHandler<Command, Result<CreateBlogResponses>>
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
            List<Task<string>> tasks = [];
            foreach (var image in request.Images)
            {
                tasks.Add(firebaseService.UploadFile(image.Name, image, "products"));
            }
            string[] imageUrls = await Task.WhenAll(tasks);
            newBlog.Images = imageUrls.Select(i => new BlogImage()
            {
                Url = i,
                BlogId = newBlog.Id
            }).ToList();
            context.Blogs.Add(newBlog);
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(newBlog.MapToCreateBlogResponse(), "Create new blog successfully");
        }
    }
}