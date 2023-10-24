package lab1;

import java.awt.EventQueue;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.border.EmptyBorder;
import javax.swing.filechooser.FileNameExtensionFilter;
import javax.swing.JRadioButton;
import javax.swing.ButtonGroup;
import javax.swing.JButton;
import java.awt.event.ActionListener;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.awt.event.ActionEvent;
import java.awt.FlowLayout;
import javax.swing.JTextArea;
import javax.swing.JTextField;
import javax.swing.JLabel;
import javax.swing.JOptionPane;

import java.awt.Color;

import lab1.Engine.cyph;
import lab1.Engine.lang;

import java.awt.Cursor;
import javax.swing.JScrollPane;
import java.awt.Font;
import javax.swing.UIManager;

import javax.swing.JFileChooser;


@SuppressWarnings("serial")
public class Gui extends JFrame {
	
	public Engine.cyph Mode = cyph.VIGINER;
	public Engine.lang Lang = lang.CYRILLIC;
	

	private JPanel contentPane;
	private final ButtonGroup buttonGroup = new ButtonGroup();
	private JTextField tfKey;
	
	private Engine engine = new Engine();
	
	private JTextArea taCiphertext = new JTextArea();
	private JTextArea taPlaintext = new JTextArea();
	
	private JButton btnEncrypt = new JButton("Encrypt");
	private JButton btnEncryptFile = new JButton("Encrypt File");
	private JButton btnDecrypt = new JButton("Decrypt");
	private JButton btnDecryptFile = new JButton("Decrypt File");
	


	/**
	 * Launch the application.
	 */
	public static void main(String[] args)
	{
		EventQueue.invokeLater(new Runnable()
		{
			public void run()
			{
				try
				{
					Gui frame = new Gui();
					frame.setVisible(true);
				} catch (Exception e)
				{
					e.printStackTrace();
				}
			}
		});
	}
	
	private String extract( JTextArea field ) 
	{
		String Result = "";
		String tmp = field.getText();
		tmp = tmp.toLowerCase();
		for ( int i = 0; i < tmp.length(); i++ )
		{
			if ( isValid( tmp.charAt( i ) ) )
			{
				if ( Mode == cyph.PLAYFAIR && tmp.charAt(i) == 'j' )
					Result += 'i';
				else
					Result += tmp.charAt( i );
			}
		}
		//JOptionPane.showMessageDialog(field, Result);
		return Result;
	}
	
	private void fill( JTextArea src, JTextArea dest, String text )
	{
		String res = "";
		//char[] tmp = ( src.getText().toLowerCase() ).toCharArray();
		char[] tmp = new char[ src.getText().length() + text.length() + 1 ];
		String tmpSrc = src.getText().toLowerCase();
		for ( int p = 0; p < tmpSrc.length(); p++ )
		{
			tmp[p] = tmpSrc.charAt(p);
		}
		int tmpFilled = src.getText().length();
		for ( int p = src.getText().length(); p < tmp.length; p++ )
			tmp[p] = (char) 0;
		
		int i = 0, j = 0;
		
		
			while ( i < text.length() && j < tmpFilled )	// common part
			{
				if ( isValid( tmp[j] ) || tmp[j] == (char) 0 ) // (char 0) is valid here
				{
//					if ( Mode == cyph.PLAYFAIR && text.charAt(i) == 'j' )
//						tmp[j] = 'i';
//					else					
						tmp[j] = text.charAt(i);
					i++;
				}
				j++;
			}
			
			if ( Mode == cyph.PLAYFAIR )
			{
			
				int q = j, t = j;
				while ( j < tmpFilled )	// number of valid symbols in src > length of text
				{
//					if ( ! isValid( text.charAt(j) ) )
//					{
//						tmp[q] = text.charAt(j);
//						q++;
//					}
					if ( isValid( tmp[j] ) )
						tmp[j] = (char) 0;
						
					j++;
				}
	
				q = t;
				while ( i < text.length() )	// length of text > number of valid symbols in src
				{
					tmp[q] = text.charAt(i);
					q++;
					i++;
				}
			
			}
		
//			if ( Mode == cyph.PLAYFAIR )
//			{
//				if ( text.length() >= tmp.length )
//				{
//					
//					for (int k = 0; k < tmp.length; k++)
//						res += tmp[k];
//					
//					//if dest == taCiphertext => if text.length()>src.getTex.length()
//		
//					if ( i != text.length() )
//						for ( ; i < text.length(); i++)
//							res += text.charAt(i);
//				}
//				else if ( text.length() < tmp.length )
//				{
//					int k = 0;
//					int q = 0;
//					while ( k < text.length() )
//					{
//						res += tmp[q];
//						q++;
//						if ( isValid( tmp[q] ) )
//							k++;
//					}
//					
//					while ( q < tmp.length )
//					{
//						if ( ! isValid( tmp[q] ) )
//							res += tmp[q];
//						q++;
//					}
//					
////					if ( i != text.length() )
////						for ( ; i < text.length(); i++)
////							res += text.charAt(i);
//					
//									
//				}
//			}
//		
//		
		for ( int q = 0; q < tmp.length; q++ )
			if ( tmp[q] != (char) 0 )
				res += tmp[q];
		//JOptionPane.showMessageDialog(dest, res);
		dest.setText( res );
	}
	
