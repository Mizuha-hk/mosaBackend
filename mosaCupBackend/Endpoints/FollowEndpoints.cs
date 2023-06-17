using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using mosaCupBackend.Data;
using mosaCupBackend.Models.DbModels;
using mosaCupBackend.Models.ReqModels;

namespace mosaCupBackend.Endpoints;

public static class FollowEndpoints
{
    public static void MapFollowEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Follow").WithTags(nameof(follow));

        //Get following users
        group.MapGet("/{id}", async Task<Results<Ok<List<follow>>, NotFound>> (string uid, mosaCupBackendContext db) =>
        {
            var followUser = await db.Follow
                .Where(model => model.Uid == uid)
                .ToListAsync();
            return followUser != null ? TypedResults.Ok(followUser) : TypedResults.NotFound();
        })
        .WithName("GetFollowingUsers")
        .WithOpenApi();

        //Get followed users
        group.MapGet("/Followed/{id}", async Task<Results<Ok<List<follow>>, NotFound>> (string uid, mosaCupBackendContext db) =>
        {
            var followedUser = await db.Follow
                .Where(model => model.FollowedUid == uid)
                .ToListAsync();
            return followedUser != null ? TypedResults.Ok(followedUser) : TypedResults.NotFound();
        })
        .WithName("GetFollowedUsers")
        .WithOpenApi();


        //Judge Follow or UnFollow(following -> 1/ unfollowing -> 0)
        group.MapPost("/JudgeFollow", async Task<Results<Ok<int>, NotFound>> (FollowReq reqData, mosaCupBackendContext db) =>
        {
            return await db.Follow
                .Where(m => m.Uid == reqData.Uid)
                .FirstOrDefaultAsync(m => m.FollowedUid == reqData.FollowedUid)
                    is follow m
                    ? TypedResults.Ok(1)
                    : TypedResults.Ok(0);
        })
        .WithName("JudgeFollow")
        .WithOpenApi();

        //Follow user
        group.MapPost("/", async (FollowReq reqData, mosaCupBackendContext db) =>
        {
            var id = Guid.NewGuid();
            var follow = new follow { Id = id, Uid = reqData.Uid, FollowedUid = reqData.FollowedUid };
            db.Follow.Add(follow);
            var notification = new notification
            {
                Id = Guid.NewGuid(),
                Uid = reqData.FollowedUid,
                AffectUid = reqData.Uid,
                TypeCode = 0,
                Date = DateTime.UtcNow,
                Pid = null
            };
            db.Notification.Add(notification);

            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Follow/{follow.Id}", follow);
        })
        .WithName("FollowUser")
        .WithOpenApi();

        //UnFollow user
        group.MapPost("/Unfollow", async Task<Results<Ok, NotFound>> (FollowReq reqData, mosaCupBackendContext db) =>
        {
            var follow = await db.Follow
                .Where(model => model.Uid == reqData.Uid)
                .FirstOrDefaultAsync(model => model.FollowedUid == reqData.FollowedUid);
            if (follow == null)
            {
                return TypedResults.NotFound();
            }

            var affected = await db.Follow
                .Where(model => model.Id == follow.Id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UnFollowUser")
        .WithOpenApi();
    }
}
