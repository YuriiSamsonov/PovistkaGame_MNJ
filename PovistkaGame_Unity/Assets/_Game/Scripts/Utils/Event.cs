using System;

public delegate void Event<T>(T eventArgs) where T : EventArgs;