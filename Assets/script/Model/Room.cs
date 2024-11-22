using System.Collections.Generic;
using System.Diagnostics;

public class Room 
{
    private List<byte> _colors = new List<byte>{0,1,2,3,4};
    private byte[] _tile = new byte[85]; // 85 tile
    private byte[] _store = new byte[12]; // 12 type of store card
    public List<byte> colors => _colors;
    public byte year { get; private set; }
    
    public Room()
    {
        for (int i = 0; i < 12; i++)
        {
            _store[i] = (byte)(i / 3 + 6);
        }

        year = 0;
    }

    public void RemoveColor(byte color)
    {
        _colors.Remove(color);
    }

    public List<byte[]> DistributeTileCard(int numberPlayer)
    {
        // if (numberPlayer < 3 || numberPlayer > 5) return null;

        int numberTile = Util.NumberTileCard(year, numberPlayer);
        List<byte[]> tiles = new List<byte[]>();
        for (int playerIndex = 0; playerIndex < numberPlayer; playerIndex++)
        {
            byte[] tilesOfPlayer = new byte[numberTile];

            System.Random random = new System.Random();
            {
                for (int i = 0; i < numberTile; i++)
                {
                    int randomIndex;
                    do
                    {
                        randomIndex = random.Next(0, 85);
                    } while (_tile[randomIndex] != 0); 

                   tilesOfPlayer[i] = (byte)randomIndex; 
                    _tile[randomIndex] = (byte)(playerIndex); 
                }
            }
            tiles.Add(tilesOfPlayer);
        }
        return tiles;
    }

    public List<byte[]> DistributeStoreCard(int numberPlayer)
    {
        // if (numberPlayer < 3 || numberPlayer > 5) return null;

        int numberStoreCard = Util.NumberStoreCard(year, numberPlayer);
        List<byte[]> storeCards = new List<byte[]>();
        for (int playerIndex = 0; playerIndex < numberPlayer; playerIndex++)
        {
            byte[] tilesOfPlayer = new byte[numberStoreCard];

            System.Random random = new System.Random();
            {
                for (int i = 0; i < numberStoreCard; i++)
                {
                    int randomIndex;
                    do
                    {
                        randomIndex = random.Next(0, 12);
                    } while (_store[randomIndex] > 0); 

                   tilesOfPlayer[i] = (byte)randomIndex; 
                    _tile[randomIndex] = (byte)(playerIndex); 
                }
            }
            storeCards.Add(tilesOfPlayer);
        }
        return storeCards;
    }

    public List<byte[]> NewYear(int numberPlayer)
    {
        year++;
        return DistributeTileCard(numberPlayer);
    }
}
