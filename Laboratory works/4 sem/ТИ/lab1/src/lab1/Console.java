package lab1;

import lab1.Engine.cyph;
import lab1.Engine.lang;

public class Console {
	


	public static void main(String[] args) {
		Engine engine = new Engine();
		engine.setState(lang.LATIN, cyph.PLAYFAIR);
		//engine.setInput("theciphertext", "key");
		engine.setInput("pizza", "key");
		try {
			engine.Encrypt();
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		String tmp = engine.getOutput();
		System.out.println("ciphertext: "+tmp);
		engine.setState(lang.LATIN, cyph.PLAYFAIR);
		engine.setInput(tmp, "key");
		try {
			engine.Decrypt();
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		System.out.println("plaintext:  "+engine.getOutput());
		System.out.println("Done");
//		for (char i = 'а'; i<='я'; i++)
//			System.out.println(i-'а'+" "+i);
//		System.out.println('ё'-'а'+" "+'ё');
//		System.out.println((char) (32+'а'));

	}
	
	
	//String newCopy = String.copyValueOf(first.toCharArray());
	
	private String filterPlayfkey(String key) {
		return "";
	}
	
	private String extract(String plain, lang LangMode) {
		String Result = "";
		
		
		return Result;
	}
	
	private String fill(String cypher, String plain, lang LangMode) {
		String Result = "";
		
		
		return Result;
	}

}
