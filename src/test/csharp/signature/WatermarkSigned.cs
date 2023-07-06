using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using NUnit.Framework;

namespace iText8.Net_Playground.Signature
{
    class WatermarkSigned
    {
        /// <summary>
        /// How to add a watermark in a digitally signed pdf using iText library without making signature invalid
        /// https://stackoverflow.com/questions/76612048/how-to-add-a-watermark-in-a-digitally-signed-pdf-using-itext-library-without-mak
        /// "Parent node.pdf", stamp.png
        /// https://github.com/nickosl50/itext_valid_sign
        /// 
        /// As already mentioned by the OP, the output of their code has invalidated signature and time stamp.
        /// The reason is that the code changes the static page content which is a disallowed change to signed
        /// PDFs. But see <see cref="WatermarkImproved"/>
        /// </summary>
        [Test]
        public void WatermarkLikeNickLazidis()
        {
            Directory.CreateDirectory(@"..\..\..\target\test-outputs\signature");

            float x = 10;
            float y = 200;
            float width = 400;
            float height = 200;

            using (PdfReader reader = new PdfReader(@"..\..\..\src\test\resources\mkl\testarea\itext8\signature\Parent node.pdf"))
            using (PdfWriter writer = new PdfWriter(@"..\..\..\target\test-outputs\signature\Parent node-WatermarkLikeNickLazidis.pdf"))
            using (PdfDocument pdfDoc = new PdfDocument(reader, writer, new StampingProperties().UseAppendMode()))
            {
                Document doc = new Document(pdfDoc);

                // Load the image
                PdfCanvas canvas = new PdfCanvas(pdfDoc.GetFirstPage(), true);
                Image image = new Image(ImageDataFactory.Create(@"..\..\..\src\test\resources\mkl\testarea\itext8\signature\stamp.png"));

                var page = pdfDoc.GetFirstPage();
                var pageRotation = page.GetRotation();
                var imageRotation = 0;

                switch (pageRotation)
                {
                    case 90:
                        var t1 = x;
                        x = page.GetPageSize().GetWidth() - y;
                        y = t1;
                        imageRotation = 270;
                        break;
                    case 180:
                        x = page.GetPageSize().GetWidth() - x;
                        y = page.GetPageSize().GetHeight() - y;
                        imageRotation = 180;
                        break;
                    case 270:
                        var t2 = x;
                        x = y;
                        y = page.GetPageSize().GetHeight() - t2;
                        imageRotation = 90;
                        break;
                }

                // Set the position and size of the image stamp
                image.SetFixedPosition(x, y);
                image.SetHeight(height);
                image.SetWidth(width);
                image.SetRotationAngle(-(Math.PI / 180) * imageRotation);

                // Add the image to the document
                doc.Add(image);

                doc.Close();
            }
        }

        /// <summary>
        /// How to add a watermark in a digitally signed pdf using iText library without making signature invalid
        /// https://stackoverflow.com/questions/76612048/how-to-add-a-watermark-in-a-digitally-signed-pdf-using-itext-library-without-mak
        /// "Parent node.pdf", stamp.png
        /// https://github.com/nickosl50/itext_valid_sign
        /// 
        /// In contrast to <see cref="WatermarkLikeNickLazidis"/> this method applies a watermark
        /// in an annotation. Consequentially, the signature is not invalidated.
        /// </summary>
        [Test]
        public void WatermarkImproved()
        {
            Directory.CreateDirectory(@"..\..\..\target\test-outputs\signature");

            float x = 10;
            float y = 200;
            float width = 400;
            float height = 200;

            using (PdfReader reader = new PdfReader(@"..\..\..\src\test\resources\mkl\testarea\itext8\signature\Parent node.pdf"))
            using (PdfWriter writer = new PdfWriter(@"..\..\..\target\test-outputs\signature\Parent node-WatermarkImproved.pdf"))
            using (PdfDocument pdfDoc = new PdfDocument(reader, writer, new StampingProperties().UseAppendMode()))
            {
                // Load the image
                ImageData imageData = ImageDataFactory.Create(@"..\..\..\src\test\resources\mkl\testarea\itext8\signature\stamp.png");

                PdfFormXObject appearanceXObject = new PdfFormXObject(new Rectangle(width, height));
                PdfCanvas canvas = new PdfCanvas(appearanceXObject, pdfDoc);
                canvas.AddImageAt(imageData, 0, 0, false);

                PdfDictionary watermarkDictionary = new PdfDictionary();
                watermarkDictionary.Put(PdfName.Type, PdfName.Annot);
                watermarkDictionary.Put(PdfName.Subtype, PdfName.Watermark);

                PdfAnnotation watermark = PdfAnnotation.MakeAnnotation(watermarkDictionary);
                watermark.SetRectangle(new PdfArray(new[] { x, y, x + width, y + height }));
                watermark.SetFlags(PdfAnnotation.LOCKED | PdfAnnotation.LOCKED_CONTENTS | PdfAnnotation.PRINT);
                watermark.SetNormalAppearance(appearanceXObject.GetPdfObject());

                var page = pdfDoc.GetFirstPage();
                page.AddAnnotation(watermark);
            }
        }
    }
}
