package Task12;

import java.util.Comparator;

public class BookNameComparator implements Comparator {
    @Override
    public int compare(Object o, Object t1) {

        String title1 = ((Book)o).getTitle();
        String title2 = ((Book)t1).getTitle();
        return title1.compareTo(title2);
    }
}
