package Task12;

import java.util.Comparator;

public class BookAuthorNamePriceComparator implements Comparator {
    @Override
    public int compare(Object o, Object t1) {
        int res = new BookAuthorNameComparator().compare(o, t1);
        if (res == 0)
        {
            int price1 = ((Book)o).getPrice();
            int price2 = ((Book)t1).getPrice();
            if (price1 < price2)
                res = 1;
            else if (price2 < price1)
                res = -1;
        }
        return res;
    }
}
