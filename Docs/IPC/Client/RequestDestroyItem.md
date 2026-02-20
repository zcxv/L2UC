### Sends the client to destroy the item [RequestDestroyItem]

### RequestDestroyItem OPCODE 0x59

```Java
	@Override
	protected void readImpl()
	{
		_objectId = readD();
		_count = readD();
	}
```