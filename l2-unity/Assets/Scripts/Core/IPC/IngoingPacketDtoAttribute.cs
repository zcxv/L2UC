using System;

namespace IPC {

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class IngoingPacketDtoAttribute : Attribute {

    }

}