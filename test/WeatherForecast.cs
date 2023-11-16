namespace test;
    // startup Startup.cs คือ เรียกหลังจาก Program.cs รันขึ้น ซึ่งใช้ในการ config ค่า packages ต่าง ๆ ที่เรา install ลงเพื่อใช้งาน 
    //หรือ setting service ต่าง ๆ หรือ get ค่าจาก ENV มาใช้ ฯลฯ
public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}

