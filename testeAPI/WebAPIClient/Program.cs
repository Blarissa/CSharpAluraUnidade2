using System.Net.Http.Headers;
using System.Text.Json;
//Usando um HttpClient para manipular solicitações e respostas
using HttpClient client = new();
//Configura cabeçalhos HTTP de todas as solicitações:
//Um cabeçalho Accept: para aceitar respostas JSON
//Um cabeçalho User-Agent.
//Esses cabeçalhos são verificados pelo código do servidor GitHub
//e são necessários para recuperar informações do GitHub.
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

//capturar os resultados e gravar cada nome do repositório no console.
var repositories = await ProcessRepositoriesAsync(client);

//Exibir o nome de cada repositório
foreach (var repo in repositories ?? Enumerable.Empty<Repository>())
{    
    Console.WriteLine($"Name: {repo.Name}");
    Console.WriteLine($"Homepage: {repo.Homepage}");
    Console.WriteLine($"GitHub: {repo.GitHubHomeUrl}");
    Console.WriteLine($"Description: {repo.Description}");
    Console.WriteLine($"Watchers: {repo.Watchers:#,0}");
    Console.WriteLine($"Last push: {repo.LastPush}");
    Console.WriteLine();
}

//Aguarda a tarefa retornada do método de chamada
//HttpClient.GetStringAsync(String). Esse método envia
//uma solicitação HTTP GET para o URI especificado. O corpo
//da resposta é retornado como um String, que está disponível
//quando a tarefa é concluída. A cadeia de caracteres de
//json resposta é impressa no console.
static async Task<List<Repository>> ProcessRepositoriesAsync(HttpClient client)
{
    //var json = await client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");

    //O código atualizado substitui GetStringAsync(String) por
    //GetStreamAsync(String). Esse método de serializador usa um
    //fluxo, em vez de uma cadeia de caracteres, como sua fonte.
    await using Stream stream = await
        client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");

    //O primeiro argumento JsonSerializer.DeserializeAsync<TValue>
    //(Stream, JsonSerializerOptions, CancellationToken) é uma
    //expressão await.
    //O método DeserializeAsync é genérico, o que significa que você
    //fornece argumentos de tipo para que tipo de objetos devem
    //ser criados a partir do texto JSON.
    var repositories =
        await JsonSerializer.DeserializeAsync<List<Repository>>(stream);   
    
    //Retornando os repositórios depois de processar a resposta JSON
    return repositories ?? new();
}