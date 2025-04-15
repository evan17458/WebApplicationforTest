using CsvHelper;
using CsvHelper.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using WebApplicationforTest.DTOs;
using WebApplicationforTest.Profiles;
using System.Reflection;
using WebApplicationforTest.Helpers; // ← 記得引入 namespace
using WebApplicationforTest.Repositories;
using WebApplicationforTest.Filters;
using System.Data;

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

    // 🟡 步驟 1：取得資料庫中已存在的 (CompanyId, ReportYearMonth)
    var existingKeys = new HashSet<string>();

    using (var connection = new SqlConnection(connectionString))
    {
        await connection.OpenAsync();
        var query = "SELECT CompanyId, ReportYearMonth FROM MonthlyRevenue";
        using var command = new SqlCommand(query, connection);
        using var reader2 = await command.ExecuteReaderAsync();

        while (await reader2.ReadAsync())
        {
            var key = $"{reader2["CompanyId"]}_{reader2["ReportYearMonth"]}";
            existingKeys.Add(key);
        }
    }

    // 🟡 步驟 2：排除重複
    var newRecords = records
        .Where(r => !existingKeys.Contains($"{r.CompanyId}_{r.ReportYearMonth}"))
        .ToList();

    Console.WriteLine($" 原始 CSV 筆數：{records.Count}");
    Console.WriteLine($" 可匯入筆數（已排除重複）：{newRecords.Count}");

    if (newRecords.Count == 0)
    {
        Console.WriteLine(" 無資料需要匯入。");
        return;
    }

    // 🟡 步驟 3：轉換成 DataTable
    var dataTable = records.ToDataTable(); // ← 改用擴充方法

    // 🟡 步驟 4：呼叫 TVP 預存程序
    using var insertConn = new SqlConnection(connectionString);
    await insertConn.OpenAsync();

    using var command3 = new SqlCommand("sp_BulkInsertMonthlyRevenue", insertConn)
    {
        CommandType = CommandType.StoredProcedure
    };

    var tvpParameter = new SqlParameter
    {
        ParameterName = "@Revenues",
        SqlDbType = SqlDbType.Structured,
        TypeName = "dbo.MonthlyRevenueType",
        Value = dataTable
    };

    command3.Parameters.Add(tvpParameter);
    await command3.ExecuteNonQueryAsync();

    Console.WriteLine(" 匯入完成！");
}
