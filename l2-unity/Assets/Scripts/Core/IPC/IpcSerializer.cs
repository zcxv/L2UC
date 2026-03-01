namespace IPC {

    public interface IpcSerializer<in T> where T : PacketDto {
        void Serialize(IpcLinkedBuffer buffer, T dto);
    }

}