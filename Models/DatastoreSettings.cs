namespace backend.Models;

public class DatastoreSettings {
  public string ConnectionString { get; set; } = null!;

  public string DatabaseName { get; set; } = null!;

  public string PostsCollectionName { get; set; } = null!;
  
  public string FieldsCollectionName { get; set; } = null!;
}