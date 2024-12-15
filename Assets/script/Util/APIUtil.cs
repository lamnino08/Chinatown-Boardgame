using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Net.Http;

public class APIUtil
{
    public static async Task<List<LobbyServer>> GetAllRoom()
    {
        using (HttpClient client = new HttpClient())
        {
            string serverURL = $"http://{LobbyNetworkManager.instance.serverUrl}/rooms";

            HttpResponseMessage response = await client.GetAsync(serverURL);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            Debug.Log($"Fetched Rooms JSON: {json}");

            // Deserialize JSON into a list of RoomListingData
            return JsonUtility.FromJson<RoomListWrapper>($"{{\"rooms\":{json}}}").rooms;
        }
    }

    [System.Serializable]
    private class RoomListWrapper
    {
        public List<LobbyServer> rooms;
    }
}
