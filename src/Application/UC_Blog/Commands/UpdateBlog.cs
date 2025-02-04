using Application.Caches.Events;
using Application.Contracts;
using Application.Responses.BlogResponses;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Blog.Commands;

public class UpdateBlog
{
    public record Command(
        Guid Id,
        string Title,
        string Content,
        string[] Images
    ) : IRequest<Result<UpdateBlogResponse>>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser, IFirebaseService firebaseService) : IRequestHandler<Command, Result<UpdateBlogResponse>>
    {
        public async Task<Result<UpdateBlogResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            // check if current user is admin
            if (!currentUser.User!.IsOrganization()) return Result.Forbidden();
            // get updating blog
            Blog? updatingBlog = await context.Blogs
                .Include(b => b.Images)
                .Include(b => b.User)
                .Where(b => b.DeletedAt == null)
                .SingleOrDefaultAsync(b => b.Id.Equals(request.Id) && b.DeletedAt == null, cancellationToken);
            if (updatingBlog is null) return Result.NotFound();
            // check if user is owner
            if (!updatingBlog.UserId.Equals(currentUser.User.Id)) return Result.Forbidden();
            // update the blog
            updatingBlog.Update(
                title: request.Title,
                content: request.Content
            );
            // remove old images
            if (updatingBlog.Images.Count > 0)
                context.BlogImages.RemoveRange(updatingBlog.Images);
            //remove images from firebase
            IEnumerable<BlogImage> unusedBlogImages = context.BlogImages
                .AsNoTracking()
                .Where(bi => bi.BlogId.Equals(updatingBlog.Id))
                .Where(bi => !request.Images.Contains(bi.Url));
            List<Task<bool>> deleteTasks = [];
            foreach (var image in unusedBlogImages)
            {
                deleteTasks.Add(firebaseService.DeleteFile(image.Url));
            }
            await Task.WhenAll(deleteTasks);
            if (deleteTasks.Any(t => !t.Result)) return Result.Error("Delete images failed");
            // add new images
            await context.BlogImages.AddRangeAsync(request.Images
                .Select(url => new BlogImage
                {
                    Url = url,
                    BlogId = updatingBlog.Id
                })
                .ToArray());
            updatingBlog.AddDomainEvent(new EntityUpdated.Event("blog"));
            // save to db
            await context.SaveChangesAsync(cancellationToken);
            // return result
            return Result.Success(new UpdateBlogResponse(request.Images), "Update blog successfully");
        }
    }
}