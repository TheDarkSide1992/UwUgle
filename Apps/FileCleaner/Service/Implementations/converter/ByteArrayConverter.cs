using Service.Interfaces;

namespace Service.Implementations.converter;

public class ByteArrayConverter : IConverter<Byte[],string>
{
    /**
     * Converts string to Byte array
     */
    public Task<string> From(byte[] message)
    {
        string? str = System.Text.Encoding.UTF8.GetString(message);

        return Task.FromResult(str);
    }

    /**
     * Converts Byte array to string
     */
    public Task<byte[]> To(string message)
    {
        byte[]? byt = System.Text.Encoding.UTF8.GetBytes(message);

        return Task.FromResult(byt);
    }
}