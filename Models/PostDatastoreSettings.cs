namespace backend.Models;

public class PostDatastoreSettings {
  public string ConnectionString { get; set; } = null!;

  public string DatabaseName { get; set; } = null!;

  public string PostsCollectionName { get; set; } = null!;
}