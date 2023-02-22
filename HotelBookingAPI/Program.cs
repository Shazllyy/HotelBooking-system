using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.EntityFrameworkCore;
using HotelBookingDAL.Repositories;
using HotelBookingDAL.Models;
using HotelBookingDAL.Contracts;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddDbContext<HotelBookingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//DI for all repositories
builder.Services.AddScoped<IRepository<room>,RepositoryRooms>();
builder.Services.AddScoped<IRepository<user>, RepositoryUsers>();
builder.Services.AddScoped<IRepository<branch>, RepositoryBranches>();
builder.Services.AddScoped<IResident<resident>, RepositoryResident>();
builder.Services.AddScoped<IRepository<resident_room>, RepositoryResidentRooms>();

builder.Services.AddScoped<IRoomReserve<room>, RepositoryRooms>();
builder.Services.AddScoped<IRoomCancelReserve<room>, RepositoryRooms>();
builder.Services.AddScoped<IRoomCancelReserve<room>, RepositoryRooms>();
builder.Services.AddScoped<IRoomFindAvailable<room>, RepositoryRooms>();
builder.Services.AddScoped<ILogin<user>, RepositoryUsers>();
builder.Services.AddScoped<IGetResidentRoooms<resident_room>, RepositoryResidentRooms>();
builder.Services.AddScoped<ICheckRoomNumbers<room>, RepositoryRooms>();
builder.Services.AddScoped<IReservationData<ReservationData>, RepositoryReservationData>();

//END DI
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(/*c => c.UseDateOnlyTimeOnlyStringConverters()*/);

// for solving support for DateOnly 
//builder.Services
//    .AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters())
//    .AddJsonOptions(options => options.UseDateOnlyTimeOnlyStringConverters());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{    
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
