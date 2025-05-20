using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tetris.Tools;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Tetris.TetrisNetworking
{
    public class NetworkHelper
    {
        public const int MAX_PLAYER_COUNT = 2;
        
        private const int maxLobbyToShow = 10;
        
        private static readonly List<QueryFilter> _openLobbyFilter = new List<QueryFilter>()
        {
            new QueryFilter(
                field: QueryFilter.FieldOptions.AvailableSlots,
                op: QueryFilter.OpOptions.GT,
                value: "0")
        };
        
        private static readonly List<QueryOrder> _newestOrder = new List<QueryOrder>()
        {
            new QueryOrder(
                asc: false,
                field: QueryOrder.FieldOptions.Created)
        };
        
        private static Dictionary<string, PlayerDataObject> _localUnityServicesData => 
            GameLobby.Instance.currentLobbyUser.GetDataForUnityServices();
        
        private static string localUasId => AuthenticationService.Instance.PlayerId;
        
        public static async UniTask<Lobby> CreateLobby(string lobbyName, int maxPlayers, bool isPrivate)
        {
            var createOptions = new CreateLobbyOptions
            {
                IsPrivate = isPrivate,
                IsLocked = true,
                Player = new Player(id: localUasId, data: _localUnityServicesData),
                Data = null
            };

            var allocation = await AllocateRelay();
            var joinCode = await GetRelayJoinCode(allocation);
            var currentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createOptions);
            Debug.Log($"[LobbyHelper] LobbyCreated with name: {currentLobby.Name} in region: {allocation.Region}");

            await UpdateLobby(currentLobby.Id, new UpdateLobbyOptions()
            {
                Data = new Dictionary<string, DataObject>()
                {
                    { "RelayJoinCode", new DataObject(DataObject.VisibilityOptions.Member, joinCode) }
                },
                IsLocked = false
            });

            var relayServerData = allocation.ToRelayServerData("dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
            return currentLobby;
        }
        
        public static async UniTask<Allocation> AllocateRelay()
        {
            var allocation = await RelayService.Instance.CreateAllocationAsync(MAX_PLAYER_COUNT - 1);
            return allocation;
        }
        
        public static async UniTaskVoid DeleteLobby(string lobbyId)
        {
            await LobbyService.Instance.DeleteLobbyAsync(lobbyId);
        }
    
        public static async UniTask<Lobby> JoinLobbyByCode(string lobbyCode)
        {
            var joinOptions = new JoinLobbyByCodeOptions
            {
                Player = new Player(id: localUasId, data: _localUnityServicesData) 
            };
            return await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, joinOptions);
        }
        
        public static async UniTask<Lobby> JoinLobbyById(string lobbyId)
        {
            var joinOptions = new JoinLobbyByIdOptions
            {
                Player = new Player(id: localUasId, data: _localUnityServicesData) 
            };
            return await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId, joinOptions);
        }
    
        public static async UniTask<Lobby> QuickJoinLobby()
        {
            var joinRequest = new QuickJoinLobbyOptions
            {
                Filter = _openLobbyFilter,
                Player = new Player(id: localUasId, data: _localUnityServicesData)
            };

            try
            {
                //temp
                var queryLobbies = await QueryAllLobbies();
                Debug.Log($"[LobbyHelper] LobiesCount: {queryLobbies.Results.Count}");
                foreach (var lobby in queryLobbies.Results)
                {
                    Debug.Log($"[LobbyHelper] Lobby NAME: {lobby.Name} TRY JOIN ID: {lobby.Id}");
                    var currentLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id);
                    var relayJoinCode = currentLobby.Data["RelayJoinCode"].Value;
                    var joinAllocation = await JoinRelay(relayJoinCode);
                    Debug.Log($"[LobbyHelper] Start connect to {joinAllocation.Region}");
                    NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(joinAllocation.ToRelayServerData("dtls"));
                    NetworkManager.Singleton.StartClient();
                    return currentLobby;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }

            return null;
        }
        
        public static async UniTask<Lobby> ReconnectToLobby(string lobbyId)
        {
            return await LobbyService.Instance.ReconnectToLobbyAsync(lobbyId);
        }
    
        public static async UniTaskVoid RemovePlayerFromLobby(string requesterUasId, string lobbyId)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(lobbyId, requesterUasId);
            }
            catch (LobbyServiceException e)
                when (e is { Reason: LobbyExceptionReason.PlayerNotFound })
            {
                Debug.LogWarning("[LobbyHelper] Removing Already Left Player");
            }
        }
    
        public static async UniTask<QueryResponse> QueryAllLobbies()
        {
            QueryLobbiesOptions queryOptions = new QueryLobbiesOptions
            {
                Count = maxLobbyToShow,
                Filters = _openLobbyFilter,
                Order = _newestOrder
            };

            return await LobbyService.Instance.QueryLobbiesAsync(queryOptions);
        }
    
        public static async UniTask<Lobby> UpdateLobby(string lobbyId, UpdateLobbyOptions updateOptions)
        {
            return await LobbyService.Instance.UpdateLobbyAsync(lobbyId, updateOptions);
        }
    
        public static async UniTask<Lobby> UpdatePlayer(string lobbyId, string playerId, UpdatePlayerOptions updateOptions)
        {
            return await LobbyService.Instance.UpdatePlayerAsync(lobbyId, playerId, updateOptions);
        }
    
        public static async void SendHeartbeatPing(string lobbyId)
        {
            await LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
        }

        public static async UniTask<ILobbyEvents> SubscribeToLobby(string lobbyId, LobbyEventCallbacks eventCallbacks)
        {
            return await LobbyService.Instance.SubscribeToLobbyEventsAsync(lobbyId, eventCallbacks);
        }
        
        public static async UniTask<string> GetRelayJoinCode(Allocation allocation)
        {
            var relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            return relayJoinCode;
        }
    
        public static async UniTask<JoinAllocation> JoinRelay(string relayJoinCode)
        {
            var joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);
            return joinAllocation;
        }
    }
}