	private boolean isKeyValid()
	{
		boolean RESULT = true;
		String key = tfKey.getText().toLowerCase();
		//String errString = "";
		
		if ( key.length() == 0 )
			RESULT = false;
		
		if ( Mode == cyph.RAILWAY )
		{
			for ( int i = 0; i < key.length(); i++ )
			{
				if ( key.charAt( i ) < '0' || key.charAt( i ) > '9' )
				{
					RESULT = false;
					//errString += key.charAt( i ) + ' ';
				}
			}
			if ( RESULT == true && key.length() == 1 && key.charAt(0) == '0' )
				RESULT = false;
		}
		else if ( Mode == cyph.VIGINER || Mode == cyph.PLAYFAIR )
		{
			for ( int i = 0; i < key.length(); i++ )
			{
				char c = key.charAt( i );
				if ( ! isValid( c ) )
				{
					RESULT = false;
					//errString += c + ' ';
				}
			}
		}
		else RESULT = false;
		/*
		 * if ( errString != "" ) JOptionPane.showMessageDialog(contentPane,
		 * "Incorrect symbols are:\n"+errString);
		 */
		return RESULT;
	}
	
	private String composeKey()
	{
		String RESULT = "";
		String tmp = tfKey.getText().toLowerCase();
		
		for ( int i = 0; i < tmp.length(); i++ )
		{
			if ( ( Mode == cyph.PLAYFAIR && ! RESULT.contains( tmp.substring( i, i + 1 ) ) ) || Mode == cyph.VIGINER )
				if ( Mode == cyph.PLAYFAIR && tmp.charAt(i) == 'j' )
					RESULT += 'i';
				else
					RESULT += tmp.charAt( i );
		}
		return RESULT;
	}
	
	public boolean isValid(char c)
	{
		if ( Lang == lang.CYRILLIC )
		{
			if ( c <= 'я' && c >= 'а' || c == 'ё')
				return true;
			else
				return false;
		}
		else if ( Lang == lang.LATIN )
		{
			if ( c <= 'z' && c >= 'a' )
				return true;
			else
				return false;
		}
		else return false;
	}
	
	public String getKey()
	{
		return "";
	}
	
	private void EncryptTextArea()
	{
		if ( isKeyValid() )
		{
			engine.setState(Lang, Mode);
			engine.setInput( extract( taPlaintext ), composeKey() );
			try {
				engine.Encrypt();
			} catch (Exception e1) {
				
				e1.printStackTrace();
			}
			fill( taPlaintext, taCiphertext, engine.getOutput() );
		}
		else
		{
			JOptionPane.showMessageDialog(btnEncrypt, "Incorrect symbols of key!");
		}
	}
	
