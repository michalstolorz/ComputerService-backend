using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ComputerService.Core.Dto.Request;
using ComputerService.Core.Interfaces.Services;
using ComputerService.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ComputerService.Core.Dto.Response;
using Microsoft.AspNetCore.Hosting;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using Syncfusion.Pdf.Grid;
using ComputerService.Core.Models;
using ComputerService.Core.Interfaces.Repositories;

namespace ComputerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IRepairService _repairService;
        private readonly IRepairRepository _repairRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public InvoiceController(IInvoiceService invoiceService, IRepairService repairService, IRepairRepository repairRepository, IWebHostEnvironment webHostEnvironment)
        {
            _invoiceService = invoiceService;
            _repairService = repairService;
            _repairRepository = repairRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repairId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("createInvoice/{repairId}")]
        //[Authorize(Roles = "Admin, Boss")]
        public async Task<IActionResult> CreateInvoice(int repairId, CancellationToken cancellationToken)
        {
            //Create PDF with PDF/A-3b conformance.
            PdfDocument document = new PdfDocument(PdfConformanceLevel.Pdf_A3B);
            //Set ZUGFeRD profile.
            document.ZugferdConformanceLevel = ZugferdConformanceLevel.Basic;

            //Create border color.
            PdfColor borderColor = new PdfColor(Color.FromArgb(255, 142, 170, 219));
            PdfBrush lightBlueBrush = new PdfSolidBrush(new PdfColor(Color.FromArgb(255, 91, 126, 215)));

            PdfBrush darkBlueBrush = new PdfSolidBrush(new PdfColor(Color.FromArgb(255, 65, 104, 209)));

            PdfBrush whiteBrush = new PdfSolidBrush(new PdfColor(Color.FromArgb(255, 255, 255, 255)));
            PdfPen borderPen = new PdfPen(borderColor, 1f);

            string path = _webHostEnvironment.ContentRootPath + "/arial.ttf";
            Stream fontStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);

            //Create TrueType font.
            PdfTrueTypeFont headerFont = new PdfTrueTypeFont(fontStream, 30, PdfFontStyle.Regular);
            PdfTrueTypeFont arialRegularFont = new PdfTrueTypeFont(fontStream, 9, PdfFontStyle.Regular);
            PdfTrueTypeFont arialBoldFont = new PdfTrueTypeFont(fontStream, 11, PdfFontStyle.Regular);

            const float margin = 30;
            const float lineSpace = 7;
            const float headerHeight = 90;

            //Add page to the PDF.
            PdfPage page = document.Pages.Add();

            PdfGraphics graphics = page.Graphics;

            //Get the page width and height.
            float pageWidth = page.GetClientSize().Width;
            float pageHeight = page.GetClientSize().Height;
            //Draw page border
            graphics.DrawRectangle(borderPen, new RectangleF(0, 0, pageWidth, pageHeight));

            //Fill the header with light Brush.
            graphics.DrawRectangle(lightBlueBrush, new RectangleF(0, 0, pageWidth, headerHeight));

            RectangleF headerAmountBounds = new RectangleF(400, 0, pageWidth - 400, headerHeight);

            graphics.DrawString("INVOICE", headerFont, whiteBrush, new PointF(margin, headerAmountBounds.Height / 3));

            graphics.DrawRectangle(darkBlueBrush, headerAmountBounds);

            graphics.DrawString("Amount", arialRegularFont, whiteBrush, headerAmountBounds, new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle));

            var repair = await _repairService.GetRepairAsync(repairId, cancellationToken);

            PdfTextElement textElement = new PdfTextElement("Invoice Number: " + repair.Id, arialRegularFont);

            PdfLayoutResult layoutResult = textElement.Draw(page, new PointF(headerAmountBounds.X - (margin + 10), 120));

            textElement.Text = "Date : " + DateTime.Now.ToString("dddd dd, MMMM yyyy");
            textElement.Draw(page, new PointF(layoutResult.Bounds.X, layoutResult.Bounds.Bottom + lineSpace));

            textElement.Text = "Bill To:";
            layoutResult = textElement.Draw(page, new PointF(margin, 120));

            textElement.Text = repair.CustomerFirstName + " " + repair.CustomerLastName;
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));
            textElement.Text = repair.CustomerEmail;
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));
            textElement.Text = repair.CustomerPhoneNumber;
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));

            PdfGrid grid = new PdfGrid();

            List<CreateInvoicePDFModel> model = new List<CreateInvoicePDFModel>();
            int i = 1;
            const decimal TaxNetValueValue = 0.77M;
            const decimal VATValue = 0.23M;
            decimal summaryPartsPrice = 0;
            foreach (var u in repair.UsedParts)
            {
                decimal TaxValueForList1 = u.PartBoughtPrice * u.Quantity * VATValue;
                decimal NetPriceForList1 = u.PartBoughtPrice * TaxNetValueValue;
                decimal NetValueForList1 = u.PartBoughtPrice * u.Quantity * TaxNetValueValue;
                TaxValueForList1 = Math.Round(TaxValueForList1, 2);
                NetPriceForList1 = Math.Round(NetPriceForList1, 2);
                NetValueForList1 = Math.Round(NetValueForList1, 2);
                model.Add(new CreateInvoicePDFModel
                {
                    Id = i,
                    Name = u.Name,
                    Quantity = (int)u.Quantity,
                    NetPrice = NetPriceForList1,
                    NetValue = NetValueForList1,
                    Tax = (VATValue * 100).ToString() + "%",
                    TaxValue = TaxValueForList1,
                    SummaryPrice = u.PartBoughtPrice * u.Quantity
                });

                summaryPartsPrice += u.PartBoughtPrice * u.Quantity;
                i++;
            }
            string allRepairNames = "";
            foreach (var repairType in repair.RepairTypes)
                allRepairNames += repairType.Name + "\n";

            decimal TaxValueForList2 = repair.RepairCost * VATValue;
            decimal NetPriceForList2 = repair.RepairCost * TaxNetValueValue;
            decimal NetValueForList2 = repair.RepairCost * TaxNetValueValue;
            TaxValueForList2 = Math.Round(TaxValueForList2, 2);
            NetPriceForList2 = Math.Round(NetPriceForList2, 2);
            NetValueForList2 = Math.Round(NetValueForList2, 2);
            model.Add(new CreateInvoicePDFModel
            {
                Id = i,
                Name = allRepairNames,
                Quantity = 1,
                NetPrice = NetPriceForList2,
                NetValue = NetValueForList2,
                Tax = (VATValue * 100).ToString() + "%",
                TaxValue = TaxValueForList2,
                SummaryPrice = repair.RepairCost
            });

            grid.DataSource = model;

            grid.Columns[1].Width = 150;
            grid.Style.Font = arialRegularFont;
            grid.Style.CellPadding.All = 5;

            grid.ApplyBuiltinStyle(PdfGridBuiltinStyle.ListTable4Accent5);

            layoutResult = grid.Draw(page, new PointF(0, layoutResult.Bounds.Bottom + 40));

            textElement.Text = "Grand Total: ";
            textElement.Font = arialBoldFont;
            layoutResult = textElement.Draw(page, new PointF(headerAmountBounds.X - 40, layoutResult.Bounds.Bottom + lineSpace));

            decimal totalAmount = repair.RepairCost + summaryPartsPrice;

            textElement.Text = totalAmount.ToString() + "PLN";
            layoutResult = textElement.Draw(page, new PointF(layoutResult.Bounds.Right + 4, layoutResult.Bounds.Y));

            graphics.DrawString(totalAmount.ToString() + "PLN", arialBoldFont, whiteBrush, new RectangleF(400, lineSpace, pageWidth - 400, headerHeight + 15), new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle));


            borderPen.DashStyle = PdfDashStyle.Custom;
            borderPen.DashPattern = new float[] { 3, 3 };

            PdfLine line = new PdfLine(borderPen, new PointF(0, 0), new PointF(pageWidth, 0));
            layoutResult = line.Draw(page, new PointF(0, pageHeight - 100));

            textElement.Text = "Computer Services Adam Paluch";
            textElement.Font = arialRegularFont;
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + (lineSpace * 3)));
            textElement.Text = "ul. Krakowska 19/2\n" +
                "02-20 Warsaw\n" +
                "NIP: 8127749027";
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));
            textElement.Text = "Any Questions? adampaluch@computer-service.com";
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));

            var fileName = "Invoice" + repair.Id + ".pdf";
            FileStream fileStream = new FileStream(fileName, FileMode.CreateNew, FileAccess.ReadWrite);
            document.Save(fileStream);
            var filePath = fileStream.Name;
            document.Close(true);
            fileStream.Close();

            var invoiceId = await _invoiceService.AddInvoiceAsync(filePath, cancellationToken);

            var repairToUpdate = await _repairRepository.GetByIdAsync(repairId, cancellationToken);

            repairToUpdate.Status = EnumStatus.Finished;
            repairToUpdate.FinishDateTime = DateTime.Now;
            repairToUpdate.InvoiceId = invoiceId;
            await _repairRepository.UpdateAsync(cancellationToken, repairToUpdate);

            return Ok();
        }
    }
}
