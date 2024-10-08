using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event;
using Lagrange.Core.Internal.Event.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupFSCreateFolderEvent))]
[Service("OidbSvcTrpcTcp.0x6d7_0")]
internal class GroupFSCreateFolderService : BaseService<GroupFSCreateFolderEvent>
{
    protected override bool Build(GroupFSCreateFolderEvent input, BotKeystore keystore, BotAppInfo appInfo,
        BotDeviceInfo device, out Span<byte> output, out List<Memory<byte>>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D7_0>(new OidbSvcTrpcTcp0x6D7_0
        {
            Create = new OidbSvcTrpcTcp0x6D7_0Create
            {
                GroupUin = input.GroupUin,
                RootDirectory = "/",
                FolderName = input.Name
            }
        }, false, true);
        
        output = packet.Serialize();
        extraPackets = null;
        return true;
    }

    protected override bool Parse(Span<byte> input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GroupFSCreateFolderEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D7Response>>(input);
        
        output = GroupFSCreateFolderEvent.Result(packet.Body.Create.Retcode, packet.Body.Create.RetMsg);
        extraEvents = null;
        return true;
    }
}