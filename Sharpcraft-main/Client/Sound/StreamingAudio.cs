using StbVorbisSharp;

namespace SharpCraft.Client.Sound;

public class StreamingAudio
{
    public readonly Vorbis Decoder;
    public readonly uint Buffer;

    public StreamingAudio(Vorbis decoder, uint buffer)
    {
        this.Decoder = decoder;
        this.Buffer = buffer;
    }
}