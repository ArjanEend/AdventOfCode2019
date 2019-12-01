using System;

public static class Day1Methods
{
    public static int GetFuelRequiredForMass(int mass)
    {
        return (int)Math.Floor((float)(mass / 3)) - 2;;
    }

    public static int CalculateFuel(int mass)
    {
        int fuel = GetFuelRequiredForMass(mass);
        if(fuel >= 0)
            return fuel + CalculateFuel(fuel);
        return 0;
    }
}