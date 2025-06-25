using backend.Models;
using MongoDB.Driver;

namespace backend.Services;

public interface IPostService {
  Task<List<Post>> GetAsync();

  Task<Post?> GetAsync(string id);
  
  Task<Post?> GetAsyncBySlug(string slug);

  Task CreateAsync(Post newPost);

  Task<Post?> ReplaceAsync(string id, Post updatedPost);

  Task<Post?> UpdateAsync(string id, Post updatedPost);

  Task<DeleteResult> RemoveAsync(string id);
}