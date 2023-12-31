﻿using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using mosaCupBackend.Data;
using mosaCupBackend.Models.DbModels;
using mosaCupBackend.Models.ReqModels;

namespace mosaCupBackend.Endpoints;

public static class UserDataEndpoints
{
    public static void MapUserDataEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/UserData").WithTags(nameof(userData));

        //Load user info
        group.MapGet("/{id}", async Task<Results<Ok<userData>, NotFound>> (string uid, mosaCupDbContext db) =>
        {
            return await db.UserData.AsNoTracking()
                .FirstOrDefaultAsync(model => model.DeletedAt == null && model.Uid == uid)
                is userData model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetUserDataById")
        .WithOpenApi();

        //Search user
        group.MapGet("/Search/{name}", async Task<Results<Ok<List<userData>>, NotFound>> (string name, mosaCupDbContext db) =>
        {
            if (!name.StartsWith("@"))
            {
                var userList = await db.UserData
                    .Where(model => model.DeletedAt == null && model.DisplayName.IndexOf(name) != -1)
                    .ToListAsync();
                return userList != null ? TypedResults.Ok(userList) : TypedResults.NotFound();
            }
            else
            {

                var userList = await db.UserData
                    .Where(model => model.Name.IndexOf(name.Replace("@", "")) != -1)
                    .ToListAsync();
                return userList != null ? TypedResults.Ok(userList) : TypedResults.NotFound();
            }
        })
        .WithName("SearchUserAsName")
        .WithOpenApi();

        //Judge Name is Available (Available -> 1/ Not available -> 0)
        group.MapGet("/JudgeAvailable/{name}", async Task<Results<Ok<int>, NotFound>> (string userName, mosaCupDbContext db) =>
        {
            return await db.UserData
                .FirstOrDefaultAsync(model => model.Name == userName)
                is userData model
                    ? TypedResults.Ok(0)
                    : TypedResults.Ok(1);
        });

        //Edit profile
        group.MapPut("/EditProfile", async Task<Results<Ok, NotFound>> (EditProfile userData, mosaCupDbContext db) =>
        {
            var affected = await db.UserData
                .Where(model => model.Uid == userData.Uid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.DisplayName, userData.DisplayName)
                  .SetProperty(m => m.Description, userData.Description)
                );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("EditProfile")
        .WithOpenApi();

        //Create user
        group.MapPost("/", async Task<Results<Ok, NotFound>> (UserDataReq reqData, mosaCupDbContext db) =>
        {
            var userdata = new userData
            {
                Uid = reqData.Uid,
                DisplayName = reqData.DisplayName,
                Name = reqData.Name,
                Description = reqData.Description,
            };

            db.UserData.Add(userdata);
            await db.SaveChangesAsync();
            return TypedResults.Ok();
        })
        .WithName("CreateUser")
        .WithOpenApi();

        //Delete user
        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (string uid, mosaCupDbContext db) =>
        {
            var affected = await db.UserData
                .Where(model => model.Uid == uid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.DeletedAt, DateTime.UtcNow)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteUser")
        .WithOpenApi();

        //Restore user
        group.MapGet("/Restore/{id}", async Task<Results<Ok, NotFound>> (string uid, mosaCupDbContext db) =>
        {
            var affected = await db.UserData
                .Where(model => model.Uid == uid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.DeletedAt, (DateTime?)null)
                );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("RestoreName")
        .WithOpenApi();
    }
}
