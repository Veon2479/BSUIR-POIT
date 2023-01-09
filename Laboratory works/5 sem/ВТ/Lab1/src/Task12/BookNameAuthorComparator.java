package Task12;

import java.util.Comparator;

public class BookNameAuthorComparator implements Comparator {
    @Override
    public int compare(Object o, Object t1) {
        int res = new BookNameComparator().compare(o, t1);
        if (res == 0)
        {
            String author1 = ((Book)o).getAuthor();
            String author2 = ((Book)t1).getAuthor();
            res = author1.compareTo(author2);
        }
        return res;
    }
}
