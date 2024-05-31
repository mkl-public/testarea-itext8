using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace iText8.Net_Playground.Signature
{
    public class DummyRsaSigningMaterial
    {
        public RSA rsa;
        public X509Certificate2 certificate;

        public DummyRsaSigningMaterial()
        {
            rsa = RSA.Create();

            CertificateRequest request = new CertificateRequest("CN=Dummy RSA signer", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(366));
        }
    }
}
