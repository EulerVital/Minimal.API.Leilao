using Leilao.Repository;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModel;
using RegraNegocio;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseWebSockets();

app.MapGet(pattern: "v1/leilao/participantes", async ([FromQuery] int idLeilao, ApplicationDbContext dbContext, HttpContext context) =>
{
     if (!context.WebSockets.IsWebSocketRequest)
     {
         context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
         return;
     }

     using var webSocket = await context.WebSockets.AcceptWebSocketAsync();

     while (true)
     {
         var data = Encoding.ASCII.GetBytes($"Teste {DateTime.Now}");

         await webSocket.SendAsync(data, System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
         await Task.Delay(1000);
     }
});

app.MapGet("v1/leilao", (ApplicationDbContext dbContext) =>
{
    var regra = new RegraLeilao(dbContext);

    return Results.Ok(regra.GetListLeilao());
})
    .Produces<List<Models.Leilao>>(StatusCodes.Status200OK);


app.MapGet("v1/leilao/{id}", (int id, ApplicationDbContext dbContext) =>
{
    var regra = new RegraLeilao(dbContext);
    regra.GetLeilaoId(id);

    if (!string.IsNullOrEmpty(regra.Mensagem))
        return Results.BadRequest(new ErroViewModel(regra.Mensagem, 400));

    return Results.Ok(regra.LeilaoCriado);
})
    .Produces<Models.Leilao>(StatusCodes.Status200OK)
    .Produces<ErroViewModel>(StatusCodes.Status400BadRequest);


app.MapPost("v1/leilao", ([FromBody] LeiaoViewModel viewModel, ApplicationDbContext dbContext) =>
{
    if (string.IsNullOrEmpty(viewModel.Nome))
        return Results.BadRequest(new ErroViewModel("Nome precisa ser fornecido", 400));

    var criaLeilao = new RegraLeilao(dbContext);
    criaLeilao.Creator(viewModel.Nome);

    return Results.Created($"/v1/leilao/{criaLeilao.LeilaoCriado?.Id}", criaLeilao.LeilaoCriado);
})
    .Produces<Models.Leilao>(StatusCodes.Status201Created)
    .Produces<ErroViewModel>(StatusCodes.Status400BadRequest);

app.MapPost("v1/leilao/item", ([FromBody] ItemViewModel viewModel, ApplicationDbContext dbContext) =>
{
    viewModel = viewModel ?? new ItemViewModel(0);

    var criaLeiao = new RegraLeilao(dbContext);
    criaLeiao.AddItens(viewModel.IdLeilao);

    if (!string.IsNullOrEmpty(criaLeiao.Mensagem))
        return Results.BadRequest(new ErroViewModel(criaLeiao.Mensagem, 400));

    Models.Item retorno = criaLeiao.LeilaoCriado.Items.FirstOrDefault(f=>f.Leilao.Id == viewModel.IdLeilao) ?? new Models.Item();

    return Results.Created($"/v1/leilao/item/{retorno?.Id}", retorno);
})
    .Produces<Models.Item>(StatusCodes.Status201Created)
    .Produces<ErroViewModel>(StatusCodes.Status400BadRequest);

app.MapGet("v1/leilao/item/{id}", (int id, ApplicationDbContext dbContext) =>
{
    var regra = new RegraItens(dbContext);
    regra.GetItemId(id);

    if (!string.IsNullOrEmpty(regra.Mensagem))
        return Results.BadRequest(new ErroViewModel(regra.Mensagem, 400));

    return Results.Ok(regra.Item);
})
    .Produces<Models.Item>(StatusCodes.Status200OK)
    .Produces<ErroViewModel>(StatusCodes.Status400BadRequest);

app.MapPost("v1/leilao/item/usuario", ([FromBody] EntrarLeilaoViewModel viewModel, ApplicationDbContext dbContext) =>
{
    var criaUsuario = new RegraUsuario(dbContext);
    criaUsuario.EntrarNoItem(viewModel.UsuarioId, viewModel.ItemId);

    if (!string.IsNullOrEmpty(criaUsuario.Mensagem))
        return Results.BadRequest(new ErroViewModel(criaUsuario.Mensagem, 400));
    
    return Results.Created($"/v1/usuario/{viewModel.UsuarioId}", criaUsuario.Usuario);
})
    .Produces<Models.Usuario>(StatusCodes.Status200OK)
    .Produces<ErroViewModel>(StatusCodes.Status400BadRequest);

app.MapPost("v1/usuario", ([FromBody] UsuarioViewModel viewModel, ApplicationDbContext dbContext) =>
{
    var criaUsuario = new RegraUsuario(dbContext);
    criaUsuario.Creator(viewModel.NomeUser);

    if (!string.IsNullOrEmpty(criaUsuario.Mensagem))
        return Results.BadRequest(new ErroViewModel(criaUsuario.Mensagem, 400));

    return Results.Created($"/v1/leilao/usuario/{criaUsuario.Usuario}", criaUsuario.Usuario);
})
    .Produces<Models.Usuario>(StatusCodes.Status200OK)
    .Produces<ErroViewModel>(StatusCodes.Status400BadRequest);

app.MapGet("v1/usuario/{id}", (int id, [FromQuery] bool? isTrazerItens, ApplicationDbContext dbContext) =>
{
    var criaUsuario = new RegraUsuario(dbContext);
    criaUsuario.GetUsuarioId(id, isTrazerItens == null ? false : (bool)isTrazerItens);

    if (!string.IsNullOrEmpty(criaUsuario.Mensagem))
        return Results.BadRequest(new ErroViewModel(criaUsuario.Mensagem, 400));

    return Results.Ok(criaUsuario.Usuario);
})
    .Produces<Models.Usuario>(StatusCodes.Status200OK)
    .Produces<ErroViewModel>(StatusCodes.Status400BadRequest);

app.Run();