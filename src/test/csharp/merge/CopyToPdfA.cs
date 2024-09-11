using iText.Kernel.Pdf;
using iText.Pdfa;
using NUnit.Framework;

namespace iText8.Net_Playground.Merge
{
    class CopyToPdfA
    {
        /// <summary>
        /// Apply PDFA Conformance Level using itext7 Unexpected end of file
        /// https://stackoverflow.com/questions/78969065/apply-pdfa-conformance-level-using-itext7-unexpected-end-of-file
        /// Invoice 1202.pdf
        /// https://filetransfer.io/data-package/rrE6UFNm#link
        /// 
        /// The "Unexpected end of file" exception occurs during PDF/A compliance tests as those tests
        /// cannot handle partial page content streams cut inside a marked content dictionary, the parser
        /// expects complete dictionaries.
        /// </summary>
        [Test]
        public void CopyLikeGuttoFreitas()
        {
            PdfAConformanceLevel pdfAConformanceLevel = PdfAConformanceLevel.PDF_A_2A;

            using (PdfReader pdfReader = new PdfReader(@"..\..\..\src\test\resources\mkl\testarea\itext8\merge\Invoice 1202.pdf"))
            using (PdfWriter writer = new PdfWriter(@"..\..\..\target\test-outputs\merge\Invoice 1202-CopyLikeGuttoFreitas.pdf"))
            using (PdfADocument pdfADoc = new PdfADocument(writer, pdfAConformanceLevel, CreateOutputIntent(false)))
            {
                pdfADoc.GetDocumentInfo().SetCreator("Scan2x");

                using (PdfDocument existingDoc = new PdfDocument(pdfReader))
                {
                    int numberOfPages = existingDoc.GetNumberOfPages();

                    for (int i = 1; i <= numberOfPages; i++)
                    {
                        PdfPage page = existingDoc.GetPage(i);

                        //PdfDictionary pageDictionary = page.GetPdfObject();

                        pdfADoc.AddPage(page.CopyTo(pdfADoc));
                    }

                    //existingDoc.CopyPagesTo(1, existingDoc.GetNumberOfPages(), pdfADoc);

                    pdfADoc.SetTagged();
                    pdfADoc.GetCatalog().SetLang(new PdfString("en-US"));
                    pdfADoc.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));
                }
            }
        }

        /// <see cref="CopyLikeGuttoFreitas"/>
        private static PdfOutputIntent CreateOutputIntent(bool hasCmykOutputIntent)
        {
            string exeDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string colourProfilePath = "";
            if (hasCmykOutputIntent)
                colourProfilePath = Path.Combine(exeDirectory, "CMYK.icc");
            else
                colourProfilePath = Path.Combine(exeDirectory, @"..\..\..\src\test\resources\mkl\testarea\itext8\merge\sRGBColor.icm");

            Stream iccProfileStream = File.OpenRead(colourProfilePath);
            return new PdfOutputIntent("Custom", "", "http://www.color.org", hasCmykOutputIntent ? "CMYK" : "sRGB IEC61966-2.1", iccProfileStream);
        }
    }
}
