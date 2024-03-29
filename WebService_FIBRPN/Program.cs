using SeatReservation;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);

// Register the SoapCore library:
builder.Services.AddSoapCore();
// Register our Hello service implementing the IHello interface:
builder.Services.TryAddSingleton<ICinema, Cinema>();

var app = builder.Build();

// Specify the endpoint of our Hello service:
app.UseRouting();
app.UseEndpoints(endpoints => {
    endpoints.UseSoapEndpoint<ICinema>("/WebService_FIBRPN/Cinema", new SoapEncoderOptions(),
    SoapSerializer.DataContractSerializer);
});

app.Run();