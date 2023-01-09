package Task12;

public class ProgrammerBook extends Book{
    private String language;
    private int level;

    public String getLanguage() {
        return language;
    }

    public int getLevel() {
        return level;
    }

    @Override
    public String toString() {
        return super.toString() + " " + language + " " + level;
    }

    @Override
    public boolean equals(Object obj) {
        return (this == obj);
    }

    @Override
    public int hashCode() {
        int res = super.hashCode();
        for (int i = 0; i < language.length(); i++)
            res += (int)language.charAt(i);
        return res + level;
    }
}
