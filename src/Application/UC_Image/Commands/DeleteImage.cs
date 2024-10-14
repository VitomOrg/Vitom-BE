using Application.Contracts;
using Ardalis.Result;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Image.Commands;

public class DeleteImage
{
    public record Command(
        Guid ProductId,
        Guid[] ImageIds
    ) : IRequest<Result>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser, IFirebaseService firebaseService) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            //check if user is organization
            if (!currentUser.User!.IsOrganization())
                return Result.Forbidden();
            //check if image id exists
            var deleteImages = context.ProductImages
                .AsNoTracking()
                .Where(image => image.DeletedAt == null)
                .Where(image => image.ProductId.Equals(request.ProductId))
                .Where(image => request.ImageIds.Contains(image.Id));
            if (deleteImages.Count() != request.ImageIds.Length)
                return Result.NotFound($"Some images are not found");
            //delete images
            context.ProductImages.RemoveRange(deleteImages);
            //delete images in firebase
            List<Task<bool>> tasksDelete = [.. deleteImages.Select(d => firebaseService.DeleteFile(d.Url))];
            // await all task execution
            await Task.WhenAll(tasksDelete);
            if (tasksDelete.Any(t => !t.Result)) return Result.Error("Delete image files failed");
            //Save changes to database
            await context.SaveChangesAsync(cancellationToken);
            return Result.NoContent();
        }
    }
}