namespace IPC {

    public interface IpcDeserializer<out T> where T : PacketDto {
        T Deserialize(IpcLinkedBuffer buffer);
    }

}