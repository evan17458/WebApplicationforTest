using System.Data;
using WebApplicationforTest.DTOs;

namespace WebApplicationforTest.Helpers
{
    public static class CsvHelperExtensions
    {
        public static DataTable ToDataTable(this List<MonthlyRevenueDto> records)
        {
            var table = new DataTable();
            table.Columns.Add("CompanyId", typeof(string));
            table.Columns.Add("CompanyName", typeof(string));
            table.Columns.Add("ReportYearMonth", typeof(string));
            table.Columns.Add("IndustryCategory", typeof(string));
            table.Columns.Add("CurrentMonthRevenue", typeof(string));
            table.Columns.Add("PreviousMonthRevenue", typeof(string));
            table.Columns.Add("LastYearMonthRevenue", typeof(string));
            table.Columns.Add("MoMChangePercent", typeof(string));
            table.Columns.Add("YoYChangePercent", typeof(string));
            table.Columns.Add("AccumulatedRevenue", typeof(string));
            table.Columns.Add("LastYearAccumulatedRevenue", typeof(string));
            table.Columns.Add("AccumulatedChangePercent", typeof(string));
            table.Columns.Add("Note", typeof(string));

            foreach (var r in records)
            {
                table.Rows.Add(
                    r.CompanyId ?? "",
                    r.CompanyName ?? "",
                    r.ReportYearMonth ?? "",
                    r.IndustryCategory ?? "",
                    r.CurrentMonthRevenue ?? "",
                    r.PreviousMonthRevenue ?? "",
                    r.LastYearMonthRevenue ?? "",
                    r.MoMChangePercent ?? "",
                    r.YoYChangePercent ?? "",
                    r.AccumulatedRevenue ?? "",
                    r.LastYearAccumulatedRevenue ?? "",
                    r.AccumulatedChangePercent ?? "",
                    r.Note ?? ""
                );
            }

            return table;
        }
    }
}
