namespace SharedModels;

public class Document
{
    public int? DocumentID { get; set; }
    public string DocumentName { get; set; }
    public Byte[] File { get; set; }
}