using System;


namespace DataTransferObjects
{
    public class ReportingDocumentDTO
    {
        public String DocCode;
        public String DocName;
        public String PaperSize;
        public int Width;
        public int Height;

        public ReportingDocumentDTO()
        {
        }

        public override String ToString()
        {
            return DocCode + " - " + DocName;
        }
    }

}
