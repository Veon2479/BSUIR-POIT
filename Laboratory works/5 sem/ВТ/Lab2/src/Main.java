import java.util.Map;
import java.util.Scanner;

/** Main class with UI functions
 */

public class Main {

    private static DbAbstract db;

    private static void Init()
    {
        db = (DbAbstract) new Database();
    }

    /** Entry point for program, contains UI loop
     * @param args - cmd arguments
     */

    public static void main(String[] args) {
        Init();

        Scanner sc = new Scanner(System.in);
        boolean fl = true;

        while (fl)
        {
            System.out.println();
            System.out.println("Type number and press Enter:");
            System.out.println("1 for adding item");
            System.out.println("2 for showing all items");
            System.out.println("===========================");
            System.out.println("3 for searching for all kettles");
            System.out.println("4 for finding cheapest item");
            System.out.println("===========================");
            System.out.println("5 for exit");
            System.out.println();

            System.out.print("> ");

            String tmp = sc.nextLine().replaceAll("\\s","");

            switch (tmp.charAt(0)) {
                case '1' -> AddItem();
                case '2' -> ShowItems();
                case '3' -> SearchType();
                case '4' -> SearchCheapest();
                case '5' -> fl = false;
                default -> {
                    System.out.println("Incorrect input!!!!");
                    System.out.println();
                }
            }

        }


    }

    private static void AddItem()
    {
        System.out.println("Select item type: ");
        System.out.println("0 for Fridge");
        System.out.println("1 for Kettle");
        System.out.println("2 for Book");


        int code = getInt(0, 2);
        ItemType type = ItemType.values()[code];

        System.out.println("Enter item name: ");
        String name = getString();

        System.out.println("Select item color: ");
        System.out.println("0 for Blue");
        System.out.println("1 for Red");
        System.out.println("2 for White");
        System.out.println("3 for Black");

        MyColor color = MyColor.values()[getInt(0, 3)];

        System.out.println("Enter item price: ");
        int price = getInt(0, Integer.MAX_VALUE);

        System.out.println("Enter item count: ");
        int count = getInt(0, Integer.MAX_VALUE);

        db.AddItem(new Item(type, name, price, color), count);

    }

    private static void ShowItems()
    {
        for ( ItemType type : ItemType.values()) {
            Map<Item, Integer> map = db.GetItemsByType(type);
            for ( Item item : map.keySet()) {
                System.out.println("Type: " + item.getType() + ", Name: " + item.getName() +
                                    ", Price: " + item.getPrice() + ", Color: " + item.getColor() +
                                    ", Count: " + map.get(item));
            }
        }
    }

    private static void SearchType()
    {
        Map<Item, Integer> map = db.GetItemsByType(ItemType.Kettle);
        for ( Item item : map.keySet()) {
            System.out.println("Type: " + item.getType() + ", Name: " + item.getName() +
                    ", Price: " + item.getPrice() + ", Color: " + item.getColor() +
                    ", Count: " + map.get(item));
        }
    }

    private static void SearchCheapest()
    {
        Item item = db.GetCheapestItem();
        if (item != null)
        {
            System.out.println("The cheapest item is:");
            System.out.println("Type: " + item.getType() + ", Name: " + item.getName() +
                    ", Price: " + item.getPrice() + ", Color: " + item.getColor());
        }
        else
        {
            System.out.println("No items!");
        }
    }

    private static int getInt(int a, int b)
    {
        int res = 0;
        Scanner sc = new Scanner(System.in);
        boolean fl = true;

        while (fl)
        {
            System.out.print("> ");
            try
            {
                res = sc.nextInt();
                if (res >= b && res < a)
                    throw new Exception();
                fl = false;
            }
            catch (Exception e)
            {
                System.out.println("Incorrect input, try again!!!");
            }
        }

        return res;
    }

    private static String getString()
    {
        Scanner sc = new Scanner(System.in);
        System.out.print("> ");
        return sc.nextLine();
    }

}