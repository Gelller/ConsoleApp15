using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp15
{
    class Program
    {
        private static readonly HttpClient _client = new HttpClient();
        static async Task Main(string[] args)
        {
            string resultTxt = "\\result.txt";
            try
            {
                var result = GetPosts(4);
                var result1 = GetPosts(5);
                var result2 = GetPosts(6);
                var result3 = GetPosts(7);
                var result4 = GetPosts(8);
                var result5 = GetPosts(9);
                var result6 = GetPosts(10);
                var result7 = GetPosts(11);
                var result8 = GetPosts(12);
                var result9 = GetPosts(13);

                var tasks = new List<Task<Post>>();
                tasks.AddRange(new[] { 
                    result, result1, result2, result3, 
                    result4, result5, result6, 
                    result7, result8, result9 
                });
                await Task.WhenAll(tasks);
                string path = Directory.GetCurrentDirectory();
                if (File.Exists(path + resultTxt))
                    File.Delete(path + resultTxt);
                tasks.ForEach(t => { 
                    File.AppendAllLines(path + resultTxt, new string[] { 
                        t.Result.UserId.ToString(), 
                        t.Result.Id.ToString(), 
                        t.Result.Title, 
                        t.Result.Body, 
                        " " 
                    }); 
                });
                Console.WriteLine("Готово");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }        
            Console.ReadLine();
        }
        static async Task<Post> GetPosts(int id)
        {
            var response = await _client.GetAsync($"https://jsonplaceholder.typicode.com/posts/{id.ToString()}");
            if(!response.IsSuccessStatusCode)
            {
                throw new Exception($"Status code {response.StatusCode}"); 
            }
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<Post>(content);
            return posts;
        }   
        class Post
        {       
          public int UserId { get; set; }
          public int Id { get; set; }
          public string  Title { get; set; }
          public string Body { get; set; }
        } 
    }
}
