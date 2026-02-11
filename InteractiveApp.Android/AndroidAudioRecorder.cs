using Android.Media;
using System;
using System.IO;
using System.Threading.Tasks;
using InteractiveApp.Services;


namespace InteractiveApp.Android;


public class AndroidAudioRecorder : IAudioRecorder
{
    private const int SampleRate = 16000;
    private const ChannelIn Channel = ChannelIn.Mono;
    private const Encoding AudioEncoding = Encoding.Pcm16bit;

    private bool _isRecording;

    public async Task<byte[]> RecordAsync(int seconds)
    {
        if (_isRecording)
            throw new InvalidOperationException("Ya grabando");

        _isRecording = true;
        await Task.Delay(500);

        int minBufferSize = AudioRecord.GetMinBufferSize(
            SampleRate, Channel, AudioEncoding);

        var recorder = new AudioRecord(
            AudioSource.Mic,
            SampleRate,
            Channel,
            AudioEncoding,
            minBufferSize);

        if (recorder.State != State.Initialized)
            throw new InvalidOperationException("AudioRecord no inicializado");

        try
        {
            using var stream = new MemoryStream();
            var buffer = new byte[minBufferSize];

            recorder.StartRecording();
            Console.WriteLine("[AUDIO] StartRecording");

            var end = DateTime.UtcNow.AddSeconds(seconds);

            while (DateTime.UtcNow < end)
            {
                int read = recorder.Read(buffer, 0, buffer.Length);
                if (read > 0)
                    stream.Write(buffer, 0, read);
            }

            recorder.Stop();
            Console.WriteLine("[AUDIO] StopRecording");

            var pcm = stream.ToArray();
            return AddWavHeader(pcm);
        }
        finally
        {
            recorder.Release();
            recorder.Dispose();
            _isRecording = false;

            Console.WriteLine("[AUDIO] Released");
        }
    }
    private static byte[] AddWavHeader(byte[] pcmData)
    {
        const int sampleRate = 16000;
        const short channels = 1;
        const short bitsPerSample = 16;

        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        int byteRate = sampleRate * channels * bitsPerSample / 8;
        short blockAlign = (short)(channels * bitsPerSample / 8);

        writer.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
        writer.Write(36 + pcmData.Length);
        writer.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"));

        writer.Write(System.Text.Encoding.ASCII.GetBytes("fmt "));
        writer.Write(16);
        writer.Write((short)1);
        writer.Write(channels);
        writer.Write(sampleRate);
        writer.Write(byteRate);
        writer.Write(blockAlign);
        writer.Write(bitsPerSample);

        writer.Write(System.Text.Encoding.ASCII.GetBytes("data"));
        writer.Write(pcmData.Length);
        writer.Write(pcmData);

        return stream.ToArray();
    }

}