using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using System.Collections.Generic;

namespace iText8.Net_Playground.Merge
{
    /// <summary>
    /// Split a Pdf into byte array pages with IText7
    /// https://stackoverflow.com/questions/76575599/split-a-pdf-into-byte-array-pages-with-itext7
    /// 
    /// This is an improved version of the OP's class.
    /// </summary>
    class ByteArrayPdfSplitter : PdfSplitter
    {
        private MemoryStream currentOutputStream = null;

        public ByteArrayPdfSplitter(PdfDocument pdfDocument) : base(pdfDocument)
        {
        }

        protected override PdfWriter GetNextPdfWriter(PageRange documentPageRange)
        {
            currentOutputStream = new MemoryStream();
            return new PdfWriter(currentOutputStream);
        }

        public MemoryStream CurrentMemoryStream
        {
            get { return currentOutputStream; }
        }

        public class DocumentReadyListender : IDocumentReadyListener
        {
            public List<byte[]> splitPdfs;

            private ByteArrayPdfSplitter splitter;

            public DocumentReadyListender(ByteArrayPdfSplitter splitter, List<byte[]> results)
            {
                this.splitter = splitter;
                this.splitPdfs = results;
            }

            public void DocumentReady(PdfDocument pdfDocument, PageRange pageRange)
            {
                pdfDocument.Close();
                byte[] contents = splitter.CurrentMemoryStream.ToArray();
                splitPdfs.Add(contents);
            }
        }
    }
}
