using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BigBrother
{
    public class SpeechSender : ISpeechSender
    {
        readonly HttpClient _client = new HttpClient();

        public async Task SendSpeech(string speech)
        {
            using var client = new HttpClient();
            
            client.BaseAddress = new Uri("https://localhost:44377");
            
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("speech", speech)
            });
            
            var result = await client.PostAsync("/Home/InputSpeech", content);
            
            string resultContent = await result.Content.ReadAsStringAsync();
            Console.WriteLine(resultContent);


            //await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get,
            //    "https://localhost:44377/Home/InputSpeech?speech=" + speech));
        }
    }
}