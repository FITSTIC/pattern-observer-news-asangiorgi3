using System;
using System.Collections.Generic;
using System.Threading;

namespace observer_event
{
    class Program
    {
        static void Main(string[] args)
        {
            var newsGenerator = new NewsSender();
            var nr1 = new NewsRater("Matteo");
            var nr2 = new NewsRater("Luca");
            var nr3 = new NewsRater("Andrea");

            newsGenerator.OnNewNews += nr1.Rate;
            newsGenerator.OnNewNews += nr2.Rate;
            newsGenerator.OnNewNews += nr3.Rate;

            int i = 1;
            string title, content;
            
            do{
                Console.WriteLine();
                Console.WriteLine($"News #{i++}");
                Console.Write("Title: ");
                title = Console.ReadLine();
                Console.WriteLine();

                Console.Write("Content: ");
                content = Console.ReadLine();
                Console.WriteLine();

                newsGenerator.GenerateNewNews(title, content);

                Console.Write("Insert new news? (y/n) : ");
            }while(Console.ReadKey().KeyChar != 'n');

            Console.WriteLine();

            newsGenerator.OnNewNews -= nr1.Rate;
            newsGenerator.OnNewNews -= nr2.Rate;
            newsGenerator.OnNewNews -= nr3.Rate;
        }        
    }

    struct News
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }
    }

    class NewsRater
    {
        private readonly string name;
        Random rnd = new Random();

        public NewsRater(string name)
        {
            this.name = name;
        }
        
        public void Rate(News n)
        {
            Console.WriteLine($"[{name}] News rate: "+rnd.Next(1,6));
        }
    }

    class NewsSender
    {
        public delegate void NewNewsHandler(News n);
        private event NewNewsHandler _onNewNews;

        public event NewNewsHandler OnNewNews
        {
            add
            {
                _onNewNews += value;
                Console.WriteLine("New rater registered.");
            }
            remove
            {
                _onNewNews -= value;
                Console.WriteLine("Rater unregistered.");
            }
        }

        public void GenerateNewNews(string title, string content)
        {
            var n = new News{
                Title = title,
                Content = content,
                Date = DateTime.Now.ToLongTimeString()
            };

            Notify(n);
        }

        private void Notify(News n)
        {
            _onNewNews(n);
        }
    }
}
