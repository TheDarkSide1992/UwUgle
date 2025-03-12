namespace Infrastructure.Entities;

internal class DocumentEntity
{
    public int file_id { get; set; }
    public string file_name { get; set; }
    public Byte[] content { get; set; }
}