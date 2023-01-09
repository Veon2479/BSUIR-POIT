import java.beans.XMLDecoder;
import java.beans.XMLEncoder;
import java.io.*;
import java.util.HashMap;
import java.util.Map;

/**
 * Realization for ODE
 */
public class Database implements DbAbstract{

    private Map<Item, Integer> Items;
    private String DataFilePath = "data.xml";

    @Override
    public boolean AddItem(Item item, int count) {

        boolean res = false;
        if (!IsExist(item))
        {
            Items.put(item, count);
            res = true;
        }
        Flush();
        return res;
    }

    private boolean IsExist(Item item)
    {
        boolean res = true;
        if (Items.get(item) == null)
            res = false;
        return res;
    }
    @Override
    public int GetItemCount(Item item) {
        Integer res = Items.get(item);
        if (res != null)
            return res;
        else
            return 0;
    }

    @Override
    public boolean ChangeItemCount(Item item, int count) {
        boolean res = false;

        Integer oldCount = Items.get(item);
        if (oldCount != null)
        {
            res = true;
        }
        else
        {
            if (oldCount + count < 0)
                res = false;
        }
        if (res)
        {
            Items.replace(item, oldCount, oldCount + count);
            Flush();
        }
        return res;
    }

    @Override
    public boolean DeleteItem(Item item) {
        boolean res = false;

        if (Items.get(item) != null)
        {
            Items.remove(item);
            res = true;
        }

        if (res)
            Flush();
        return res;
    }

    @Override
    public Map<Item, Integer> GetItemsByType(ItemType type) {
        Map<Item, Integer> res = new HashMap<Item, Integer>();
        for (Item item : Items.keySet()) {
            if (item.getType() == type)
            {
                res.put(item, GetItemCount(item));
            }
        }
        return res;
    }

    @Override
    public Item GetCheapestItem() {
        Item minItem = null;
        int minPrice = Integer.MAX_VALUE;
        for (Item item : Items.keySet()) {
            if (item.getPrice() < minPrice)
            {
                minItem = item;
                minPrice = item.getPrice();
            }
        }
        return minItem;
    }

    private void Load()
    {
        File f = new File(DataFilePath);
        if (f.exists())
        {
            try
            {
                XMLDecoder d = new XMLDecoder(new BufferedInputStream(new FileInputStream(DataFilePath)));
                Items = (Map<Item, Integer>) d.readObject();
                d.close();
            }
            catch (Exception e)
            {
                System.out.println("ERR: loading: " + e.getMessage());
            }

        }
        else
        {
            Flush();
        }
    }

    private void Flush()
    {
        try
        {
            XMLEncoder e = new XMLEncoder(new BufferedOutputStream(new FileOutputStream(DataFilePath)));
            e.writeObject(Items);
            e.close();
        }
        catch (Exception e)
        {
            System.out.println("ERR: flushing: " + e.getMessage());
        }
    }

    public Database()
    {
        Items = new HashMap<>();
        Load();
    }
}
