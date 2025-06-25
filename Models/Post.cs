using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace backend.Models;

public class Post {
  [BsonId]
  [BsonRepresentation(BsonType.ObjectId)]
  [JsonPropertyName("id")]
  public string? Id { get; set; }

  [BsonElement("title")]
  [JsonPropertyName("title")]
  public string? Title { get; set; }

  [BsonElement("created")]
  [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
  [JsonPropertyName("created")]
  public DateTime CreatedOn { get; set; }

  [BsonElement("modified")]
  [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
  [JsonPropertyName("modified")]
  public DateTime ModifiedOn { get; set; }

  [BsonElement("excerpt")]
  [JsonPropertyName("excerpt")]
  public string? Excerpt { get; set; }

  [BsonElement("content")]
  [JsonPropertyName("content")]
  public string? Content { get; set; }

  [BsonElement("slug")]
  [JsonPropertyName("slug")]
  public string? Slug { get; set; }
}
