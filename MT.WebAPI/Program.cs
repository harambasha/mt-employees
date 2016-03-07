using System;
using System.Net.Http;
using System.Text;
using Microsoft.Owin.Hosting;
using MT.WebAPI.Helpers;

namespace MT.WebAPI
{
    public class Program
    {
        private static void Main()
        {
            using (WebApp.Start<Startup>(ProgramConstants.BASE_ADRESS))
            {
                var client = new HttpClient();
                var response =
                    client.GetAsync(new StringBuilder().Append(ProgramConstants.BASE_ADRESS)
                        .Append(ProgramConstants.EMPLOYEE_ENDPOINT)
                        .ToString()).Result;

                if (response != null)
                {
                    Console.WriteLine(
                        new StringBuilder().Append(ProgramConstants.RESULT)
                            .Append(response.Content.ReadAsStringAsync().Result)
                            .ToString());
                }
                else
                {
                    Console.WriteLine(ProgramConstants.HTTP_CLIENT_ERROR_MESSAGE);
                }
                Console.WriteLine();
                Console.WriteLine(ProgramConstants.STOP_AND_CLOSE_MESSAGE);
                Console.ReadLine();
            }
        }
    }
}