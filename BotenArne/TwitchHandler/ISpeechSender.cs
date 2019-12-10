using System.Threading.Tasks;

namespace BigBrother
{
    public interface ISpeechSender
    {
        Task SendSpeech(string speech);
    }
}