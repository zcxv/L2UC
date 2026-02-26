using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

public class LockFreeQueue<T> {
    private Node head; 
    private Node tail;

    private void EnqueueWithContention(Node currentTail, Node newNode) {
        var tmp = currentTail;
        do {
            currentTail = tmp;
            tmp = Interlocked.CompareExchange(ref currentTail.Next, newNode, null);
        } while (tmp is not null);

        // attempt to install a new tail. Do not retry if failed, competing thread installed more recent version of it
        Interlocked.CompareExchange(ref tail, newNode, currentTail);
    }

    public void Enqueue(T item) {
        var newNode = new Node(item);
        if (Interlocked.CompareExchange(ref tail, newNode, null) is { } currentTail) {
            EnqueueWithContention(currentTail, newNode);
        } else {
            head = newNode;
        }
    }

    public bool TryDequeue([MaybeNullWhen(false)] out T result) {
        if (Volatile.Read(ref head) is { } currentHead)
            return TryDequeueWithContention(currentHead, out result);

        result = default;
        return false;
    }

    private bool TryDequeueWithContention(Node currentHead, [MaybeNullWhen(false)] out T value) {
        for (Node newHead, tmp; !currentHead.TryRead(out value); currentHead = ReferenceEquals(tmp, currentHead) ? newHead : tmp) {
            newHead = currentHead.Next;

            if (newHead is null) {
                value = default;
                return false;
            }

            tmp = Interlocked.CompareExchange(ref head, newHead, currentHead);
        }

        return true;
    }

    private sealed class Node {
        internal Node Next;
        private T value;
        private volatile int visited;

        internal Node(T value) {
            this.value = value;
        }

        internal bool TryRead([MaybeNullWhen(false)] out T result) {
            if (Interlocked.Exchange(ref visited, 1) is 0) {
                result = value;
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                    value = default!;
                }

                return true;
            }

            result = default;
            return false;
        }
    }
}