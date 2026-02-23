# Component

## Bad practice

### Serialize field for debug view

**Bad:** Make private (and public too!) fields is serializable for debug purpouses or with no reason.
**Good:** Doesnt make private fields is serializable. Adding `[NonSerialized]` to public fields. Uses Inspector Debug Mode (... → Debug) for debug purpouses.


