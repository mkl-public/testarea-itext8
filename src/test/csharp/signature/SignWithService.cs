using iText.Bouncycastle.X509;
using iText.Kernel.Pdf;
using iText.Signatures;
using NUnit.Framework;
using Org.BouncyCastle.X509;
using System.Security.Cryptography;

namespace iText8.Net_Playground.Signature
{
    internal class SignWithService
    {
        /// <summary>
        /// Signing a PDF through iText with the use signature from external web service(digidentity)
        /// https://stackoverflow.com/questions/78517659/signing-a-pdf-through-itext-with-the-use-signature-from-external-web-servicedig
        /// 
        /// You can sign a PDF using a signing service by wrapping all service specific code in an
        /// <see cref="IExternalSignature"/> implementation (like <see cref="ServiceSignature"/> here),
        /// so that seen from your main code signing becomes a single-step, simple task.
        /// </summary>
        [Test]
        public void SignWithIExternalSignature()
        {
            string pdfFile = @"..\..\..\src\test\resources\mkl\testarea\itext8\signature\test.pdf";
            string resultFile = @"..\..\..\target\test-outputs\signature\test-signedWithServiceSignature.pdf";

            using (PdfReader reader = new PdfReader(pdfFile))
            using (MemoryStream mem = new MemoryStream())
            {
                PdfSigner signer = new PdfSigner(reader, mem, new StampingProperties().UseAppendMode());
                ServiceSignature signature = new ServiceSignature();
                signer.SignDetached(signature, signature.GetChain(), null, null, null, 0, PdfSigner.CryptoStandard.CMS);
                File.WriteAllBytes(resultFile, mem.ToArray());
            }
        }

        /// <summary>
        /// This <see cref="IExternalSignature"/> implementation is where you put all the code that needs
        /// to access the API of your service provider. Obviously you can inject required extra information
        /// in the constructor or via properties, and you can optimize this for multiple usages (e.g. by
        /// retrieving the certificate chain only once).
        /// </summary>
        /// <seealso cref="SignWithIExternalSignature"/>
        class ServiceSignature : IExternalSignature
        {
            public string GetDigestAlgorithmName()
            {
                return "SHA-256";
            }

            public string GetSignatureAlgorithmName()
            {
                return "RSA";
            }

            public ISignatureMechanismParams? GetSignatureMechanismParameters()
            {
                return null;
            }

            public X509CertificateBC[] GetChain()
            {
                // Replace this with your code that retrieves your certificate chain from your signature
                // service.
                return new X509CertificateBC[] {
                    new X509CertificateBC(new X509Certificate(dummy.certificate.GetRawCertData()))
                };
            }

            public byte[] Sign(byte[] message)
            {
                byte[] hash = SHA256.HashData(message);

                // Replace this with your code that retrieves a signature for the given hash value
                // from your signature service.
                return dummy.rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }

            DummyRsaSigningMaterial dummy = new DummyRsaSigningMaterial();
        }
    }
}
