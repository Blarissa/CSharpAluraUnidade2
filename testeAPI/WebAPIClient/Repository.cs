//O código anterior define uma classe para representar
//o objeto JSON retornado da API do GitHub. Você usará
//essa classe para exibir uma lista de nomes de repositório.
using System.Text.Json.Serialization;

/**Altera o nome da propriedade name para Name.
 * Adiciona o JsonPropertyNameAttribute para especificar 
 * como essa propriedade aparece no JSON.
 */
public record class Repository(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("html_url")] Uri GitHubHomeUrl,
    [property: JsonPropertyName("homepage")] Uri Homepage,
    [property: JsonPropertyName("watchers")] int Watchers,
    [property: JsonPropertyName("pushed_at")] DateTime LastPushUtc)
{
    public DateTime LastPush => LastPushUtc.ToLocalTime();
}