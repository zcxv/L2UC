namespace IPC {
    
    /// <summary>
    /// DTO interface representing data for an outgoing packet.
    /// Implementations must guarantee immutability.
    ///
    /// Purpose: Ensures data integrity during I/O operations by preventing state changes 
    /// and eliminates read-lock contention (lock-free access).
    /// </summary>
    public interface PacketDto {
        
    }
}