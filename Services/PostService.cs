namespace backend.Services;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Models;

public class PostService : IPostService {
  private readonly IMongoCollection<Post> _postsCollection;

  public PostService(IOptions<PostDatastoreSettings> postDatastoreSettings) {
    var mongoClient = new MongoClient(postDatastoreSettings.Value.ConnectionString);
    var mongoDatabase = mongoClient.GetDatabase(postDatastoreSettings.Value.DatabaseName);
    _postsCollection = mongoDatabase.GetCollection<Post>(postDatastoreSettings.Value.PostsCollectionName);
  }

  public async Task<List<Post>> GetAsync() =>
    await _postsCollection.Find(_ => true).ToListAsync();

  public async Task<Post?> GetAsync(string id) =>
    await _postsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

  public async Task<Post?> GetAsyncByPermalink(string permalink) =>
    await _postsCollection.Find(x => x.Permalink == permalink).FirstOrDefaultAsync();

  public async Task CreateAsync(Post newPost) =>
    await _postsCollection.InsertOneAsync(newPost);

  public async Task<Post?> ReplaceAsync(string id, Post updatedPost) {
    if (await _postsCollection.Find(x => x.Id == id).FirstOrDefaultAsync() == null) return null;
    await _postsCollection.ReplaceOneAsync(x => x.Id == id, updatedPost);
    return await _postsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
  }

  public async Task<Post?> UpdateAsync(string id, Post patchedPost) {
    if (await _postsCollection.Find(x => x.Id == id).FirstOrDefaultAsync() == null) return null;

    UpdateDefinition<Post> update = Builders<Post>.Update.Combine();

    if (!patchedPost.Title.IsNullOrEmpty()) update = update.Set(x => x.Title, patchedPost.Title);
    if (!patchedPost.Excerpt.IsNullOrEmpty()) update = update.Set(x => x.Excerpt, patchedPost.Excerpt);
    if (!patchedPost.Content.IsNullOrEmpty()) update = update.Set(x => x.Content, patchedPost.Content);
    if (!patchedPost.Permalink.IsNullOrEmpty()) update = update.Set(x => x.Permalink, patchedPost.Permalink);

    update = update.Set(x => x.ModifiedOn, DateTime.Now);

    await _postsCollection.UpdateOneAsync(x => x.Id == id, update);

    return await _postsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
  }

  public async Task<DeleteResult> RemoveAsync(string id) =>
    await _postsCollection.DeleteOneAsync(x => x.Id == id);
}