using System.Globalization;

class Validation
{
public static DateTime ValidateTime(string getTime)
{
    DateTime result;

    while (true)
    {


        if (DateTime.TryParseExact(getTime, "HH:mm", null,
            DateTimeStyles.None, out result))
        {
            return result;
        }

        Console.WriteLine("Invalid format. Try again (HH:mm): ");
        getTime = Console.ReadLine();
    }
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
        int result;

        while (!int.TryParse(Input, out result) || (result <= 0))
        {

         if (Input == "0") return 0;

            Console.WriteLine("Please enter a valid id, or press 0 to return to the main menu");
            Input = Console.ReadLine();
        }

        return result;
    }
}