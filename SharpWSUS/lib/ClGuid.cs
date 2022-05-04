using System;

public class ClGuid
{
    public static Guid gUpdate;
    public static Guid gBundle;
    public static Guid gTargetGroup;
    //public ClGuid()
    //{
    //    gUpdate = Guid.NewGuid();
    //    gBundle = Guid.NewGuid();
    //    gTargetGroup = Guid.NewGuid();
    //}

    public static void GenerateUpdateGUID()
    {
        gUpdate = Guid.NewGuid();
    }

    public static void GenerateBundleGUID()
    {
        gBundle = Guid.NewGuid();
    }
    public static void GenerateTargetGroupGUID()
    {
        gTargetGroup = Guid.NewGuid();
    }


}
