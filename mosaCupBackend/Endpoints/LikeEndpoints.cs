using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using mosaCupBackend.Data;
using mosaCupBackend.Models.DbModels;
using mosaCupBackend.Models.ReqModels;

namespace mosaCupBackend.Endpoints;

public static class LikeEndpoints
{
    public static void MapLikeEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Like").WithTags(nameof(like));

        //Get Like User
        group.MapGet("/{pid}", async Task<Results<Ok<List<like>>, NotFound>> (Guid Pid, mosaCupBackendContext db) =>
        {
            var likeList = await db.Like
                .Where(model => model.PostId == Pid)
                .ToListAsync();

            return likeList != null ? TypedResults.Ok(likeList) : TypedResults.NotFound();
        })
        .WithName("GetLikeById")
        .WithOpenApi();

        //Like
        group.MapPost("/", async (LikeReq reqData, mosaCupBackendContext db) =>
        {
            var like = new like { Id = Guid.NewGuid(), PostId = reqData.PostId, Uid = reqData.Uid };
            db.Like.Add(like);

            var Post = await db.Post
                .FirstOrDefaultAsync(m => m.Id == like.PostId);

            var notification = new notification
            {
                Id = Guid.NewGuid(),
                Uid = Post.Uid,
                AffectUid = like.Uid,
                TypeCode = 1,
                Date = DateTime.UtcNow,
                Pid = Post.Id
            };
            db.Notification.Add(notification);

            await db.SaveChangesAsync();

            return TypedResults.Created($"/api/Like/{like.Id}", like);
        })
        .WithName("CreateLike")
        .WithOpenApi();

        //Delete Like
        group.MapPost("/Delete", async Task<Results<Ok, NotFound>> (LikeReq reqData, mosaCupBackendContext db) =>
        {
            var likeData = await db.Like
                .FirstOrDefaultAsync(model => model.PostId == reqData.PostId && model.Uid == reqData.Uid);
            if (likeData == null)
            {
                return TypedResults.NotFound();
            }

            var affected = await db.Like
                .Where(model => model.Id == likeData.Id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteLike")
        .WithOpenApi();
    }
}
