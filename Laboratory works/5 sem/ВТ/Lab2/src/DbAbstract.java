import java.util.Map;

/**
 * DAO layer for using database
 */
public interface DbAbstract {

    /**
     * Put item in the db
     * @param item
     * @param count
     * @return operation status
     */
    public boolean AddItem(Item item, int count);

    /**
     * Get number of described items in the db
     * @param item
     * @return count
     */
    public int GetItemCount(Item item);

    /**
     * Change number of described item in db
     * @param item
     * @param count
     * @return operation status
     */
    public boolean ChangeItemCount(Item item, int count);

    /**
     * Delete item from db
     * @param item
     * @return operation status
     */
    public boolean DeleteItem(Item item);

    /**
     * Get all items of described type from db
     * @param type
     * @return map of (item, count)
     */
    public Map<Item, Integer> GetItemsByType(ItemType type);

    /**
     * Get first item, that costs less than others
     * @return that item
     */
    public Item GetCheapestItem();
}
