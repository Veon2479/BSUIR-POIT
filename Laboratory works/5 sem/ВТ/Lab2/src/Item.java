
/** Class for describing items in the "shop"
 */
public class Item {
    /**
     * This item type
     */
    ItemType Type;

    /**
     * This item name
     */
    String Name;

    /**
     * This item price
     */
    int Price;

    /**
     * THis item color
     */
    MyColor Color;

    /**
     * Public constructor for item
     * @param type
     * @param name
     * @param price
     * @param color
     */
    public Item(ItemType type, String name, int price, MyColor color)
    {
        setType(type);
        setName(name);
        setPrice(price);
        setColor(color);
    }


    /**
     * Default constructor
     */
    public Item()
    {

    }

    /**
     * Getter for Type
     * @return Type
     */
    public ItemType getType() {
        return Type;
    }
    /**
     * Setter for Type
     */
    public void setType(ItemType type) {
        Type = type;
    }

    /**
     * getter for name
     * @return name
     */
    public String getName() {
        return Name;
    }

    /**
     * setter for name
     * @param name
     */
    public void setName(String name) {
        Name = name;
    }

    /**
     * Getter for price
     * @return price
     */
    public int getPrice() {
        return Price;
    }

    /**
     * setter for price
     * @param price
     */
    public void setPrice(int price) {
        Price = price;
    }

    /**
     * getter for color
     * @return color
     */
    public MyColor getColor() {
        return Color;
    }


    /**
     * setter for color
     * @param color
     */
    public void setColor(MyColor color) {
        Color = color;
    }
}
