package com.itextpdf.signatures.validation.v1;

import com.itextpdf.kernel.pdf.PdfDocument;

/**
 * Public extension of the currently package protected {@link SignatureValidator}
 * class. Eventually the signature validator will become public and this extension
 * will not be required anymore.
 */
public class PublicSignatureValidator extends SignatureValidator {
    public PublicSignatureValidator(PdfDocument document, ValidatorChainBuilder builder) {
        super(document, builder);
    }
}
