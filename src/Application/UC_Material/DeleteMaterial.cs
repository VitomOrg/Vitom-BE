using Application.Contracts;
using Ardalis.Result;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Material;

public class DeleteMaterial
{
    public record Command(
        Guid ProductId,
        Guid[] MaterialIds
    ) : IRequest<Result>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser, IFirebaseService firebaseService) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            //check if user is not organization
            if (!currentUser.User!.IsOrganization())
                return Result.Forbidden();
            //check if material exists in database
            var deleteMaterial = context.ModelMaterials
                .AsNoTracking()
                .Where(m => request.MaterialIds.Contains(m.Id) && m.ProductId.Equals(request.ProductId));
            if (deleteMaterial.Count() != request.MaterialIds.Length)
                return Result.NotFound($"Some materials are not found");
            //delete material
            context.ModelMaterials.RemoveRange(deleteMaterial);
            //delete material in firebase
            List<Task<bool>> tasksDelete = [.. deleteMaterial.Select(d => firebaseService.DeleteFile(d.Url))]; ;
            // await all task execution
            await Task.WhenAll(tasksDelete);
            if (tasksDelete.Any(t => !t.Result)) return Result.Error("Delete material files failed");
            await context.SaveChangesAsync(cancellationToken);
            return Result.NoContent();
        }
    }
}