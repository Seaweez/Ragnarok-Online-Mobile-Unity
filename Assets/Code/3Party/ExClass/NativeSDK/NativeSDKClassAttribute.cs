using System;

[AttributeUsage(AttributeTargets.Class)]
public class NativeSDKClassAttribute : Attribute {

}

[AttributeUsage(AttributeTargets.Method)]
public class DoNotNativeSDKAttribute : Attribute {

}
