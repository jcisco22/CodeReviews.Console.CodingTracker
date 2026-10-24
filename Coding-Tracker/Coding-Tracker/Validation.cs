using System.Globalization;

class Validation
{
    public static DateTime ValidateTime(string getTime)
    {
        DateTime result;
           
        while (!DateTime.TryParseExact(getTime, "HH:mm", null, DateTimeStyles.None, out result))
        {
            Console.WriteLine("Invalid format. Try again (HH:mm): ");
            getTime = Console.ReadLine();
        }

        return result;
    }

    public static DateTime ValidateEndTime(DateTime startTime)
    {
        DateTime endTime = ValidateTime(Console.ReadLine());

        while (endTime <= startTime)
        {
         Console.WriteLine("End time must be after start time.\nPlease try again (dd-MM-yyyy HH:mm): ");
         endTime = ValidateTime(Console.ReadLine());
        }

        return endTime;
    }
    public static int ValidateId(string Input)
    {
        // Console.WriteLine($"Input received: '{Input}'");

        int result;

        while (!int.TryParse(Input, out result) || (result <= 0))
        {
            Console.WriteLine("Please enter a valid id");
            Input = Console.ReadLine();
        }

        return result;
    }
}