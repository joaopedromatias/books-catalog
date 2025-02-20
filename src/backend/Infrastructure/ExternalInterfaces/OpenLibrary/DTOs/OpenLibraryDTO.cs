using Newtonsoft.Json;

namespace ExternalInterfaces.OpenLibrary.DTOs;

public struct OpenLibraryBaseDTO 
{ 
    [JsonProperty("works")]
    public IEnumerable<OpenLibraryDTO> Works { get; set; }
}

public struct OpenLibraryDTO
{
    [JsonProperty("key")]
    public string Key { get; set; }
    [JsonProperty("title")]
    public string Title { get; set; }
    [JsonProperty("cover_id")]
    public int? CoverId { get; set; }
    [JsonProperty("first_publish_year")]
    public int PublishYear { get; set; }

    [JsonProperty("authors")]
    public IEnumerable<Authors> Authors { get; set; }
}

public struct Authors
{
    [JsonProperty("key")]   
    public string Key { get; set; }
    [JsonProperty("name")]   
    public string Name { get; set; }    
}
