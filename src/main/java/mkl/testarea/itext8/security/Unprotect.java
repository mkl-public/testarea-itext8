package mkl.testarea.itext8.security;

import java.io.IOException;

import com.itextpdf.kernel.pdf.PdfDocument;
import com.itextpdf.kernel.pdf.PdfReader;
import com.itextpdf.kernel.pdf.PdfWriter;

/**
 * A simple tool to remove encryption from a file encrypted with the default
 * user password.
 * 
 * @author mkl
 */
public class Unprotect {
    /**
     * @param args
     */
    public static void main(String[] args) throws IOException {
        String fileIn;
        String fileOut;
        if (args.length == 0) {
            System.err.println("Unprotect fileIn.pdf [fileOut.pdf]");
            System.exit(1);
        }
        fileIn = args[0];
        if (args.length > 1)
            fileOut = args[1];
        else
            fileOut = fileIn + "-unprotected.pdf";
        try (
            PdfReader pdfReader = new PdfReader(fileIn).setUnethicalReading(true);
            PdfWriter pdfWriter = new PdfWriter(fileOut);
            PdfDocument pdfDocument = new PdfDocument(pdfReader, pdfWriter)
        ) {
            
        }
    }
}
