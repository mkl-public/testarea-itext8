package mkl.testarea.itext8.signature;

import java.io.File;
import java.io.IOException;

import com.itextpdf.kernel.pdf.PdfDocument;
import com.itextpdf.kernel.pdf.PdfReader;
import com.itextpdf.signatures.validation.v1.PublicSignatureValidator;
import com.itextpdf.signatures.validation.v1.ValidatorChainBuilder;
import com.itextpdf.signatures.validation.v1.report.ValidationReport;

/**
 * This class allows to execute the iText <code>SignatureValidator</code> from the
 * command line.
 * <p>
 * Because currently the <code>SignatureValidator</code> still is package protected,
 * we actually use the {@link PublicSignatureValidator}, a class derived from it to
 * make it public. Eventually the signature validator will become public and this
 * extension will not be required anymore.
 */
public class ValidateSignatures {
    public static void main(String[] args) throws IOException {
        ValidateSignatures instance = new ValidateSignatures();
        for (String arg : args) {
            File file = new File(arg);
            System.out.println();
            System.out.println();
            System.out.println(file);
            System.out.println();
            System.out.println();
            instance.validate(file);
        }
    }

    public void validate(File file) throws IOException {
        ValidatorChainBuilder builder = new ValidatorChainBuilder();

        try (
            PdfReader pdfReader = new PdfReader(file);
            PdfDocument pdfDocument = new PdfDocument(pdfReader);
        ) {
            PublicSignatureValidator validator = new PublicSignatureValidator(pdfDocument, builder);

            ValidationReport report = validator.validateLatestSignature();
            System.out.println(report);
        }
    }
}
