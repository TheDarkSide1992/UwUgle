using Infrastructure.Entities;
using SharedModels;

namespace Infrastructure.Implementations.Mappers;

internal class FileMapper
{
    /**
     * Maps entity document to Document format model
     */
    public Document FromEntityToDocument(DocumentEntity entity)
    {
        Document doc = new Document
        {
            DocumentID = entity.file_id,
            DocumentName = entity.file_name,
            File = entity.content
        };

        return doc;
    }

    /**
     * Maps Entity document into a simple document format
     */
    public DocumentSimple FromEntityToDocumentSimple(DocumentEntity entity)
    {
        DocumentSimple docuSimple = new DocumentSimple
        {
            DocumentID = entity.file_id,
            DocumentName = entity.file_name
        };

        return docuSimple;
    }
}