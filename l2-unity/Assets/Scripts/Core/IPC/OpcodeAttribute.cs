using System;

namespace IPC {

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class OpcodeAttribute : Attribute {
        public int[] Opcode { get; private set; }

        public OpcodeAttribute(params int[] opcode) {
            this.Opcode = opcode;
        }
    }

}