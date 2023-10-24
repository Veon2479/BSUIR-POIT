package lab1;

public class Engine {
	
	public Engine() {
		clrState();
	}
	
	public enum lang{
		CYRILLIC,
		LATIN,
		NONE
	}
	
	public enum cyph{
		RAILWAY,
		VIGINER,
		PLAYFAIR,
		NONE
	}
	
	private String Input, Output, Key;
	@SuppressWarnings("unused")
	private lang LangMode;
	private cyph Cypher;
	
	public void setState(lang LangMode, cyph Cypher) {
		this.LangMode = LangMode;
		this.Cypher = Cypher;
	}
	
	private void clrState() {
		this.LangMode = lang.NONE;
		this.Cypher = cyph.NONE;
		this.Input = "";
		this.Output = "";
	}
	
	public void setInput(String Input, String Key) {
		this.Input = Input;
		this.Key = Key;
	}

	public void Encrypt() throws Exception {
		//System.out.println("We will chose the way we enter Valhalla!");
		switch (Cypher) {
		case RAILWAY:
			encrRailway();
			break;
		case VIGINER:
			encrViginer();
			break;
		case PLAYFAIR:
			encrPlayfair();
			break;
		case NONE:
			break;
		}
		
	}
	
	private void encrRailway() throws Exception{
		int key;
		
		key = getNumber(this.Key);
		
		int count = this.Input.length();
		if (key == 1) key = 1;
		else key = 2*key - 2;
		int offset = key;
		int stage = offset;
		int pos = 0;
		while (count > 0){				
			
			this.Output += this.Input.charAt(pos);
			pos += offset;
			count--;
			offset = key - offset;
			if (offset == 0) offset = key;
			if (pos >= this.Input.length() ) {
				stage -= 2;
				offset = stage;
				pos = (key - offset)/2;
				if (offset == 0) offset = key;
			}

			
		}
		
	}
	
	private void encrViginer() {
		for (int i = 0; i < this.Input.length(); i++) {
			int code = getCode(this.Input.charAt(i));
			int newCode = ( ( code + getKey(i) ) % 33 );
			this.Output += (char) getChar( newCode ) ;
		}
		
	}
	
	private int getKey(int pos) {
		int len = this.Key.length();
		int res = (getCode(this.Key.charAt(pos % len)) + (pos / len) ) % 33;
		return res;
		
	}
	//вызывающая программа сразу даст корректный ключ
	private void encrPlayfair() {
		
		char[] Line = new char[this.Input.length() * 2];
		int i = 0, pos = 0;
		while (i < this.Input.length() ) {
			Line[pos] = this.Input.charAt(i);
			pos++;
			
			if ( i+1 == this.Input.length() ) {
				Line[pos] = 'z';
				i++;
			}
			
			else if ( this.Input.charAt(i) == this.Input.charAt(i+1) ) {
				Line[pos] = 'z';
				i++;
			}
			else {
				Line[pos] = this.Input.charAt(i+1);
				i += 2;
			}
			pos++;
		}
		//что будет, если будет биграмма zz????? пусть она не зашифруется
		i = 0;
		int code1 = 0, code2 = 0;
		while ( i < pos ) {
			if ( !( Line[i] == 'z' && Line[i+1] == 'z') ) {
				code1 = getPos(Line[i]);
				code2 = getPos(Line[i+1]);
				
				if ( (code1 % 5) == (code2 % 5) ) {
					this.Output += getPlayfChar( (code1 + 5) % 25);
					this.Output += getPlayfChar( (code2 + 5) % 25);
				}
				else if ( (code1 / 5) == (code2 / 5) ) {
					this.Output += getPlayfChar( (code1 - (code1 % 5) ) + ( (code1 + 1) % 5 ) );
					this.Output += getPlayfChar( (code2 - (code2 % 5) ) + ( (code2 + 1) % 5 ) );
				}
				else {
					//System.out.println(code1 + " " + code2);
					this.Output += getPlayfChar( (code1 - (code1 % 5) ) + ( code2 % 5 ) );
					//System.out.println( (code1 - (code1 % 5) ) + ( code2 % 5 ) );
					this.Output += getPlayfChar( (code2 - (code2 % 5) ) + ( code1 % 5 ) );
					//System.out.println( (code2 - (code2 % 5) ) + ( code1 % 5 ) );
				}
			}
			else
			{
				this.Output += "zz";
			}
			i += 2;
		}		
	}
	
	private int getPos( char c ) {
		if ( this.Key.indexOf( c ) != -1 ) return this.Key.indexOf( c );
		else {
			int res = c - 'a';
			for (int i = 0; i < this.Key.length(); i++) {
				if (this.Key.charAt(i) > c) res++;
			}
			if ( c > 'j') res--;
			return res;
			
//			the matrix (virtual)
//			0  1  2  3  4
//			5  6  7  8  9
//			10 11 12 13 14
//			15 16 17 18 19
//			20 21 22 23 24
			
		}
	}
	
