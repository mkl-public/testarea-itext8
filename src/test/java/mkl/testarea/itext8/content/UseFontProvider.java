package mkl.testarea.itext8.content;

import java.io.File;
import java.io.IOException;

import org.junit.BeforeClass;
import org.junit.Test;

import com.itextpdf.kernel.pdf.PdfDocument;
import com.itextpdf.kernel.pdf.PdfWriter;
import com.itextpdf.layout.Document;
import com.itextpdf.layout.element.Div;
import com.itextpdf.layout.element.Paragraph;
import com.itextpdf.layout.font.FontProvider;
import com.itextpdf.layout.properties.Property;

/**
 * @author mkl
 */
public class UseFontProvider {
    final static File RESULT_FOLDER = new File("target/test-outputs", "content");

    @BeforeClass
    public static void setUpBeforeClass() throws Exception {
        RESULT_FOLDER.mkdirs();
    }

    /**
     * <a href="https://stackoverflow.com/questions/78330185/in-itext-is-it-possible-to-create-a-custom-font-with-different-stylings-and-make">
     * In iText is it possible to create a custom font with different stylings and make it work with .setBold and .setItalic?
     * </a>
     * <p>
     * This test illustrates how to use font families and font providers.
     * </p>
     */
    @Test
    public void testUseSystemFonts() throws IOException {
        try (
            PdfWriter pdfWriter = new PdfWriter(new File(RESULT_FOLDER, "UseSystemFontProvider.pdf"));
            PdfDocument pdfDocument = new PdfDocument(pdfWriter);
            Document document = new Document(pdfDocument);
        ) {
            FontProvider fontProvider = new FontProvider();
            fontProvider.addSystemFonts();
            document.setFontProvider(fontProvider);

            Div div = new Div();
            div.setFontFamily("Verdana");
            Paragraph paragraph = new Paragraph();
            paragraph.add("Regular");
            div.add(paragraph);
            paragraph = new Paragraph();
            paragraph.setProperty(Property.FONT_WEIGHT, "700");
            paragraph.add("bold");
            div.add(paragraph);
            paragraph = new Paragraph();
            paragraph.setProperty(Property.FONT_STYLE, "italic");
            paragraph.add("Italic");
            div.add(paragraph);
            paragraph = new Paragraph();
            paragraph.setProperty(Property.FONT_WEIGHT, "700");
            paragraph.setProperty(Property.FONT_STYLE, "italic");
            paragraph.add("Bold & Italic");
            div.add(paragraph);
            document.add(div);
        }
    }

}