	private void DecryptTextArea()
	{
		if ( isKeyValid() )
		{
			engine.setState(Lang, Mode);
			String tmp = extract( taCiphertext );
			if ( ( Mode == cyph.PLAYFAIR && tmp.length() % 2 == 0 ) || Mode != cyph.PLAYFAIR )
			{
				engine.setInput( tmp, composeKey() );
				try {
					engine.Decrypt();
				} catch (Exception e1) {
					
					e1.printStackTrace();
				}
				//TODO: bug with playfair filling!
				fill( taCiphertext, taPlaintext, engine.getOutput() );
			}
			else 
				JOptionPane.showMessageDialog(btnDecrypt, "Invalid Ciphertext!");
		}
		else
		{
			JOptionPane.showMessageDialog(btnEncrypt, "Incorrect symbols of key!");
		}
	}
	
	

	/**
	 * Create the frame.
	 */
	public Gui()
	{
		setFont(new Font("FreeSans", Font.PLAIN, 14));
		setTitle("Lab1");
		setCursor(Cursor.getPredefinedCursor(Cursor.HAND_CURSOR));
		setResizable(false);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setBounds(100, 100, 665, 462);
		contentPane = new JPanel();
		contentPane.setBackground(Color.LIGHT_GRAY);
		contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));
		setContentPane(contentPane);
		contentPane.setLayout(null);
		
		JPanel panel = new JPanel();
		panel.setBackground(Color.LIGHT_GRAY);
		panel.setBounds(5, 5, 655, 35);
		contentPane.add(panel);
		panel.setLayout(new FlowLayout(FlowLayout.CENTER, 5, 5));
		
		JRadioButton rbtnRailway = new JRadioButton("Railway");
		rbtnRailway.addActionListener(new ActionListener()
		{
			public void actionPerformed(ActionEvent e)
			{
				Mode = cyph.RAILWAY;
				Lang = lang.LATIN;
			}
		});
		rbtnRailway.setBackground(Color.LIGHT_GRAY);
		buttonGroup.add(rbtnRailway);
		panel.add(rbtnRailway);
		
		JRadioButton rbtnViginer = new JRadioButton("Viginer");
		rbtnViginer.addActionListener(new ActionListener() 
		{
			public void actionPerformed(ActionEvent e) 
			{
				Mode = cyph.VIGINER;
				Lang = lang.CYRILLIC;
			}
		});
		rbtnViginer.setBackground(Color.LIGHT_GRAY);
		rbtnViginer.setSelected(true);
		buttonGroup.add(rbtnViginer);
		panel.add(rbtnViginer);
		
		JRadioButton rbtnPlayfair = new JRadioButton("Playfair");
		rbtnPlayfair.addActionListener(new ActionListener() 
		{
			public void actionPerformed(ActionEvent e)
			{
				Mode = cyph.PLAYFAIR;
				Lang = lang.LATIN;
			}
		});
		rbtnPlayfair.setBackground(Color.LIGHT_GRAY);
		buttonGroup.add(rbtnPlayfair);
		panel.add(rbtnPlayfair);
		
		JLabel lblPlaintext = new JLabel("Plaintext");
		lblPlaintext.setFont(UIManager.getFont("Button.font"));
		lblPlaintext.setBounds(15, 52, 60, 17);
		contentPane.add(lblPlaintext);
		
		JLabel lblCiphertext = new JLabel("Ciphertext");
		lblCiphertext.setBounds(327, 52, 85, 17);
		contentPane.add(lblCiphertext);
		
		JLabel lblKey = new JLabel("Key");
		lblKey.setBounds(15, 390, 60, 17);
		contentPane.add(lblKey);
		
		
		
		btnEncryptFile.setBounds(15, 351, 152, 27);
		contentPane.add(btnEncryptFile);
		
		
		btnDecryptFile.setBounds(327, 351, 163, 27);
		contentPane.add(btnDecryptFile);
		
		
		
		

		
		
		btnEncrypt.setBounds(179, 351, 136, 27);
		contentPane.add(btnEncrypt);
		
		
		btnDecrypt.setBounds(502, 351, 151, 27);
		contentPane.add(btnDecrypt);
		
		JLabel lblInfo = new JLabel("");
		lblInfo.setBounds(337, 390, 323, 17);
		contentPane.add(lblInfo);
		
		JScrollPane scrollPane = new JScrollPane();
		scrollPane.setBounds(327, 70, 326, 269);
		contentPane.add(scrollPane);
		
		
		scrollPane.setViewportView(taCiphertext);
		
		JScrollPane scrollPane_1 = new JScrollPane();
		scrollPane_1.setBounds(15, 70, 300, 269);
		contentPane.add(scrollPane_1);
		
		
		scrollPane_1.setViewportView(taPlaintext);
		
		tfKey = new JTextField();
		tfKey.setBounds(41, 388, 612, 21);
		contentPane.add(tfKey);
		tfKey.setColumns(10);
		
		btnEncrypt.addActionListener(new ActionListener() 
		{
			public void actionPerformed(ActionEvent e) 
			{
				EncryptTextArea();
			}
		});

		btnDecrypt.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				
				DecryptTextArea();
				
				
			}
		});
		
		
		
		
		btnEncryptFile.addActionListener(new ActionListener() 
		{
			public void actionPerformed(ActionEvent e) 
			{
				JFileChooser fileopen = new JFileChooser( "/home/andmin/Workspace/Java/Eclipse/InformationTheory-labs/lab1/tests/" );         
				fileopen.setFileSelectionMode( JFileChooser.FILES_ONLY );
				var filter = new FileNameExtensionFilter( "Text File (*.txt)", "txt" );
				fileopen.setDialogTitle( "Open file for encryption" );
				fileopen.setMultiSelectionEnabled( false );
				fileopen.setFileFilter( filter );
				try {
					//int ret = fileopen.showDialog( null, "Open file for encryption" );
					int ret = fileopen.showOpenDialog(taPlaintext);
					if ( ret == JFileChooser.APPROVE_OPTION )
					{
						File file = fileopen.getSelectedFile();
						var path = file.getPath();
						var name = file.getName();
						String input = "";
						
						var cin = new FileReader( path );
						int c = 0;
						while ( (c = cin.read() ) != -1 )
						{
							input += (char) c;
						}
						
						cin.close();
						taPlaintext.setText( input );
						
						EncryptTextArea();
						
						var cout = new FileWriter( path.replace( name, "encr_"+name ) );
						input = taCiphertext.getText();
						cout.write( input );
						cout.close();
					}
				} catch (Exception e1)
				{
					JOptionPane.showMessageDialog( taPlaintext, "ERROR!!!" );
				}
				
				
			}
		});
		
		btnDecryptFile.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				
				JFileChooser fileopen = new JFileChooser( "/home/andmin/Workspace/Java/Eclipse/InformationTheory-labs/lab1/tests/" );         
				fileopen.setFileSelectionMode( JFileChooser.FILES_ONLY );
				var filter = new FileNameExtensionFilter( "Text File (*.txt)", "txt" );
				fileopen.setDialogTitle( "Open file for decryption" );
				fileopen.setMultiSelectionEnabled( false );
				fileopen.setFileFilter( filter );
				try {
					int ret = fileopen.showOpenDialog(taPlaintext);
					if ( ret == JFileChooser.APPROVE_OPTION )
					{
						File file = fileopen.getSelectedFile();
						var path = file.getPath();
						var name = file.getName();
						String input = "";
						
						var cin = new FileReader( path );
						int c = 0;
						while ( (c = cin.read() ) != -1 )
						{
							input += (char) c;
						}
						
						cin.close();
						taCiphertext.setText( input );
						
						DecryptTextArea();
						
						var cout = new FileWriter( path.replace( name, "decr_"+name ) );
						input = taPlaintext.getText();
						cout.write( input );
						cout.close();
					}
				} catch (Exception e1)
				{
					JOptionPane.showMessageDialog( taPlaintext, "ERROR!!!" );
				}
			}
		});
		
	}
}
