package mkl.testarea.itext8.form;

import java.io.File;
import java.io.IOException;
import java.io.InputStream;
import java.util.Map;

import org.junit.BeforeClass;
import org.junit.Test;

import com.itextpdf.forms.PdfAcroForm;
import com.itextpdf.forms.fields.PdfFormField;
import com.itextpdf.kernel.pdf.PdfDocument;
import com.itextpdf.kernel.pdf.PdfReader;
import com.itextpdf.kernel.pdf.PdfWriter;

/**
 * @author mkl
 */
public class SetCheckbox {
    final static File RESULT_FOLDER = new File("target/test-outputs", "form");

    @BeforeClass
    public static void setUpBeforeClass() throws Exception {
        RESULT_FOLDER.mkdirs();
    }

    /**
     * <a href="https://stackoverflow.com/questions/76307909/enabling-a-single-checkbox-rather-than-the-entire-checkbox-within-the-same-acrof">
     * Enabling a single checkbox rather than the entire checkbox within the same acrofield
     * </a>
     * <br/>
     * <a href="https://www.cigna.com/static/www-cigna-com/docs/health-care-providers/form-cms1500.pdf">
     * form-cms1500.pdf
     * </a>
     * <p>
     * Indeed, all checkboxes are on.
     * </p>
     * @see #testSetCms1500()
     */
    @Test
    public void testSetAndFlattenCms1500LikeAbhiShrestha() throws IOException {
        try (
            InputStream resource = getClass().getResourceAsStream("form-cms1500.pdf");
            PdfReader reader = new PdfReader(resource);
            PdfWriter writer = new PdfWriter(new File(RESULT_FOLDER, "form-cms1500-SetAndFlattenedLikeAbhiShrestha-java.pdf"));
            PdfDocument pdfDoc = new PdfDocument(reader, writer)
        ) {
            PdfAcroForm form = PdfAcroForm.getAcroForm(pdfDoc, true);
            Map<String, PdfFormField> fields = form.getAllFormFields();

            fields.get("insurance_type").setValue("Medicare");

            form.flattenFields();
        }
    }

    /**
     * <a href="https://stackoverflow.com/questions/76307909/enabling-a-single-checkbox-rather-than-the-entire-checkbox-within-the-same-acrof">
     * Enabling a single checkbox rather than the entire checkbox within the same acrofield
     * </a>
     * <br/>
     * <a href="https://www.cigna.com/static/www-cigna-com/docs/health-care-providers/form-cms1500.pdf">
     * form-cms1500.pdf
     * </a>
     * <p>
     * Before flattening one can identify the issue better - here also all check boxes are
     * checked, and they are because all the AP entries are touched.
     * </p>
     * @see #testSetAndFlattenCms1500LikeAbhiShrestha()
     */
    @Test
    public void testSetCms1500() throws IOException {
        try (
            InputStream resource = getClass().getResourceAsStream("form-cms1500.pdf");
            PdfReader reader = new PdfReader(resource);
            PdfWriter writer = new PdfWriter(new File(RESULT_FOLDER, "form-cms1500-Set-java.pdf"));
            PdfDocument pdfDoc = new PdfDocument(reader, writer)
        ) {
            PdfAcroForm form = PdfAcroForm.getAcroForm(pdfDoc, true);
            Map<String, PdfFormField> fields = form.getAllFormFields();

            fields.get("insurance_type").setValue("Medicare");
        }
    }

}
