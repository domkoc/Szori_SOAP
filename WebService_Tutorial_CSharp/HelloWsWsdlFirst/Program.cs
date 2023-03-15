using HelloWsWsdlFirst;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);

// Register the SoapCore library:
builder.Services.AddSoapCore();
// Register our Hello service implementing the IHello interface:
builder.Services.TryAddSingleton<IHello, Hello>();

var app = builder.Build();

// Specify the endpoint of our Hello service:
app.UseRouting();
app.UseEndpoints(endpoints => {
    endpoints.UseSoapEndpoint<IHello>("/HelloService", new SoapEncoderOptions(),
    SoapSerializer.DataContractSerializer);
});

app.Run();
