using System;
using System.Reflection;
using CiccioSoft.Data.MariaDbEmbedded.Interop.Native;
using Xunit;

namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Tests;

public sealed class MySqlTests
{
    [Fact]
    public void MethodsThrowObjectDisposedExceptionWhenHandleIsZero()
    {
        using MySql sut = CreateDisposedClient();

        Assert.Throws<ObjectDisposedException>(() => sut.Ping());
        Assert.Throws<ObjectDisposedException>(() => sut.Query("SELECT 1"));
        Assert.Throws<ObjectDisposedException>(() => sut.Connect("localhost", 3306, "root", "root", "db"));
        Assert.Throws<ObjectDisposedException>(() => sut.SetOption(MySqlOption.MYSQL_OPT_CONNECT_TIMEOUT, 1u));
        Assert.Throws<ObjectDisposedException>(() => sut.SetOption(MySqlOption.MYSQL_OPT_RECONNECT, true));
        Assert.Throws<ObjectDisposedException>(() => sut.SetOption(MySqlOption.MYSQL_SET_CHARSET_NAME, "utf8mb4"));
        Assert.Throws<ObjectDisposedException>(() => MySql.GetClientInfo());
        Assert.Throws<ObjectDisposedException>(() => sut.GetServerInfo());
        Assert.Throws<ObjectDisposedException>(() => sut.Error());
    }

    [Fact]
    public void Dispose_IsNoOpWhenAlreadyDisposed()
    {
        using MySql sut = CreateDisposedClient();

        sut.Dispose();
    }

    [Fact]
    public void Query_ReturnType_IsInt()
    {
        MethodInfo method = typeof(MySql).GetMethod(nameof(MySql.Query), [typeof(string)])
            ?? throw new InvalidOperationException("Unable to find Query(string) method.");

        Assert.Equal(typeof(int), method.ReturnType);
    }

    // [Fact]
    // public void MySqlInteropException_DefaultConstructor_InitializesType()
    // {
    //     Exception sut = new MySqlInteropException();

    //     Assert.IsType<MySqlInteropException>(sut);
    // }

    // [Fact]
    // public void MySqlInteropException_MessageConstructor_SetsMessage()
    // {
    //     var sut = new MySqlInteropException("native call failed");

    //     Assert.Equal("native call failed", sut.Message);
    // }

    // [Fact]
    // public void MySqlInteropException_InnerExceptionConstructor_SetsAllValues()
    // {
    //     var inner = new InvalidOperationException("inner");
    //     var sut = new MySqlInteropException("native call failed", inner);

    //     Assert.Equal("native call failed", sut.Message);
    //     Assert.Same(inner, sut.InnerException);
    // }

    private static MySql CreateDisposedClient()
    {
        ConstructorInfo ctor = typeof(MySql).GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            binder: null,
            [typeof(IntPtr)],
            modifiers: null)
            ?? throw new InvalidOperationException("Unable to find private MySql(IntPtr) constructor.");

        return (MySql)ctor.Invoke([IntPtr.Zero]);
    }
}
