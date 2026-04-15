using System;
using System.Reflection;
using CiccioSoft.Data.MariaDbEmbedded.Interop.IA;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.IA.Tests;

public sealed class MySqlClientTests
{
    [Fact]
    public void MethodsThrowObjectDisposedExceptionWhenHandleIsZero()
    {
        using MySqlClient sut = CreateDisposedClient();

        Assert.Throws<ObjectDisposedException>(() => sut.Ping());
        Assert.Throws<ObjectDisposedException>(() => sut.Query("SELECT 1"));
        Assert.Throws<ObjectDisposedException>(() => sut.SetOption(MySqlOption.ConnectTimeout, 1u));
        Assert.Throws<ObjectDisposedException>(() => sut.SetOption(MySqlOption.Reconnect, true));
        Assert.Throws<ObjectDisposedException>(() => sut.SetOption(MySqlOption.SetCharsetName, "utf8mb4"));
    }

    [Fact]
    public void Dispose_IsNoOpWhenAlreadyDisposed()
    {
        using MySqlClient sut = CreateDisposedClient();

        sut.Dispose();
    }

    [Fact]
    public void QueryReturnsOkEnumValueIsZero()
    {
        Assert.Equal(0, (int)MySqlResultCode.Ok);
    }

    [Fact]
    public void ErrorEnumValueIsOne()
    {
        Assert.Equal(1, (int)MySqlResultCode.Error);
    }

    [Fact]
    public void MySqlInteropException_DefaultConstructor_InitializesType()
    {
        Exception sut = new MySqlInteropException();

        Assert.IsType<MySqlInteropException>(sut);
    }

    [Fact]
    public void MySqlInteropException_MessageConstructor_SetsMessage()
    {
        var sut = new MySqlInteropException("native call failed");

        Assert.Equal("native call failed", sut.Message);
    }

    [Fact]
    public void MySqlInteropException_InnerExceptionConstructor_SetsAllValues()
    {
        var inner = new InvalidOperationException("inner");
        var sut = new MySqlInteropException("native call failed", inner);

        Assert.Equal("native call failed", sut.Message);
        Assert.Same(inner, sut.InnerException);
    }

    private static MySqlClient CreateDisposedClient()
    {
        ConstructorInfo ctor = typeof(MySqlClient).GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            binder: null,
            [typeof(IntPtr)],
            modifiers: null)
            ?? throw new InvalidOperationException("Unable to find private MySqlClient(IntPtr) constructor.");

        return (MySqlClient)ctor.Invoke([IntPtr.Zero]);
    }
}
