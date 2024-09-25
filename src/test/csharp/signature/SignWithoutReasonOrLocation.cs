using iText.Forms.Fields.Properties;
using iText.Forms.Form.Element;
using iText.IO.Image;
using iText.IO.Source;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout.Properties;
using iText.Signatures;
using NUnit.Framework;
using Org.BouncyCastle.Asn1.Esf;

namespace iText8.Net_Playground.Signature
{
    class SignWithoutReasonOrLocation
    {
        /// <summary>
        /// iText8 Signature without reason or location
        /// https://stackoverflow.com/questions/79022298/itext8-signature-without-reason-or-location
        /// 
        /// Indeed, currently iText generated signature appearances always have reason and location
        /// lines. There used to be the feature of dropping those lines if the value was empty or
        /// null. Apparently this feature stopped warking, a regression.
        /// </summary>
        [Test]
        public void SignLikeTaniaVanderstraeten()
        {
            string pdfFile = @"..\..\..\src\test\resources\mkl\testarea\itext8\signature\test.pdf";
            string imageFile = @"..\..\..\src\test\resources\mkl\testarea\itext8\signature\stamp.png";
            string resultFile = @"..\..\..\target\test-outputs\signature\test-signedLikeTaniaVanderstraeten.pdf";

            DummyRsaSigningMaterial signingMaterial = new DummyRsaSigningMaterial();
            SignWithService.ServiceSignature eidSignature = new SignWithService.ServiceSignature();
            var chain = eidSignature.GetChain();

            using (PdfReader reader = new PdfReader(pdfFile))
            using (MemoryStream mem = new MemoryStream())
            {
                StampingProperties properties = new StampingProperties();
                properties.UseAppendMode();

                string fieldName = "SignatureField";
                var IMG = imageFile; // "sig.jpg";

                SignerProperties signerProperties = new SignerProperties();
                signerProperties.SetPageRect(new iText.Kernel.Geom.Rectangle(100, 100, 400, 200) /*rectangle*/);
                signerProperties.SetPageNumber(1 /*pageNr*/);
                signerProperties.SetFieldName(fieldName);
                signerProperties.SetSignatureCreator("Signing Test" /*signer*/);
                signerProperties.SetSignDate(DateTime.Now /*signDate*/);
                signerProperties.SetReason("");
                signerProperties.SetLocation("");

                var signatureAppearance = new SignatureFieldAppearance(fieldName);
                signatureAppearance.SetContent(new SignedAppearanceText().SetReasonLine("").SetLocationLine(""));
                signatureAppearance.SetBackgroundImage(
                new BackgroundImage.Builder()
                        .SetImage(new PdfImageXObject(ImageDataFactory.Create(IMG)))
                        .Build());
                signerProperties.SetSignatureAppearance(signatureAppearance);

                PdfSigner signer = new PdfSigner(reader, mem, null, properties, signerProperties);
                signer.SignDetached(eidSignature, chain, null/*crlList*/, null, null/*tsaClient*/, 0, PdfSigner.CryptoStandard.CADES);

                File.WriteAllBytes(resultFile, mem.ToArray());
            }
        }
    }
}
