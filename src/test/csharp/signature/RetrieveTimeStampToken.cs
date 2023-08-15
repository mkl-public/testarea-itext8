using iText.Commons.Bouncycastle.Asn1.Cms;
using iText.Commons.Bouncycastle.Asn1;
using iText.IO.Font;
using iText.Kernel.Pdf;
using iText.Signatures;
using NUnit.Framework;
using iText.Bouncycastleconnector;
using iText.Commons.Bouncycastle;
using iText.Commons.Bouncycastle.Tsp;
using Org.BouncyCastle.Cms;
using System.Collections;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Tsp;
using Org.BouncyCastle.Asn1;

namespace iText8.Net_Playground.Signature
{
    class RetrieveTimeStampToken
    {
        /// <summary>
        /// Read TimeStampToken in iText 8 from TimeStampTokenInfo
        /// https://stackoverflow.com/questions/76394568/read-timestamptoken-in-itext-8-from-timestamptokeninfo
        /// 
        /// This test illustrates how to extract the full TimeStampToken (and not merely the TSTInfo).
        /// There are two obvious options for this, using either the BouncyCastle abstraction of iText
        /// (allowing to work with both BouncyCastle and BouncyCastle-FIPS) or a specific BouncyCastle
        /// variant directly.
        /// 
        /// Also there are two cases, document time stamps and signature time stamps.
        /// 
        /// Here document time stamps are extracted using the BouncyCastle abstraction and signature
        /// time stamps are extracted using BouncyCastle directly.
        /// </summary>
        [Test]
        public void retrieveFromLoremIpsumLta()
        {
            Directory.CreateDirectory(@"..\..\..\target\test-outputs\signature");

            using (PdfReader pdfReader = new PdfReader(@"..\..\..\src\test\resources\mkl\testarea\itext8\signature\Lorem Ipsum-LTA.pdf"))
            using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
            {
                SignatureUtil signatureUtil = new SignatureUtil(pdfDocument);
                foreach (string name in signatureUtil.GetSignatureNames())
                {
                    var pdfPkcs7 = signatureUtil.ReadSignatureData(name);
                    var tstInfo = pdfPkcs7.GetTimeStampTokenInfo();
                    Console.WriteLine(name + ": " + tstInfo.GetGenTime());

                    PdfSignature pdfSignature = signatureUtil.GetSignature(name);
                    PdfString contents = pdfSignature.GetContents();
                    byte[] bytes = PdfEncodings.ConvertToBytes(contents.GetValue(), null);
                    if (pdfPkcs7.IsTsp())
                    {
                        IBouncyCastleFactory BOUNCY_CASTLE_FACTORY = BouncyCastleFactoryCreator.GetFactory();
                        IAsn1Object asn1Object = BOUNCY_CASTLE_FACTORY.CreateASN1InputStream(bytes).ReadObject();
                        IAsn1Sequence tokenSequence = BOUNCY_CASTLE_FACTORY.CreateASN1Sequence(asn1Object);
                        IContentInfo contentInfo = BOUNCY_CASTLE_FACTORY.CreateContentInfo(tokenSequence);
                        ITimeStampToken timeStampToken = BOUNCY_CASTLE_FACTORY.CreateTimeStampToken(contentInfo);
                        byte[] tstBytes = timeStampToken.GetEncoded();
                        File.WriteAllBytes(@"..\..\..\target\test-outputs\signature\Lorem Ipsum-LTA-" + name + "-doc.tst", tstBytes);
                    }
                    else
                    {
                        CmsSignedData cmsSignedData = new CmsSignedData(bytes);
                        ICollection signerInfos = cmsSignedData.GetSignerInfos().GetSigners();
                        foreach (SignerInformation signer in signerInfos.Cast<SignerInformation>())
                        {
                            Org.BouncyCastle.Asn1.Cms.Attribute attribute = signer.UnsignedAttributes[PkcsObjectIdentifiers.IdAASignatureTimeStampToken];
                            if (attribute != null)
                            {
                                foreach (Asn1Encodable asn1Encodable in attribute.AttrValues)
                                {
                                    CmsSignedData tstSignedData = new CmsSignedData(asn1Encodable.GetEncoded());
                                    TimeStampToken timeStampToken = new TimeStampToken(tstSignedData);
                                    byte[] tstBytes = timeStampToken.GetEncoded();
                                    File.WriteAllBytes(@"..\..\..\target\test-outputs\signature\Lorem Ipsum-LTA-" + name + "-sig.tst", tstBytes);
                                }
                            }
                        }
                    }
                }

            }
        }
    }
}
