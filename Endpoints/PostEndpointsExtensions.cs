using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;
using backend.Models;
using backend.Services;

namespace backend.endpoints;

public static class PostEndpointsExtensions {
  public static void MapEndpointsPost(this WebApplication app) {
    app.MapGet("/posts", async (IPostService postService) => TypedResults.Ok(await postService.GetAsync()))
      .WithName("GetPosts")
      .WithOpenApi(x => new OpenApiOperation(x)
      {
        Summary = "GetPosts",
        Description = "Returns all posts.",
        Tags = new List<OpenApiTag> { new() { Name = "Posts" } }
      });

    app.MapGet("/posts/id/{id}", async Task<Results<Ok<Post>, NotFound>> (IPostService postService, string id) =>
        await postService.GetAsync(id) is { } post
          ? TypedResults.Ok(post)
          : TypedResults.NotFound()
      ).WithName("GetPostById")
      .WithOpenApi(x => new OpenApiOperation(x)
      {
        Summary = "GetPostById",
        Description = "Returns single post by id",
        Tags = new List<OpenApiTag> { new() { Name = "Posts" } }
      });
    
    app.MapGet("/posts/{permalink}", async Task<Results<Ok<Post>, NotFound>> (IPostService postService, string permalink) =>
        await postService.GetAsyncByPermalink(permalink) is { } post
          ? TypedResults.Ok(post)
          : TypedResults.NotFound()
      ).WithName("GetPostByPermalink")
      .WithOpenApi(x => new OpenApiOperation(x)
      {
        Summary = "GetPostByPermalink",
        Description = "Returns single post by permalink",
        Tags = new List<OpenApiTag> { new() { Name = "Posts" } }
      });

    app.MapPost("/posts", async (IPostService postService, Post post) => {
          await postService.CreateAsync(post);
          return TypedResults.Created();
        }
      ).WithName("CreatePost")
      .WithOpenApi(x => new OpenApiOperation(x)
      {
        Summary = "CreatePost",
        Description = "Creates new post",
        Tags = new List<OpenApiTag> { new() { Name = "Posts" } }
      });

    app.MapPut("/posts/{id}",
        async Task<Results<Ok<Post>, NotFound>> (IPostService postService, string id, Post newPost) =>
          await postService.ReplaceAsync(id, newPost) is { } post
            ? TypedResults.Ok(post)
            : TypedResults.NotFound()
      )
      .WithName("ReplacePost")
      .WithOpenApi(x => new OpenApiOperation(x)
      {
        Summary = "ReplacePost",
        Description = "Replaces an existing post",
        Tags = new List<OpenApiTag> { new() { Name = "Posts" } }
      });

    app.MapPatch("/posts/{id}",
        async Task<Results<Ok<Post>, NotFound>> (IPostService postService, string id, Post patchedPost) =>
          await postService.UpdateAsync(id, patchedPost) is { } post
            ? TypedResults.Ok(post)
            : TypedResults.NotFound()
      )
      .WithName("UpdatePost")
      .WithOpenApi(x => new OpenApiOperation(x)
      {
        Summary = "UpdatePost",
        Description = "Updates an existing post",
        Tags = new List<OpenApiTag> { new() { Name = "Posts" } }
      });

    app.MapDelete("/posts/{id}", async Task<Results<Ok, NotFound>> (IPostService postService, string id) =>
        (await postService.RemoveAsync(id)).DeletedCount > 0
          ? TypedResults.Ok()
          : TypedResults.NotFound()
      )
      .WithName("DeletePost")
      .WithOpenApi(x => new OpenApiOperation(x)
      {
        Summary = "DeletePost",
        Description = "Deletes an existing post",
        Tags = new List<OpenApiTag> { new() { Name = "Posts" } }
      });
  }
}