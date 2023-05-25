using iText.Forms.Fields;
using iText.Forms;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using NUnit.Framework;

namespace iText8.Net_Playground.Signature
{
    class MultipleSignatureFields
    {
        /// <summary>
        /// itext lock some fields after sign - one field ok but second field sign cause error?
        /// https://stackoverflow.com/questions/76252293/itext-lock-some-fields-after-sign-one-field-ok-but-second-field-sign-cause-err
        /// 
        /// This is the OP's code. Indeed, Adobe Acrobat has issues signing both signature fields
        /// in the same session.
        /// </summary>
        /// <see cref="CreateLikeD00larPlusType"/>
        [Test]
        public void CreateLikeD00lar()
        {
            Directory.CreateDirectory(@"..\..\..\target\test-outputs\signature");

            PdfWriter writer = new PdfWriter(@"..\..\..\target\test-outputs\signature\AddSignFieldItext77.pdf");
            iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer);

            var newPage = pdf.AddNewPage(new PageSize(PageSize.A4));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, true);

            var formRectangle = new iText.Kernel.Geom.Rectangle(100, 800, 200, 20);
            var formFieldBuilder = new TextFormFieldBuilder(pdf, "TextFormField1");
            formFieldBuilder.SetPage(newPage);

            formFieldBuilder.SetWidgetRectangle(formRectangle);
            var formfield = formFieldBuilder.CreateText();
            form.AddField(formfield);

            var formRectangle2 = new iText.Kernel.Geom.Rectangle(100, 700, 200, 20);
            var formFieldBuilder2 = new TextFormFieldBuilder(pdf, "TextFormField2");
            formFieldBuilder2.SetPage(newPage);

            formFieldBuilder2.SetWidgetRectangle(formRectangle2);
            var formfield2 = formFieldBuilder2.CreateText();
            form.AddField(formfield2);


            var signatureRectangle1 = new iText.Kernel.Geom.Rectangle(36, 448, 200, 100);
            var signatureField1 = new SignatureFormFieldBuilder(pdf, "SignField1");
            signatureField1.SetPage(newPage);
            signatureField1.SetWidgetRectangle(signatureRectangle1);
            PdfSignatureFormField sig1 = signatureField1.CreateSignature();


            PdfSigFieldLock pdfSigFieldLock1 = new PdfSigFieldLock();
            string[] fieldToLock1 = new string[] { "TextFormField1" };
            pdfSigFieldLock1.SetFieldLock(PdfSigFieldLock.LockAction.INCLUDE, fieldToLock1);
            PdfDictionary dict = sig1.GetPdfObject();
            dict.Put(PdfName.Lock, pdfSigFieldLock1.GetPdfObject());
            form.AddField(sig1);


            var signatureRectangle2 = new iText.Kernel.Geom.Rectangle(36, 248, 200, 100);
            var signatureField2 = new SignatureFormFieldBuilder(pdf, "SignField2");
            signatureField2.SetPage(newPage);
            signatureField2.SetWidgetRectangle(signatureRectangle2);
            PdfSignatureFormField sig2 = signatureField2.CreateSignature();

            PdfSigFieldLock pdfSigFieldLock2 = new PdfSigFieldLock();
            string[] fieldToLock2 = new string[] { "TextFormField2", };
            pdfSigFieldLock2.SetFieldLock(PdfSigFieldLock.LockAction.INCLUDE, fieldToLock2);
            PdfDictionary dict2 = sig2.GetPdfObject();
            dict2.Put(PdfName.Lock, pdfSigFieldLock2.GetPdfObject());
            form.AddField(sig2);
            pdf.Close();
        }

        /// <summary>
        /// itext lock some fields after sign - one field ok but second field sign cause error?
        /// https://stackoverflow.com/questions/76252293/itext-lock-some-fields-after-sign-one-field-ok-but-second-field-sign-cause-err
        /// 
        /// This is the OP's code with the addition of setting the Type entry of the form fields.
        /// Now Adobe Acrobat has no issues anymore signing both signature fields in the same session.
        /// </summary>
        /// <see cref="CreateLikeD00lar"/>
        [Test]
        public void CreateLikeD00larPlusType()
        {
            Directory.CreateDirectory(@"..\..\..\target\test-outputs\signature");

            PdfWriter writer = new PdfWriter(@"..\..\..\target\test-outputs\signature\AddSignFieldItext77PlusType.pdf");
            iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer);

            var newPage = pdf.AddNewPage(new PageSize(PageSize.A4));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, true);

            var formRectangle = new iText.Kernel.Geom.Rectangle(100, 800, 200, 20);
            var formFieldBuilder = new TextFormFieldBuilder(pdf, "TextFormField1");
            formFieldBuilder.SetPage(newPage);

            formFieldBuilder.SetWidgetRectangle(formRectangle);
            var formfield = formFieldBuilder.CreateText();
            form.AddField(formfield);

            var formRectangle2 = new iText.Kernel.Geom.Rectangle(100, 700, 200, 20);
            var formFieldBuilder2 = new TextFormFieldBuilder(pdf, "TextFormField2");
            formFieldBuilder2.SetPage(newPage);

            formFieldBuilder2.SetWidgetRectangle(formRectangle2);
            var formfield2 = formFieldBuilder2.CreateText();
            form.AddField(formfield2);


            var signatureRectangle1 = new iText.Kernel.Geom.Rectangle(36, 448, 200, 100);
            var signatureField1 = new SignatureFormFieldBuilder(pdf, "SignField1");
            signatureField1.SetPage(newPage);
            signatureField1.SetWidgetRectangle(signatureRectangle1);
            PdfSignatureFormField sig1 = signatureField1.CreateSignature();


            PdfSigFieldLock pdfSigFieldLock1 = new PdfSigFieldLock();
            string[] fieldToLock1 = new string[] { "TextFormField1" };
            pdfSigFieldLock1.SetFieldLock(PdfSigFieldLock.LockAction.INCLUDE, fieldToLock1);
            PdfDictionary dict = sig1.GetPdfObject();
            dict.Put(PdfName.Type, PdfName.Annot);
            dict.Put(PdfName.Lock, pdfSigFieldLock1.GetPdfObject());
            form.AddField(sig1);


            var signatureRectangle2 = new iText.Kernel.Geom.Rectangle(36, 248, 200, 100);
            var signatureField2 = new SignatureFormFieldBuilder(pdf, "SignField2");
            signatureField2.SetPage(newPage);
            signatureField2.SetWidgetRectangle(signatureRectangle2);
            PdfSignatureFormField sig2 = signatureField2.CreateSignature();

            PdfSigFieldLock pdfSigFieldLock2 = new PdfSigFieldLock();
            string[] fieldToLock2 = new string[] { "TextFormField2", };
            pdfSigFieldLock2.SetFieldLock(PdfSigFieldLock.LockAction.INCLUDE, fieldToLock2);
            PdfDictionary dict2 = sig2.GetPdfObject();
            dict2.Put(PdfName.Type, PdfName.Annot);
            dict2.Put(PdfName.Lock, pdfSigFieldLock2.GetPdfObject());
            form.AddField(sig2);
            pdf.Close();
        }
    }
}
