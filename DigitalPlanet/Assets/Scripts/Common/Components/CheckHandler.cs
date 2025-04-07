using UnityEngine;

public class CheckHandler
{
    // 地球平均半径，单位：米
    private const double EarthRadius = 6371000;

    // 计算两个经纬度之间的距离
    public  int CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        // 将角度转换为弧度
        double dLat = Deg2Rad(lat2 - lat1);
        double dLon = Deg2Rad(lon2 - lon1);

        double a = System.Math.Sin(dLat / 2) * System.Math.Sin(dLat / 2) +
                   System.Math.Cos(Deg2Rad(lat1)) * System.Math.Cos(Deg2Rad(lat2)) *
                   System.Math.Sin(dLon / 2) * System.Math.Sin(dLon / 2);

        double c = 2 * System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1 - a));

        // 计算两点间的距离（单位：米）
        double distance = EarthRadius * c;
        int mi = (int)System.Math.Round(distance);
        Debug.Log("@经纬度之间的近似直线距离：" + mi);
        // 计算距离
        return mi;
    }

    // 角度转弧度
    private static double Deg2Rad(double degrees)
    {
        return degrees * System.Math.PI / 180;
    }
}