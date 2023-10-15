namespace NetCordAddons.Examples.Services;

public class TransientTest
{
    private int _number;

    public void Add()
    {
        _number += 1;
        Console.WriteLine(_number);
    }
}