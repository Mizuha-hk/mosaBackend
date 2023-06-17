using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using mosaCupBackend.Data;
using mosaCupBackend.Models.DbModels;
using mosaCupBackend.Models.ReqModels;

namespace mosaCupBackend.Endpoints;

public static class PostEndpoints
{
    public static void MapPostEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Post").WithTags(nameof(post));

        //get all
        group.MapGet("/", async (mosaCupBackendContext db) =>
        {
            return await db.Post
                .OrderByDescending(m => m.PostedDate)
                .ToListAsync();
        })
        .WithName("GetAllPosts")
        .WithOpenApi();

        //get 10 posts
        group.MapGet("/{page}", async (int page, mosaCupBackendContext db) =>
        {
            return await db.Post
                .OrderByDescending(m => m.PostedDate)
                .Skip(page * 10)
                .Take(10)
                .ToListAsync();
        })
        .WithName("GetPostsByPage")
        .WithOpenApi();

        //add Post / return JoyLevel
        group.MapPost("/", async (PostReq reqData, mosaCupBackendContext db) =>
        {
            var model = new JoyLevelMLModel.JoyLevelMLModel.ModelInput();
            model.Sentence = reqData.Content;
            var result = JoyLevelMLModel.JoyLevelMLModel.Predict(model);

            var Post = new post
            {
                Id = Guid.NewGuid(),
                Uid = reqData.Uid,
                Content = reqData.Content,
                PostedDate = DateTime.UtcNow,
                ReplyId = reqData.ReplyId,
                JoyLevel = Convert.ToInt32(result.Avg__Readers_Joy)
            };
            db.Post.Add(Post);
            await db.SaveChangesAsync();
            return Results.Ok(Post.JoyLevel);
        })
        .WithName("CreatePost")
        .WithOpenApi();

        //Delete Post
        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (Guid id, mosaCupBackendContext db) =>
        {
            var affected = await db.Post
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeletePost")
        .WithOpenApi();
    }
}
