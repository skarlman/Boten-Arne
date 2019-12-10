using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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


            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/Home/InputSpeech");
            httpRequestMessage.Content = new StringContent($"\"{speech}\"",Encoding.UTF8,"application/json");
           
            var result = await client.SendAsync(httpRequestMessage);
            
            string resultContent = await result.Content.ReadAsStringAsync();
            Console.WriteLine(resultContent);


            //await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get,
            //    "https://localhost:44377/Home/InputSpeech?speech=" + speech));
        }
    }
}