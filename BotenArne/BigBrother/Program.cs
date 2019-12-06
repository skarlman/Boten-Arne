using System;

namespace BigBrother
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            SpeechRecognition.ContinuousRecognitionWithAuthorizationTokenAsync(new SpeechSender()).Wait();

            Console.WriteLine("Finished listening......");
            Console.ReadLine();

        }
    }


}
