using iText.Kernel.Pdf;
using iText.Signatures;
using NUnit.Framework;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;

namespace iText8.Net_Playground.Signature
{
    class SignWithEcdsaCert
    {
        /// <summary>
        /// ECDsa signed PDF showing "Document has been altered or corrupted since it was signed" error in Adobe Reader
        /// https://stackoverflow.com/questions/77874119/ecdsa-signed-pdf-showing-document-has-been-altered-or-corrupted-since-it-was-si
        /// test.pdf (as testImreTessenyi.pdf)
        /// test.pfx (as testImreTessenyi.pfx)
        /// https://drive.google.com/drive/folders/1NucHLeYBjZ4NP3ijrAXtk8ZqAwdW5ipY?usp=sharing
        /// 
        /// As it turns out, ECDsa.SignData returns the signature in plain format but iText
        /// used the signature algorithm OID for ECD signatures in DER format. Transforming
        /// the signature to DER format fixed the issue.
        /// </summary>
        [Test]
        public void SignImreTessenyi()
        {
            Directory.CreateDirectory(@"..\..\..\target\test-outputs\signature");

            string pdfFile = @"..\..\..\src\test\resources\mkl\testarea\itext8\signature\testImreTessenyi.pdf";
            string pfxFile = @"..\..\..\src\test\resources\mkl\testarea\itext8\signature\testImreTessenyi.pfx";
            string resultFile = @"..\..\..\target\test-outputs\signature\testImreTessenyi-signed.pdf";

            byte[] signedPdf = SignPdfECC(pdfFile, pfxFile);
            File.WriteAllBytes(resultFile, signedPdf);
        }

        /// <see cref="SignImreTessenyi"/>
        private static byte[] SignPdfECC(string unsignedPdfPath, string certificatePfxPath)
        {
            byte[] certificatePfx = File.ReadAllBytes(certificatePfxPath);

            using (PdfReader reader = new PdfReader(unsignedPdfPath))
            using (MemoryStream mem = new MemoryStream())
            {
                PdfSigner signer = new PdfSigner(reader, mem, new StampingProperties().UseAppendMode());
                X509Certificate cert = new X509Certificate(certificatePfx, "password", X509KeyStorageFlags.Exportable);
                X509Certificate2 signatureCert = new X509Certificate2(cert);
                ECDsa pk = signatureCert.GetECDsaPrivateKey();
                IExternalSignature signature = new EcdsaSignature(pk, DigestAlgorithms.SHA256);
                iText.Bouncycastle.X509.X509CertificateBC[] chain = new iText.Bouncycastle.X509.X509CertificateBC[] { new iText.Bouncycastle.X509.X509CertificateBC(new Org.BouncyCastle.X509.X509Certificate(signatureCert.GetRawCertData())) };
                signer.SignDetached(signature, chain, null, null, null, 0, PdfSigner.CryptoStandard.CMS);
                return mem.ToArray();
            }
        }

        /// <see cref="SignImreTessenyi"/>
        /// <see cref="SignPdfECC"/>
        public class EcdsaSignature : IExternalSignature
        {
            private readonly string _encryptionAlgorithm;
            private readonly string _hashAlgorithm;
            private readonly ECDsa _pk;

            public EcdsaSignature(ECDsa pk, string hashAlgorithm)
            {
                _pk = pk;
                _hashAlgorithm = DigestAlgorithms.GetDigest(DigestAlgorithms.GetAllowedDigest(hashAlgorithm));
                _encryptionAlgorithm = "ECDSA";
            }

            public string GetDigestAlgorithmName()
            {
                return "SHA-256";
            }

            public virtual string GetEncryptionAlgorithm()
            {
                return _encryptionAlgorithm;
            }

            public virtual string GetHashAlgorithm()
            {
                return _hashAlgorithm;
            }

            public string GetSignatureAlgorithmName()
            {
                return "ECDSA";
            }

            public ISignatureMechanismParams GetSignatureMechanismParameters()
            {
                return null;
            }

            /// <summary>
            /// Here the fix is applied, the signature returned by _pk.SignData
            /// is now transformed from plain format to DER format before returning
            /// it from this method.
            /// </summary>
            public virtual byte[] Sign(byte[] message)
            {
                return PlainToDer(_pk.SignData(message, new HashAlgorithmName(_hashAlgorithm)));
            }

            /// <summary>
            /// Copied from https://github.com/itext/i7ns-samples/blob/develop/itext/itext.publications/itext.publications.signing-examples.simple/iText/SigningExamples/Simple/X509Certificate2Signature.cs
            /// </summary>
            byte[] PlainToDer(byte[] plain)
            {
                int valueLength = plain.Length / 2;
                BigInteger r = new BigInteger(1, plain, 0, valueLength);
                BigInteger s = new BigInteger(1, plain, valueLength, valueLength);
                return new DerSequence(new DerInteger(r), new DerInteger(s)).GetEncoded(Asn1Encodable.Der);
            }
        }

    }
}
