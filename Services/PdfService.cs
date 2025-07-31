using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SmartRoom.Dtos;

namespace SmartRoom.Services
{
    public interface IPdfService
    {
        byte[] GenerateMoMPdf(MoMDto momDto);
    }

    public class PdfService : IPdfService
    {
        public byte[] GenerateMoMPdf(MoMDto momDto)
        {
            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);

                    page.Header()
                        .Text($"Minutes of Meeting: {momDto.MeetingTitle}")
                        .FontSize(24)
                        .Bold()
                        .FontColor(Colors.Blue.Medium);

                    page.Content().Column(column =>
                    {
                        column.Item().Text($"Summary: {momDto.Summary}").FontSize(14).Bold();
                        column.Item().Text($"Notes: {momDto.Notes}").FontSize(12);
                        column.Item().Text($"Created By: {momDto.CreatedByName}").FontSize(12);
                        column.Item().Text($"Created At: {momDto.CreatedAt}").FontSize(12);

                        column.Item().Text("Action Items:").FontSize(16).Bold().Underline();

                        if (momDto.ActionItems == null || !momDto.ActionItems.Any())
                        {
                            column.Item().Text("No action items available.");
                        }
                        else
                        {
                            foreach (var ai in momDto.ActionItems)
                            {
                                column.Item().Text($"{ai.Description} (Assigned to: {ai.AssignedToName})");
                                if (!string.IsNullOrWhiteSpace(ai.DiscussionPoint))
                                    column.Item().Text($"Discussion: {ai.DiscussionPoint}").Italic();

                                if (!string.IsNullOrWhiteSpace(ai.Decision))
                                    column.Item().Text($"Decision: {ai.Decision}").Italic();

                                column.Item().Text($"Completed: {(ai.IsCompleted ? "Yes" : "No")}");
                                if (ai.DueDate.HasValue)
                                    column.Item().Text($"Due Date: {ai.DueDate.Value.ToShortDateString()}");
                            }
                        }

                    });

                    page.Footer()
                        .AlignCenter()
                        .Text($"Generated on {DateTime.UtcNow:yyyy-MM-dd HH:mm}");
                    
                });
            });

            return document.GeneratePdf();
        }
    }
}
