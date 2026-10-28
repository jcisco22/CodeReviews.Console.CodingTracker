using System.Globalization;

class Validation
{
    public static DateTime? ValidateTime(string getTime)
    {
        DateTime result;

        while (!DateTime.TryParseExact(getTime, "dd-MM-yyyy HH:mm", null, DateTimeStyles.None, out result))
        {
            if (getTime == "0") return null;

            Console.WriteLine("Invalid format. Try again (dd-MM-yyyy, HH:mm): \nOr enter 0 to exit. ");
            getTime = Console.ReadLine();
        }

        return result;

    }

    public static DateTime? ValidateEndTime(DateTime startTime)
    {
        DateTime? endTime = ValidateTime(Console.ReadLine());

        while (endTime != null && endTime <= startTime)
        {
            Console.WriteLine("End time must be after start time. Try again (dd-MM-yyyy, HH:mm), or enter 0 to return to the main menu: ");
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