
Console.ForegroundColor = ConsoleColor.Red;
Console.BackgroundColor = ConsoleColor.White;
Console.WriteLine("Красный текст на белом фоне");
Console.ResetColor();
Console.WriteLine("Обычный текст");


foreach (var color in Enum.GetValues<ConsoleColor>())
{
    Console.Write($"{color,3:D} - ");
    Console.ForegroundColor = color;
    Console.WriteLine(color);
    Console.ResetColor();
}