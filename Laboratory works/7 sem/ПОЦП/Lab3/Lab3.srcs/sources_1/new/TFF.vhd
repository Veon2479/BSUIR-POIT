----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/05/2023 09:30:27 PM
-- Design Name: 
-- Module Name: TFF - Behavioral
-- Project Name: 
-- Target Devices: 
-- Tool Versions: 
-- Description: 
-- 
-- Dependencies: 
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
-- 
----------------------------------------------------------------------------------


library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity TFF is
  Port ( 
    CLR, C, T : in std_logic;
    Q : out std_logic
  );
end TFF;

architecture Behavioral of TFF is
    signal s : std_logic;
    signal tx : std_logic;
begin
	DFF: process (CLR, C, tx)
	begin
	       tx <= s xor T;

		if CLR = '1' then
			s <= '0';
		elsif rising_edge(C) then
			s <= tx;
		end if;
	end process;
	
	Q <= s;

end Behavioral;
