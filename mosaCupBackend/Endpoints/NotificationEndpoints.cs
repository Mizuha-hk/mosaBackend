using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using mosaCupBackend.Data;
using mosaCupBackend.Models.DbModels;
namespace mosaCupBackend.Endpoints;

public static class NotificationEndpoints
{
    public static void MapNotificationEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Notification").WithTags(nameof(notification));

        //Get user notification
        group.MapPost("/{page}", async Task<Results<Ok<List<notification>>, NotFound>> (int page, string uid, mosaCupBackendContext db) =>
        {
            return await db.Notification.AsNoTracking()
                .OrderByDescending(m => m.Date)
                .Where(model => model.Uid == uid)
                .ToListAsync()
                is List<notification> model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetUserNotification")
        .WithOpenApi();
    }
}
