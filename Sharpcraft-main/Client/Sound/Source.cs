using System.Numerics;

namespace SharpCraft.Client.Sound;

public class Source
{
    public readonly SourceType Type;
    
    public uint Channel;
    public bool HasChannel;
    public float Pitch;
    public float Gain;
    public bool Looping;
    public Vector3 Position;
    public float DistOrRoll;
    public AttenuationModel AttenuationModel;
    public StreamingAudio StreamingAudio;
    public uint Buffer;

    public Source(SourceType type)
    {
        this.Type = type;
        this.Channel = 0u;
        this.HasChannel = false;
        this.Pitch = 0.0f;
        this.Gain = 0.0f;
        this.Looping = false;
        this.Position = Vector3.Zero;
        this.DistOrRoll = 0.0f;
        this.AttenuationModel = AttenuationModel.None;
        this.StreamingAudio = null;
        this.Buffer = 0;
    }

    public void DetachChannel()
    {
        this.Channel = 0u;
        this.HasChannel = false;
    }

    public void AttachChannel(uint chan)
    {
        this.Channel = chan;
        this.HasChannel = true;
    }
}