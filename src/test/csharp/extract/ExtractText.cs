using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using NUnit.Framework;
using System;

namespace iText8.Net_Playground.Extract
{
    internal class ExtractText
    {
        /// <summary>
        /// Extracting text using iText7 throws exception
        /// https://stackoverflow.com/questions/78515947/extracting-text-using-itext7-throws-exception
        /// 
        /// Indeed, this code throws the error indicated by the OP. It is caused by iText
        /// assuming an "EI" sequence inside the Ascii85-encoded data to already be the
        /// end-of-inline-image marker.
        /// 
        /// A deeper analysis shows that before commit 0e44a96b2f3b90fb6656310d2c0f5615b05d4391 
        /// iText would not have fallen for this wrong marker. In that commit, though, iText
        /// stopped insisting on a leading white space before the marker.
        /// 
        /// To fix this, one has to patch the iText content stream parsing classes or repair a
        /// copy of them. As a work-around one may pre-process the PDF content streams to not
        /// contain such problem inline image streams.
        /// </summary>
        [Test]
        public void testExtractLikeAndrus()
        {
            string pdfPath = @"..\..\..\src\test\resources\mkl\testarea\itext8\extract\aripaev.pdf";
            using (var reader = new PdfReader(pdfPath))
            using (var pdfDocument = new PdfDocument(reader))
            {
                for (int i = 1; i <= pdfDocument.GetNumberOfPages(); ++i)
                {
                    var strategy = new LocationTextExtractionStrategy();
                    var page = pdfDocument.GetPage(i);
                    var text = PdfTextExtractor.GetTextFromPage(page, strategy);

                    Console.WriteLine(text);
                }
            }
        }
    }
}
