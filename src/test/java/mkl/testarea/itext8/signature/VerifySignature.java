package mkl.testarea.itext8.signature;

import java.io.IOException;
import java.io.InputStream;
import java.security.GeneralSecurityException;
import java.security.Security;
import java.util.List;

import org.bouncycastle.jce.provider.BouncyCastleProvider;
import org.junit.BeforeClass;
import org.junit.Test;

import com.itextpdf.kernel.pdf.PdfDocument;
import com.itextpdf.kernel.pdf.PdfReader;
import com.itextpdf.signatures.CertificateInfo;
import com.itextpdf.signatures.PdfPKCS7;
import com.itextpdf.signatures.SignatureUtil;

/**
 * @author mkl
 */
public class VerifySignature {
    @BeforeClass
    public static void setUpBeforeClass() throws Exception {
        BouncyCastleProvider bcp = new BouncyCastleProvider();
        Security.insertProviderAt(bcp, 1);
    }

    /**
     * <a href="https://stackoverflow.com/questions/77537388/unknown-pdfexception-by-itext7-when-timestamptoken-is-not-present-in-the-embed">
     * "Unknown PdfException" by iText7 when TimeStampToken is not present in the embedded Pkcs7 UnsignedAttributes section
     * </a>
     * <br/>
     * <a href="https://drive.google.com/file/d/1v0zBc-YW-3VmANTms33gSa8mu5QVrjDG/view">
     * output.pdf
     * </a> as "riccardo_output.pdf"
     * <p>
     * Indeed, there is a regression: If the signature container has unsigned attributes
     * but not a timestamp attribute among them, the code runs into an exception due to
     * a method call on a <code>null</code> object. It's just fairly seldom that a signature
     * has unsigned attributes but not a timestamp one, so the error has not popped up before.
     * </p>
     */
    @Test
    public void testRiccardoOutput() throws IOException, GeneralSecurityException {
        System.out.println("\n\nriccardo_output.pdf\n===================");
        
        try (   InputStream resource = getClass().getResourceAsStream("riccardo_output.pdf") ) {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(resource));
            SignatureUtil signUtil = new SignatureUtil(pdfDoc);
            List<String> names = signUtil.getSignatureNames();
            for (String name : names) {
                System.out.println("===== " + name + " =====");
                System.out.println("Signature covers whole document: " + signUtil.signatureCoversWholeDocument(name));
                System.out.println("Document revision: " + signUtil.getRevision(name) + " of " + signUtil.getTotalRevisions());
                PdfPKCS7 pkcs7 = signUtil.readSignatureData(name);
                System.out.println("Subject: " + CertificateInfo.getSubjectFields(pkcs7.getSigningCertificate()));
                System.out.println("Integrity check OK? " + pkcs7.verifySignatureIntegrityAndAuthenticity());
            }
            System.out.println();
        }
    }

}
