

using Interface_Segregation_Principle_Demo.Model;
using Interface_Segregation_Principle_Demo.Ultilities;

class program
{
    static List<Videos> VideoList;
    static void PrintVideo(List<Videos> videos)
    {
        Console.WriteLine("List of Videos");
        Console.WriteLine("===============");
        foreach (var video in videos)
        {
            Console.WriteLine($"{video.Title.PadRight(39, ' ')} " + $"{video.Author.PadRight(15, ' ')} {video.Price} "
                + $"{video.Topic?.PadRight(15, ' ')} " + $"{video.Duration ?? "N/A"}");


        }
        Console.WriteLine();
    }
    static void Main(string[] args)
    {
        String id = String.Empty;
        Console.Title = "Interface Segregation Principle Demo";
        do
        {
            Console.WriteLine("File no. to read :1/2/3-Enter(exit): ");
            id = Console.ReadLine() ?? String.Empty;
            if ("123".Contains(id))
            {
                VideoList = Ultilities.ReadData(id);
                if (VideoList.Count == 0)
                {
                    Console.WriteLine("No videos found.");
                }
                else
                {
                    PrintVideo(VideoList);
                }
            }
            else if (id.ToLower() != "exit")
            {
                Console.WriteLine("Invalid input. Please enter 1, 2, or 3 to read a file, or 'exit' to quit.");
            }

        }
        while (!String.IsNullOrEmpty(id));



    }
}