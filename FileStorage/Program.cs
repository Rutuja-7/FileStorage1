using FileStorage.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBlobStorage, BlobStorage>();
builder.Services.AddScoped<IQueueStorage, QueueStorage>();
builder.Services.AddScoped<ITableStorage, TableStorage>();
builder.Services.AddScoped<IFileShareStorage, FileShareStorage>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
//{
	app.UseSwagger();
	app.UseSwaggerUI();
//}

app.UseAuthorization();

app.MapControllers();

app.Run();
