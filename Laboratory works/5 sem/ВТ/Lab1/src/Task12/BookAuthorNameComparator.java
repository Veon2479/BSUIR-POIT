package Task12;

import java.util.Comparator;

public class BookAuthorNameComparator implements Comparator {
    @Override
    public int compare(Object o, Object t1) {
        String author1 = ((Book)o).getAuthor();
        String author2 = ((Book)t1).getAuthor();
        int res = author1.compareTo(author2);
        if (res == 0)
        {
            res = new BookNameComparator().compare(o, t1);
        }
        return res;
    }
}
