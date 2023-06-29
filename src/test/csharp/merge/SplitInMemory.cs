using iText.Kernel.Pdf;
using NUnit.Framework;
using static iText8.Net_Playground.Merge.ByteArrayPdfSplitter;

namespace iText8.Net_Playground.Merge
{
    class SplitInMemory
    {
        /// <summary>
        /// Split a Pdf into byte array pages with IText7
        /// https://stackoverflow.com/questions/76575599/split-a-pdf-into-byte-array-pages-with-itext7
        /// 
        /// This is an improved version of the OP's method.
        /// </summary>
        public static List<Byte[]> SplitOnPages(Byte[] bytes)
        {
            List <byte[]> result = new List<byte[]>();
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                using (PdfReader reader = new PdfReader(memoryStream))
                {
                    PdfDocument docToSplit = new PdfDocument(reader);
                    ByteArrayPdfSplitter splitter = new ByteArrayPdfSplitter(docToSplit);
                    splitter.SplitByPageCount(1, new DocumentReadyListender(splitter, result));
                }
            }

            return result;
        }

        /// <summary>
        /// Split a Pdf into byte array pages with IText7
        /// https://stackoverflow.com/questions/76575599/split-a-pdf-into-byte-array-pages-with-itext7
        /// 
        /// Tests the improved <see cref="SplitOnPages"/> method.
        /// </summary>
        [Test]
        public void SplitForMatiasMasso()
        {
            Directory.CreateDirectory(@"..\..\..\target\test-outputs\merge");

            byte[] bytes = File.ReadAllBytes(@"..\..\..\src\test\resources\mkl\testarea\itext8\form\form-cms1500.pdf");
            List<byte[]> resultBytes = SplitOnPages(bytes);

            for (int i = 0; i < resultBytes.Count; i++)
            {
                File.WriteAllBytes(@"..\..\..\target\test-outputs\merge\form-cms1500-" + i + ".pdf", resultBytes[i]);
            }
        }
    }
}
