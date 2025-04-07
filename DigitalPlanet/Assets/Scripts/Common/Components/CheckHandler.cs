using UnityEngine;

public class CheckHandler
{
    // ����ƽ���뾶����λ����
    private const double EarthRadius = 6371000;

    // ����������γ��֮��ľ���
    public  int CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        // ���Ƕ�ת��Ϊ����
        double dLat = Deg2Rad(lat2 - lat1);
        double dLon = Deg2Rad(lon2 - lon1);

        double a = System.Math.Sin(dLat / 2) * System.Math.Sin(dLat / 2) +
                   System.Math.Cos(Deg2Rad(lat1)) * System.Math.Cos(Deg2Rad(lat2)) *
                   System.Math.Sin(dLon / 2) * System.Math.Sin(dLon / 2);

        double c = 2 * System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1 - a));

        // ���������ľ��루��λ���ף�
        double distance = EarthRadius * c;
        int mi = (int)System.Math.Round(distance);
        Debug.Log("@��γ��֮��Ľ���ֱ�߾��룺" + mi);
        // �������
        return mi;
    }

    // �Ƕ�ת����
    private static double Deg2Rad(double degrees)
    {
        return degrees * System.Math.PI / 180;
    }
}