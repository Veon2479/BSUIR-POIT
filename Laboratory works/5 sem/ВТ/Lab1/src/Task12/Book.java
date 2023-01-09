package Task12;

public class Book implements Comparable{
    private String title;
    private String author;
    private int price;
    private static int edition;

    private int isbn;

    public static int getEdition() {
        return edition;
    }

    public int getPrice() {
        return price;
    }

    public String getAuthor() {
        return author;
    }

    public String getTitle() {
        return title;
    }

    @Override
    public boolean equals(Object obj) {
        return (this == obj);
    }

    @Override
    public String toString() {
        return title + " " + author + " " + price + " " + edition;
    }

    @Override
    public int hashCode() {
        int res = super.hashCode();
        for (int i = 0; i < title.length(); i++)
            res += (int)title.charAt(i);
        for (int i = 0; i < author.length(); i++)
            res += (int)author.charAt(i);
        return res + price + edition;
    }

    @Override
    public Object clone()
    {
        Book res = new Book();
        res.price = price;
        res.author = author;
        res.title = title;
        return res;
    }

    @Override
    public int compareTo(Object o) {
        int res = 0;
        if (isbn < ((Book)o).isbn)
            res = -1;
        else if (isbn > ((Book)o).isbn)
            res = 1;
        return res;
    }
}