	private char getPlayfChar(int pos) {
		char res = 'Z';
		for (char c = 'a'; c <= 'z'; c++)
		{
			if ( c != 'j' && pos == getPos( c ) )
			{
				res = c;
				break;
			}
		}
		return res;
	}
	
	public void Decrypt() throws Exception {
		
		switch (Cypher) {
		case RAILWAY:
			decrRailway();
			break;
		case VIGINER:
			decrViginer();
			break;
		case PLAYFAIR:
			decrPlayfair();
			break;
		case NONE:
			break;
		}
		
	}
	
	private void decrRailway() throws Exception {
		
		int key = getNumber(this.Key);	 
		int count = 0;
		if (key == 1) key = 1;
		else key = 2*key - 2;
		int offset = key;
		int stage = offset;
		int pos = 0;
		
		char[] tmp = new char[this.Input.length()];
		
		while (count < this.Input.length()){		
			
			tmp[pos] = this.Input.charAt(count);
			
			pos += offset;
			count++;
			offset = key - offset;
			if (offset == 0) offset = key;
			if (pos >= this.Input.length() ) {
				stage -= 2;
				offset = stage;
				pos = (key - offset)/2;
				if (offset == 0) offset = key;
			}
		}
		this.Output = new String(tmp);

		
		
	}
	
	private void decrViginer() {
		for (int i = 0; i < this.Input.length(); i++) {
			int code = getCode(this.Input.charAt(i));
			int newCode = getChar( ( ( code + 33 - getKey(i) ) % 33 ) );
			this.Output += (char) newCode;
		}
	}
	
	//если идти справа налево по расшифрованной строке, то нулевой символ всегда будет вторым в биграмме
	private void decrPlayfair() {
		char[] Line = new char[this.Input.length() * 2];
		int i = 0, pos = 0;
		while (i < this.Input.length() ) {
			Line[i] = this.Input.charAt(i);
			i++;
			Line[i] = this.Input.charAt(i);
			i++;
		}
		pos = i - 1;
		//что будет, если будет биграмма zz????? пусть она не расшифруется

		
		char lastChar = (char) 0;
		int code1 = 0, code2 = 0;
		while (pos > 0)
		{
			if ( !( Line[pos - 1] == 'z' && Line[pos] == 'z' ) )
			{
				//decipher bigramm
				code1 = getPos( Line[pos - 1] );
				code2 = getPos( Line[pos] );
				
				if ( (code1 % 5) == (code2 % 5) ) {
					Line[ pos - 1 ] = getPlayfChar( (code1 + 25 - 5) % 25);
					Line[ pos ] = getPlayfChar( (code2 + 25 - 5) % 25);
				}
				else if ( (code1 / 5) == (code2 / 5) ) {
					Line[ pos - 1 ] = getPlayfChar( (code1 - (code1 % 5) ) + ( (code1 + 5 - 1) % 5 ) );
					Line[ pos ] = getPlayfChar( (code2 - (code2 % 5) ) + ( (code2 + 5 - 1) % 5 ) );
				}
				else {
					Line[ pos - 1 ] = getPlayfChar( (code1 - (code1 % 5) ) + ( code2 % 5 ) );
					Line[ pos ] = getPlayfChar( (code2 - (code2 % 5) ) + ( code1 % 5 ) );
				}
					
			}
			
			if ( Line[pos] == 'z' && ( ( lastChar == (char) 0  ) || ( lastChar == Line[pos - 1] ) ) ) 
			{
				Line[pos] = (char) 0;
			}
						
			lastChar = Line[pos - 1];
			pos -= 2;
			
			
		}
		
		for (pos = 0; pos < Line.length; pos++)
		{
			if ( Line[ pos ] != (char) 0 )
				this.Output += Line[ pos ];
		}
		
	
		
		
	}
	
	public String getOutput() {
		String tmp = this.Output;
		clrState();
		return tmp;
	}
	
	public static int getNumber(String in) throws Exception {	//проверка на то, что число
		int RESULT = 0;
		int i = 0, tmp = 0;
		if (in.length() == 0) throw new Exception();
		while (i < in.length()) {
			tmp = in.charAt(i)-'0';
			if (tmp > 9) throw new Exception();
			RESULT = RESULT*10 + tmp;
			i++;
		}
		return RESULT;
	}
	
	public static int getCode(char c) {
		if (c == 'ё') return 6;
		if (c > 'е') return (int) c - 'а' + 1;
		else return (int) c - 'а';
	}
	
	public static char getChar(int code) {
		if (code == 6) return 'ё';
		if (code > 6) return (char) (code - 1 + 'а' );
		else return (char) (code + 'а' );
		
	}
	
}
