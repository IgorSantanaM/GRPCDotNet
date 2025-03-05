global using static Basics.FirstServiceDefinition;
using Auth;
using Basics;
using Grpc.Net.Compression;
using MVCClient.Interceptors;
using System.IO.Compression;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


// Add services to the container.
services.AddControllersWithViews();
services.AddTransient<ClientLoggerInteceptor>();
services.AddTransient<ServerLoggingInteceptor>();

services.AddGrpcClient<FirstServiceDefinitionClient>(opt =>
{
    opt.Address = new Uri("https://localhost:7057");
})
    .AddCallCredentials((context, metadata) =>
    {
        var token = JwtHelper.GenerateJwtToken("MVC");
        if (!string.IsNullOrEmpty(token))
        {
            metadata.Add("Authorization", $"Bearer {token}");
        }
        return Task.CompletedTask;
    })
    .AddInterceptor<ClientLoggerInteceptor>();


services.AddGrpc(opt =>
{
    opt.Interceptors.Add<ServerLoggingInteceptor>();
    opt.ResponseCompressionAlgorithm = "gzip";
    opt.ResponseCompressionLevel = CompressionLevel.SmallestSize;
    //opt.CompressionProviders = new List<ICompressionProvider>()
    //{
    //    new GzipCompressionProvider(CompressionLevel.SmallestSize);
    //};
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
