using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RelayData {
    public static bool isHost;
    public static string JoinCode;
    public static string IPv4Address;
    public static ushort Port;
    public static Guid AllocationID;
    public static byte[] AllocationIDBytes;
    public static byte[] ConnectionData;
    public static byte[] HostConnectionData;
    public static byte[] Key;
}