using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout.Element;
using iText.Layout;
using NUnit.Framework;
using iText.Layout.Borders;

namespace iText8.Net_Playground.Content
{
    internal class PositionParagraph
    {
        /// <summary>
        /// c# iText7 how to set the position of the top left corner of a paragraph
        /// https://stackoverflow.com/questions/77493336/c-sharp-itext7-how-to-set-the-position-of-the-top-left-corner-of-a-paragraph
        /// 
        /// This shows how to position a paragraph with its top left  at 100, 100, at least
        /// as far iText measuring is concerned.
        /// </summary>
        [Test]
        public void PositionParagraphAt100_100()
        {
            Directory.CreateDirectory(@"..\..\..\target\test-outputs\content");

            var targetPdfPath = @"..\..\..\target\test-outputs\content\100x100.pdf";
            using (PdfDocument pdfDoc = new PdfDocument(new PdfWriter(targetPdfPath)))
            {
                Document document = new Document(pdfDoc);
                var page = pdfDoc.AddNewPage(PageSize.A4);
                Paragraph paragraph = new Paragraph("Must be at (100,100)");
                PdfFont FontHELVETICA = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                paragraph.SetFontSize(20f);
                var pageHeight = PageSize.A4.GetHeight();
                PdfCanvas pdfCanvas = new PdfCanvas(page);
                var rectHeight = 200f;
                var rectWidth = 200f;
                pdfCanvas.SetLineWidth(0.5f);
                pdfCanvas.Rectangle(100, pageHeight - 100 - rectHeight, 200, 200);//It is easy to place the rectangle in the position of (100,100)pt
                pdfCanvas.Stroke();

                Canvas canvas = new Canvas(pdfCanvas, PageSize.A4.ApplyMargins(100, 0, 0, 100, false));
                paragraph.SetMarginTop(0);
                paragraph.SetPaddingTop(0);
                paragraph.SetBorder(new SolidBorder(1));
                canvas.Add(paragraph);
            }
        }
    }
}
