using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

namespace Tetris.TetrisNetworking
{
    public class LobbyUser
    {
        public bool IsHost { get; private set; }
        public string Nickname { get; private set; }
        public string Id { get; private set; }

        private readonly Dictionary<string, PlayerDataObject> _dataForServices = new()
        {
            { "DisplayName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, "") }
        };

        public LobbyUser(string id, string nickName)
        {
            IsHost = false;
            Nickname = nickName;
            Id = id;

            _dataForServices["DisplayName"].Value = Nickname;
        }

        public void SetNickName(string nickname)
        {
            Nickname = nickname;
            _dataForServices["DisplayName"].Value = Nickname;
        }

        public void SetIsHost(bool isHost)
        {
            IsHost = isHost;
        }

        public void SetId(string id)
        {
            Id = id;
        }

        public Dictionary<string, PlayerDataObject> GetDataForUnityServices() => _dataForServices;
    }
}