using System.IO;
using System;

public interface IByteBuffer
{
    byte this[int index] { get; set; }

    void position(int i);

    //JAVA TO VB & C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
    //ORIGINAL LINE: public abstract void readFrom(InputStream inputstream) throws IOException;
    void readFrom(Stream inputstream);

    void skipBytes(int i);

    //JAVA TO VB & C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
    //ORIGINAL LINE: public abstract void readFrom(InputStream inputstream, int i) throws IOException;
    void readFrom(Stream inputstream, int i);

    int capacity();

    //JAVA TO VB & C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
    //ORIGINAL LINE: public abstract void writeTo(OutputStream outputstream) throws IOException;
    void writeTo(Stream outputstream);

    void pack();

    void writeByte(int i);

    int readByte();

    int readUnsignedByte();

    void read(byte[] bytes, int i, int j, int k);

    int getReadPos();

    void setReadPos(int i);

    void write(byte[] bytes, int i, int j, int k);

    void writeChar(char c);

    char readChar();

    byte[] getBytes();

    object Clone();

    void writeAnsiString(string s);

    //string readAnsiString();

    int Length();

    void writeBoolean(bool flag);

    bool readBoolean();

    float readFloat();

    void writeFloat(float i);

    double readDouble();

    void writeDouble(double i);

    void reset();

    void writeLong(long l);

    void writeShortAnsiString(string s);

    long readLong();

    void writeShort(int i);

    int readShort();

    void readBytes(IByteBuffer data, int offset, int length);

    void writeBytes(IByteBuffer data, int offset, int length);

    void writeByteBuffer(IByteBuffer bytebuffer);

    void writeByteBuffer(IByteBuffer bytebuffer, int length);

    void writeBytes(byte[] bytes);

    void writeBytes(byte[] bytes, int offset, int length);

    byte[] readBytes(int i);

    int readUnsignedShort();

    //string readShortAnsiString();

    int available();

    //string ToString();

    int getWritePos();

    void setWritePos(int i);

    byte[] getRawBytes();

    void writeUTF(String s);

    String readUTF();

    void Clear();

    void writeInt(int i);

    int readInt();

    int position();

    /// <summary> 读出一个指定长度的字节数组  </summary>
    byte[] readData();

    /// <summary> 写入一个字节数组，可以为null  </summary>
    void writeData(byte[] data);

    int bytesAvailable { get; }
}
