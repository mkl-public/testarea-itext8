using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using NUnit.Framework;

namespace iText8.Net_Playground.Form
{
    class SetCheckbox
    {
        /// <summary>
        /// Enabling a single checkbox rather than the entire checkbox within the same acrofield
        /// https://stackoverflow.com/questions/76307909/enabling-a-single-checkbox-rather-than-the-entire-checkbox-within-the-same-acrof
        /// 
        /// Indeed, all checkboxes are on.
        /// </summary>
        /// <see cref="SetCms1500"/>
        [Test]
        public void SetAndFlattenCms1500LikeAbhiShrestha()
        {
            Directory.CreateDirectory(@"..\..\..\target\test-outputs\form");

            using (PdfReader reader = new PdfReader(@"..\..\..\src\test\resources\mkl\testarea\itext8\form\form-cms1500.pdf"))
            using (PdfWriter writer = new PdfWriter(@"..\..\..\target\test-outputs\form\form-cms1500-SetAndFlattenedLikeAbhiShrestha.pdf"))
            using (PdfDocument pdfDoc = new PdfDocument(reader, writer))
            {
                PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
                IDictionary<string, PdfFormField> fields = form.GetAllFormFields();

                fields["insurance_type"].SetValue("Medicare");

                form.FlattenFields();
            }
        }

        /// <summary>
        /// Enabling a single checkbox rather than the entire checkbox within the same acrofield
        /// https://stackoverflow.com/questions/76307909/enabling-a-single-checkbox-rather-than-the-entire-checkbox-within-the-same-acrof
        /// 
        /// Before flattening one can identify the issue better - here also all check boxes are
        /// checked, and they are because all the AP entries are touched.
        /// </summary>
        /// <see cref="SetAndFlattenCms1500LikeAbhiShrestha"/>
        [Test]
        public void SetCms1500()
        {
            Directory.CreateDirectory(@"..\..\..\target\test-outputs\form");

            using (PdfReader reader = new PdfReader(@"..\..\..\src\test\resources\mkl\testarea\itext8\form\form-cms1500.pdf"))
            using (PdfWriter writer = new PdfWriter(@"..\..\..\target\test-outputs\form\form-cms1500-Set.pdf"))
            using (PdfDocument pdfDoc = new PdfDocument(reader, writer))
            {
                PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
                IDictionary<string, PdfFormField> fields = form.GetAllFormFields();

                fields["insurance_type"].SetValue("Medicare");
            }
        }
    }
}
