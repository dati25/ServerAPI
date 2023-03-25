using SeverAPI.Database.Models;
using SeverAPI.Results.ReportResults;

namespace SeverAPI.Commands.ReportCommands
{
    public class ReportCommandPost : ICommand
    {
        public ReportResultPost Execute(ReportResultPost report)
        {
            this.context.Add(new Report(report.IdPc, report.Status, report.ReportTime, report.Description));
            this.context.SaveChanges();

            return report;
        }
    }
}
