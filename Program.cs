using CsvHelper;
using CsvHelper.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using WebApplicationforTest.DTOs;
using WebApplicationforTest.Profiles;
using System.Reflection;

using WebApplicationforTest.Repositories;
using WebApplicationforTest.Filters;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()     
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddScoped<IRevenueRepository, RevenueRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});
var app = builder.Build();

// 執行 CSV 匯入邏輯
await ImportCsvToDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();

// 匯入資料邏輯
async Task ImportCsvToDatabaseAsync()
{
    Console.WriteLine(" 匯入方法呼叫開始");

    var csvPath = Path.Combine(AppContext.BaseDirectory, "t187ap05_L.csv");

    var connectionString = @"Server=(localdb)\ProjectModels;Database=CompanyRevenueDb;Trusted_Connection=True;";

    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    var encoding = Encoding.GetEncoding("big5");

    using var reader = new StreamReader(csvPath, encoding);
    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        MissingFieldFound = null,
        BadDataFound = null
    };

    using var csv = new CsvReader(reader, config);
    var records = csv.GetRecords<MonthlyRevenueDto>().ToList();
    Console.WriteLine($" 共讀取 {records.Count} 筆資料");

    using var connection = new SqlConnection(connectionString);
    await connection.OpenAsync();

    foreach (var r in records)
    {
        try
        {
            using var cmd = new SqlCommand("sp_InsertMonthlyRevenue", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CompanyId", r.CompanyId ?? "");
            cmd.Parameters.AddWithValue("@CompanyName", r.CompanyName ?? "");
            cmd.Parameters.AddWithValue("@ReportYearMonth", r.ReportYearMonth ?? "");
            cmd.Parameters.AddWithValue("@IndustryCategory", r.IndustryCategory ?? "");
            cmd.Parameters.AddWithValue("@CurrentMonthRevenue", r.CurrentMonthRevenue);
            cmd.Parameters.AddWithValue("@PreviousMonthRevenue", r.PreviousMonthRevenue);
            cmd.Parameters.AddWithValue("@LastYearMonthRevenue", r.LastYearMonthRevenue);
            cmd.Parameters.AddWithValue("@MoMChangePercent", r.MoMChangePercent);
            cmd.Parameters.AddWithValue("@YoYChangePercent", r.YoYChangePercent);
            cmd.Parameters.AddWithValue("@AccumulatedRevenue", r.AccumulatedRevenue);
            cmd.Parameters.AddWithValue("@LastYearAccumulatedRevenue", r.LastYearAccumulatedRevenue);
            cmd.Parameters.AddWithValue("@AccumulatedChangePercent", r.AccumulatedChangePercent);
            cmd.Parameters.AddWithValue("@Note", r.Note ?? "");

            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($" 發生錯誤：{ex.Message}");
        }
    }

    Console.WriteLine("匯入完成！");
}
